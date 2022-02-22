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

    //制限時間
    public float Timer;
    public Text timetext;

    //最初の待機時間
    float firstcount = 4;
    public Text counttext;

    int firstcountInt;

    //開始の判定
    public bool startflag = false;

    //終了の判定
    public bool TimeUP = false;

    //スタート音声
    public AudioSource StartAudio;

    // Start is called before the first frame update
    void Start()
    {
        //カウントダウン用の画像を一度すべて透明化
        foreach (CanvasRenderer CR in CountDownImages)
        {
            CR.SetAlpha(0);
        }

        BlackPanel.SetAlpha(0);
    }

    // Update is called once per frame
    void Update()
    {

        First();　//カウントダウン開始

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

    //カウントダウン（whileだと動かなかったので一定数になると処理を終える）
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

    //制限時間
    void TimeCounter()
    {
        if (!PC.Loseflag && !ES.Winflag)
        {
            Timer = Mathf.Clamp(Timer - 1 * Time.deltaTime, 0, 60);
        }
    }

    //透明化
    void AllAlphaZero()
    {
        foreach (CanvasRenderer CR in CountDownImages)
        {
            CR.SetAlpha(0);
        }
    }
}
