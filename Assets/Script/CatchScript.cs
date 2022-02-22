using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchScript : MonoBehaviour
{

    public bool catchtrigger = false;
    public bool Destoryflag = false;

    public string Ewepon;
    public GameObject EMAKURA;
    public MakuraShot makurasc;

    int count = 0;

    // Update is called once per frame
    void Update()
    {
        if (catchtrigger)
        {
            count++;
            if(count > 20)
            {
                catchtrigger = false;
                count = 0;
            }
        }
        else
        {
            count = 0;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == Ewepon)
        {
            catchtrigger = true;
            if (Destoryflag)
            {
                EMAKURA.transform.position = new Vector3(2, -5, 0);
                makurasc.stayflag = true;
                Destoryflag = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        catchtrigger = false;
        Destoryflag = false;
    }

}
