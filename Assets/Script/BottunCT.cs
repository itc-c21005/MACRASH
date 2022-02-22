using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]

public class BottunCT : MonoBehaviour
{

    Button ShotButton;
    public Image ShotImage;

    public CatchBottunScript CatchBsc;

    public TimeText timeSC;

    // Start is called before the first frame update
    void Start()
    {

        ShotButton = gameObject.GetComponent<Button>();
    }


    public void OffShotBottun()
    {
        if (timeSC.startflag)
        {
            if (!timeSC.TimeUP)
            {
                //������{�^��������
                ShotButton.interactable = false;
                ShotImage.enabled = false;

                CatchBsc.OnCatch();
            }
        }
    }

    public void ReloadMakura()
    {

        CatchBsc.OffCatch();

        //������{�^�����o��
        ShotButton.interactable = true;
        ShotImage.enabled = true;

    }

}
