using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereController : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.W)) //위 -z방향
        {
            transform.Translate(new Vector3 (0,0,-1)*Time.deltaTime *2);
        }
        if(Input.GetKey(KeyCode.D)) //오른쪽 -x 방향
        {
            transform.Translate(new Vector3 (-1,0,0)*Time.deltaTime *2);
        }
        if(Input.GetKey(KeyCode.S)) //오른쪽 z 방향
        {
            transform.Translate(new Vector3 (0,0,1)*Time.deltaTime *2);
        }
        if(Input.GetKey(KeyCode.A)) //오른쪽 x 방향
        {
            transform.Translate(new Vector3 (1,0,0)*Time.deltaTime *2);
        }
    }
}
