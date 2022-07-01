using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;


public class SphereController : MonoBehaviour
{
    public float speed = 4f;
    public Rigidbody SphereRigidbody;
    private bool action_flag;// 신호 들어왔는지 확인하는 플래그
    public bool collision_flag ; // 충돌했는지 확인하는 플래
    private int current_direction;
    //public static SphereController instance = null;
    //float inputX, inputZ;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start CharacterMove");
        SphereRigidbody = GetComponent<Rigidbody>();
        action_flag = true;
        collision_flag = false;
        current_direction = 0;


        //Server.Instance.SetTouchCallback(new CallbackTouch(OnTouch));
        Server.Instance.SetDirectionCallback(new CallbackDirection(OnDirection)); //static Server 반환됨
        //윗줄은 callbackDirection = new CallbackDirection(OnDirection); 이거랑 같은 말임
        // 델리게이트에 메서드 추가하는것과 같음
        // 호출 순서 : 데이터 수신 받음 -> HandleIncommingRequest메서드 실행 -> DirectionHandler 메서드 실행 -> callbackDirection 대리자 실행 --최종--> Ondirection 메서드 실행
        //             <-----------------------------------------------------Server class--------------------------------------------------->           <----SphereController->
    }
    void OnCollisionEnter(Collision col){
        //if(col.gameObject.name == "OutsideWall" || col.gameObject.name == "InsideWall")
        //{
        //    Debug.Log("OnCollisionEnter");
        //}
        //Debug.Log("X: " + SphereRigidbody.position.x.ToString() + " Y: " + SphereRigidbody.position.y.ToString() + " Z: " + SphereRigidbody.position.z.ToString());
        collision_flag = true;
        Debug.Log(col.gameObject.name);
        Debug.Log("충돌진입");
    }
    private void OnCollisionStay(Collision col)
    {
        //collision_flag = true;
        //Debug.Log("충돌중");
    }
    private void OnCollisionExit(Collision col)
    {
        collision_flag = false;
        Debug.Log("충돌나옴");
    }

    // Update is called once per frame
    void Update()
    {
        if (action_flag == false) // 신호가 들어왔으면
        {
            switch (current_direction)
            {
                case 0://위쪽 방향 -z방향
                    transform.Translate(new Vector3(0, 0, -1) * speed);
                    break;
                case 1://아래쪽 방향 z방향
                    transform.Translate(new Vector3(0, 0, 1) * speed);
                    break;
                case 2: //오른쪽 방향 -x 방향
                    transform.Translate(new Vector3(-1, 0, 0) * speed);
                    break;
                case 3: // 왼쪽 방향 x 방향
                    transform.Translate(new Vector3(1, 0, 0) * speed);
                    break;
            }
            //Server.Instance.m_SendPacket.position_x = SphereRigidbody.position.x;
            //Server.Instance.m_SendPacket.position_z = SphereRigidbody.position.z;
            //Server.Instance.m_SendPacket.collision_flag = collision_flag;
            Server.Instance.position_x = SphereRigidbody.position.x;
            Server.Instance.position_z = SphereRigidbody.position.z;
            if (collision_flag == true)
            {
                Server.Instance.is_collision = 1;
            }
            else
            {
                Server.Instance.is_collision = 0;
            }
        }
        
        
        action_flag = true;

    }
    void OnDirection(int direction)
    {
        Debug.Log("CharacterMove : " + direction);
        action_flag = false; // 행동 신호 들어옴
        current_direction = direction;
        
        //datapacket.position_x = SphereRigidbody.position.x;
        //datapacket.position_z = SphereRigidbody.position.z;
        //if (collision_flag == true)//부딪히면 
        //{
        //    datapacket.is_collision = 1;
        //}
        //else// 안부딪히
        //{
        //    datapacket.is_collision = 0;
        //}

        //byte[] buffer = new byte[Marshal.SizeOf(datapacket)];
        //unsafe
        //{
        //    fixed (byte* fixed_buffer = buffer)
        //    {
        //        Marshal.StructureToPtr(datapacket, (IntPtr)fixed_buffer, false);
        //    }
        //}

        //stream.Write(buffer, 0, Marshal.SizeOf(datapacket));

        //stream.Flush();
        //switch (direction)
        //{
        //    case 0://위쪽 방향 -z방향
        //        //inputZ = 1f;
        //        //inputX = 0f;
        //        transform.Translate(new Vector3(0,0,-1)*speed);
        //        break;
        //    case 1://아래쪽 방향 z방향
        //        //inputZ = -1f;
        //        //inputX = 0f;
        //        transform.Translate(new Vector3(0,0,1)*speed);
        //        break;
        //    case 2: //오른쪽 방향 -x 방향
        //        //inputX = 1f;
        //        //inputZ = 0f;
        //        transform.Translate(new Vector3(-1,0,0)*speed);
        //        break;
        //    case 3: // 왼쪽 방향 x 방향
        //        //inputX = -1f;
        //        //inputZ = 0f;
        //        transform.Translate(new Vector3(1,0,0)*speed);
        //        break;
        //}
    }
}
[StructLayout(LayoutKind.Sequential)]
public class DataPacket
{
    [MarshalAs(UnmanagedType.R4)]
    public float position_x;
    [MarshalAs(UnmanagedType.R4)]
    public float position_z;
    [MarshalAs(UnmanagedType.I4)]
    public int is_collision;
    //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
    //public string Name;
    //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
    //public string Subject;
    //[MarshalAs(UnmanagedType.I4)]
    //public int Grade;
    //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
    //public string Memo;
}