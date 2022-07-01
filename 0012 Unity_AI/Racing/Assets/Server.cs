using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;
public delegate void CallbackDirection(int direction);// delegate 선언

public class Server : MonoBehaviour
{
    #region private members
    private TcpListener tcpListener;
    private Thread tcpListenerThread;
    private TcpClient connectedTcpClient;
    #endregion
    public DataPacket datapacket;
    //public SphereController sphere = null;
    //public float Position_X, Position_Z;
    //public bool Collisition_flag;
    public float position_x = 0f;
    public float position_z = 0f;
    public int is_collision = 0;
    public static Server instance = null;
    CallbackDirection callbackDirection; // delegate(대리자)

    private void Awake() // 게임이 시작되기전, 모든 변수와 게임의 상태를 초기화하기 위해서 호출
                         // Start()보다 먼저 호출됨, MonoBehaviour.Awake()
    {
        Debug.Log("Start Server");
        instance = this; // this = Server
        //m_SendPacket.position_x = 0.0f;
        //m_SendPacket.position_z= 0.0f;
        //m_SendPacket.collision_flag = false;
        // Start TcpServer background thread
        //sphere = SphereController.Instance;
        datapacket = new DataPacket();
        //sphere = SphereController.Instance;
        tcpListenerThread = new Thread(new ThreadStart(ListenForIncommingRequest));
        tcpListenerThread.IsBackground = true;
        tcpListenerThread.Start();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static Server Instance
    {
        get
        {
            if(instance == null)
            {
                return null;
            }
            return instance;
        }
    }
    
    public void SetDirectionCallback(CallbackDirection callback)// 매개변수 delegate 타입
    {
        if (callbackDirection == null)
        {
            callbackDirection = callback;
        }
        else
        {
            callbackDirection += callback;
        }
    }

    private void ListenForIncommingRequest()
    {
        try
        {
            // Create listener on 192.168.200.179 port 50001
            tcpListener = new TcpListener(IPAddress.Parse("192.168.200.108"),50001);
            tcpListener.Start();
            Debug.Log("Server is listening");
            while(true)
            {
                using(connectedTcpClient = tcpListener.AcceptTcpClient())// 문장형태의 using문, IDisposable 객체의 올바른 사용을 보장하는 편리한 구문 제공
                                                                         // 관리되지 않는 리소스에 액세스하는 대표적인 클래스들, 사용한 후에 적절한 시기에 해제(Dispose)하여 
                                                                         // 해당 리소스를 반납, using 문을 벗어나면 자동으로 해제함
                {
                    using(NetworkStream stream = connectedTcpClient.GetStream()) // Client로 Stream받기
                    {
                        do
                        {
                            //Byte[] bytesTypeOfService = new Byte[4];
                            //Byte[] bytesDisplayId = new Byte[4];
                            //Byte[] bytesPayloadLength = new Byte[4];
                            
                            //int lengthTypeOfService = stream.Read(bytesTypeOfService, 0, 4);
                            //int lengthDisplayId= stream.Read(bytesDisplayId,0,4);
                            //int lengthPayloadLength = stream.Read(bytesPayloadLength,0,4);
                            
                            //if (lengthTypeOfService <= 0 && lengthDisplayId <= 0 && lengthPayloadLength <=0 )
                            //{
                            //    break;
                            //}
                            
                            //if (!BitConverter.IsLittleEndian)//System.BitConverter, 기본-> Byte의 배열로, 바이트 배열을 기본 데이터형식으로 변환
                            //                                 // IsLittleEndian : 이 컴퓨터 아키텍처에서 데이터가 저장되는 바이트 순서를 나타냄
                            //{
                            //    Array.Reverse(bytesTypeOfService);
                            //    Array.Reverse(bytesDisplayId);
                            //    Array.Reverse(bytesPayloadLength);
                            //}
                            //int typeOfService = BitConverter.ToInt32(bytesTypeOfService,0);
                            //int displayId = BitConverter.ToInt32(bytesDisplayId,0);
                            //int payloadLength = BitConverter.ToInt32(bytesPayloadLength,0);
                            
                            //if (typeOfService == 3)
                            //{
                            //    payloadLength = 1012;
                            //}
                            Byte[] bytes = new Byte[4];
                            //int length = stream.Read(bytes,0,payloadLength);
                            stream.Read(bytes, 0, 4);//데이터 읽
                            int direction = BitConverter.ToInt32(bytes, 0);//byte -> int 로 변환 
                            callbackDirection(direction);// 받은 action signal --> sphere 전달

                            datapacket.position_x = this.position_x;
                            datapacket.position_z = this.position_z;
                            datapacket.is_collision = this.is_collision;

                            byte[] buffer = new byte[Marshal.SizeOf(datapacket)];
                            unsafe
                            {
                                fixed (byte* fixed_buffer = buffer)
                                {
                                    Marshal.StructureToPtr(datapacket, (IntPtr)fixed_buffer, false);
                                }
                            }

                            stream.Write(buffer, 0, Marshal.SizeOf(datapacket));
                            Debug.Log("보낸 데이터 크기는 : "+Marshal.SizeOf(datapacket));
                            stream.Flush();
                            //HandleIncommingRequest(typeOfService,displayId,payloadLength,bytes); // param : 1,0,4, byte[4]
                            //HandleIncommingRequest(bytes);
                            //DirectionHandler(bytes);
                            //datapacket.m_StringlVariable = "datagood";
                            //byte[] sendPacket = StructToByteArray(m_SendPacket);
                            //Debug.Log(sendPacket.Length);
                            //connectedTcpClient.Client.Send(sendPacket, 0, sendPacket.Length, SocketFlags.None);
                            //Debug.Log("데이터 전송 완/");
                            //connectedTcpClient.Send
                        } while(true);
                    }
                }
            }
        }
        catch (SocketException socketException)
        {

            Debug.Log("SocketException " + socketException.ToString());
        }
    }
    //byte[] StructToByteArray(object obj)
    //{
    //    int size = Marshal.SizeOf(obj);
    //    byte[] arr = new byte[size];
    //    IntPtr ptr = Marshal.AllocHGlobal(size);

    //    Marshal.StructureToPtr(obj, ptr, true);
    //    Marshal.Copy(ptr, arr, 0, size);
    //    Marshal.FreeHGlobal(ptr);
    //    return arr;
    //}
    // Handle incomming request
    //private void HandleIncommingRequest(int typeOfService, int displayId, int payloadLength, byte[] bytes)
    //private void HandleIncommingRequest(byte[] bytes)
    //{
    //    Debug.Log("=========================================");
    //    Debug.Log("Type of Service : " + typeOfService);
    //    Debug.Log("Display Id      : " + displayId);
    //    Debug.Log("Payload Length  : " + payloadLength);
    //    //DirectionHandler(displayId, payloadLength, bytes);
    //    DirectionHandler(bytes);
    //}
    // Handle Direction Signal
    //private void DirectionHandler(byte[] bytes)
    //{
    //    Debug.Log("Execute Direction Handler");
    //    int direction = BitConverter.ToInt32(bytes, 0);
    //    Debug.Log("Direction  : " + direction);
    //    if(callbackDirection != null)
    //    {
    //        callbackDirection(direction);
    //    }
    //}
}


