using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BosBGM : MonoBehaviour
{

    public AudioSource BGM;
    public PlayerController PC;
    public EnemyScript ES;
    public TimeText timesc;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(PC.Loseflag || ES.Winflag || timesc.TimeUP)
        {
            BGM.Stop();
        }
    }
}
