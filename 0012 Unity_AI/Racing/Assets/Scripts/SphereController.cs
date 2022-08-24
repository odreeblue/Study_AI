using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net.Sockets;
using UnityEngine.SceneManagement;


//public delegate void CallbackPosAndCol(float x, float y, int isCol);// delegate 선언
public class SphereController : MonoBehaviour
{
    public float speed;
    public Rigidbody SphereRigidbody;
    public bool collision_flag ; // 충돌했는지 확인하는 플래
    public Vector3 move;
    public Vector3 SubjectPosition;
    public Vector3 SpherePosition;
    public int game_done;
    void Start()
    {
        //Time.timeScale = 1f;
        Debug.Log("Start CharacterMove");
        SphereRigidbody = GetComponent<Rigidbody>();
        //action_flag = false;
        collision_flag = false;
        //current_direction = 0;
        Debug.Log("초기 x 위치는 : " + SphereRigidbody.position.x);
        Debug.Log("초기 z 위치는 : " + SphereRigidbody.position.z);
        SpherePosition = SphereRigidbody.transform.position;
        Debug.Log("SpherePosition초기 x 위치는 : " + SpherePosition.x);
        Debug.Log("SpherePosition초기 y 위치는 : " + SpherePosition.y);
        Debug.Log("SpherePosition초기 z 위치는 : " + SpherePosition.z);
        speed = 5.0f;
        //SpherePosition.y = 0.0f;
        Debug.Log("SpherePosition 수정 후 y 위치는 : " + SpherePosition.y);
        move = new Vector3(0, 0, 0);
        //Server.Instance.SetTouchCallback(new CallbackTouch(OnTouch));
        Server.Instance.SetDirectionCallback(new CallbackDirection(OnDirection)); //static Server 반환됨
    }
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name.Substring(0,3) == "Out" || col.gameObject.name.Substring(0,3) == "InS")
        {
            collision_flag = true;
            Debug.Log(col.gameObject.name);
        }
    }
    private void OnCollisionStay(Collision col)
    {
        if (col.gameObject.name.Substring(0, 3) == "Out" || col.gameObject.name.Substring(0, 3) == "InS")
        {
            collision_flag = true;
            Debug.Log(col.gameObject.name);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(move != new Vector3(0,0,0))// move가 입력이되었고 게임이 끝나지 않았으면
        {
            SphereRigidbody.transform.Translate(move * speed * Time.deltaTime);
        }
    }
    void LateUpdate()
    {
        if(move != new Vector3(0, 0, 0))
        {
            float epsilon = 0.1f;
            float loss_x = SphereRigidbody.transform.position.x - SubjectPosition.x;
            //float loss_y = SphereRigidbody.transform.position.y - SubjectPosition.y;
            float loss_z = SphereRigidbody.transform.position.z - SubjectPosition.z;

            float abs_xz = Math.Abs(loss_x)+ Math.Abs(loss_z);
            Debug.Log("SphereRigidbody.x: " + SphereRigidbody.transform.position.x + "-SubjectPosition.x :"+ SubjectPosition.x + " ,loss x : " + loss_x);
            Debug.Log("SphereRigidbody.z: " + SphereRigidbody.transform.position.z + "-SubjectPosition.z :" + SubjectPosition.z + " ,loss z : " + loss_z);
            //Debug.Log("loss z : " + loss_z);
            Debug.Log("total abs loss : " + abs_xz + "  Collision_flag "+ collision_flag + "  move : " + move[0]+","+move[1]+","+move[2]);
            // 목표 도달 & 충돌 없음
            if ((abs_xz <= epsilon) && collision_flag == false && move != new Vector3(0, 0, 0))
            {
                Server.Instance.SendData.Add(SphereRigidbody.transform.position.x);
                Server.Instance.SendData.Add(SphereRigidbody.transform.position.z);
                if(SphereRigidbody.transform.position.x < -3 && SphereRigidbody.transform.position.z > 3) // 목표에 도달했다면
                {
                    Server.Instance.SendData.Add(2.0f);
                    SphereRigidbody.transform.position = new Vector3(4.0f, 3.0f, -4.0f);
                    SpherePosition = new Vector3(4.0f, 0.0f, -4.0f);
                }
                else // 목표에 도달하지 않았다1
                {
                    Server.Instance.SendData.Add(0.0f);
                    SpherePosition = SphereRigidbody.transform.position;
                }
                
                move = new Vector3(0, 0, 0);
                
            }
            // 목표 도달 & 충돌 있음
            else if ((abs_xz <= epsilon) && collision_flag == true && move != new Vector3(0, 0, 0))
            {
                Server.Instance.SendData.Add(SphereRigidbody.transform.position.x);
                Server.Instance.SendData.Add(SphereRigidbody.transform.position.z);
                Server.Instance.SendData.Add(1.0f);
                move = new Vector3(0, 0, 0);
                //SphereRigidbody.transform.position = new Vector3(4.0f, 3.0f, -4.0f);
                collision_flag = false;
                //SpherePosition = new Vector3(4.0f, 0.0f, -4.0f);
                SpherePosition = SphereRigidbody.transform.position;
            }
            // 목표 미도달 & 충돌 있음
            else if ((abs_xz > epsilon) && collision_flag == true && move != new Vector3(0, 0, 0))
            {
                Server.Instance.SendData.Add(SphereRigidbody.transform.position.x);
                Server.Instance.SendData.Add(SphereRigidbody.transform.position.z);
                Server.Instance.SendData.Add(1.0f);
                move = new Vector3(0, 0, 0);
                //SphereRigidbody.transform.position = new Vector3(4.0f, 3.0f, -4.0f);
                collision_flag = false;
                //SpherePosition = new Vector3(4.0f, 0.0f, -4.0f);
                SpherePosition = SphereRigidbody.transform.position;
            }
        }
        
    }

    void OnDirection(int direction, int done)
    {
        Debug.Log("CharacterMove : " + direction);

        switch(direction)
        {
            case 0: //위쪽 방향 -z방향
                move = new Vector3(0, 0, -1f);
                break;
            case 1: //아래쪽 방향 z방향
                move = new Vector3(0, 0, 1f);
                break;
            case 2: //오른쪽 방향 -x 방향
                move = new Vector3(-1f, 0, 0);
                break;
            case 3: // 왼쪽 방향 x 방향
                move = new Vector3(1f, 0, 0);
                break;
        }
        this.game_done = done;
        if (game_done != 0)
        {
            move = new Vector3(0, 0, 0);
            SphereRigidbody.transform.position = new Vector3(4.0f, 3.0f, -4.0f);
            SpherePosition = new Vector3(4.0f, 0.0f, -4.0f);
            Server.Instance.SendData.Add(SphereRigidbody.transform.position.x); //python 에 전달되었을 때 의미 없지만 통신을 위해 
            Server.Instance.SendData.Add(SphereRigidbody.transform.position.z); //python 에 전달되었을 때 의미 없지만 통신을 위해 
            Server.Instance.SendData.Add(1.0f);  //python 에 전달되었을 때 의미 없지만 통신을 위해 
        }
        SubjectPosition = SpherePosition + move;

    }
}
