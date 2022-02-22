using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakuraShot : MonoBehaviour
{

    Rigidbody rb;

    public float shotspeed;

    //Player�^�O
    public string Player;

    //��Q���^�O
    public string Wool;

    //�ҋ@��Ԃ��ǂ�����bool
    public bool stayflag;

    // Start is called before the first frame update
    void Start()
    {
        stayflag = true;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!stayflag)
        {
            rb.velocity = transform.forward * shotspeed * Time.deltaTime;
        }
        else
        {
            rb.velocity = transform.forward * 0;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!stayflag)
        {
            transform.position = new Vector3(0, -5, 0);
            stayflag = true;
        }
    }

}
