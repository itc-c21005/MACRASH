using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeText : MonoBehaviour
{

    public PlayerController PC;
    public EnemyScript ES;

    public NumberControll[] NumCl;

    public CanvasRenderer[] CountDownImages;

    public CanvasRenderer BlackPanel;

    //��������
    public float Timer;
    public Text timetext;

    //�ŏ��̑ҋ@����
    float firstcount = 4;
    public Text counttext;

    int firstcountInt;

    //�J�n�̔���
    public bool startflag = false;

    //�I���̔���
    public bool TimeUP = false;

    //�X�^�[�g����
    public AudioSource StartAudio;

    // Start is called before the first frame update
    void Start()
    {
        //�J�E���g�_�E���p�̉摜����x���ׂē�����
        foreach (CanvasRenderer CR in CountDownImages)
        {
            CR.SetAlpha(0);
        }

        BlackPanel.SetAlpha(0);
    }

    // Update is called once per frame
    void Update()
    {

        First();�@//�J�E���g�_�E���J�n

        if (startflag)
        {
            TimeCounter();
        }

        if (Timer == 0)
        {
            BlackPanel.SetAlpha(1);
            TimeUP = true;
        }

        int TimerInt = (int)Timer;
        string timeText = TimerInt + "";
        timetext.text = timeText;
        string[] GetTimeOne = new string[2];
        int keta = timeText.Length;

        for (int i = 0; i < 2; i++)
        {
            if (i < keta)
            {

                GetTimeOne[i] = timeText.Substring(keta - (i + 1), 1);
                NumCl[i].nowNumber = int.Parse(GetTimeOne[i]);
            }
            else
            {
                NumCl[i].nowNumber = 10;
            }


        }
    }

    //�J�E���g�_�E���iwhile���Ɠ����Ȃ������̂ň�萔�ɂȂ�Ə������I����j
    void First()
    {
        firstcount = firstcount - 1.2f * Time.fixedDeltaTime;
        firstcountInt = (int)firstcount;
        if (firstcount > 0)
        {
            AllAlphaZero();
            CountDownImages[firstcountInt].SetAlpha(1);
            if (firstcountInt == 1) StartAudio.Play();
        }

        if (firstcountInt == 0)
        {

            AllAlphaZero();
            CountDownImages[firstcountInt].SetAlpha(1);
        }
        if (firstcountInt == -1)
        {
            CountDownImages[firstcountInt + 1].SetAlpha(0);
            startflag = true;
        }

        if (firstcountInt < -2)
        {
            return;
        }
    }

    //��������
    void TimeCounter()
    {
        if (!PC.Loseflag && !ES.Winflag)
        {
            Timer = Mathf.Clamp(Timer - 1 * Time.deltaTime, 0, 60);
        }
    }

    //������
    void AllAlphaZero()
    {
        foreach (CanvasRenderer CR in CountDownImages)
        {
            CR.SetAlpha(0);
        }
    }
}
