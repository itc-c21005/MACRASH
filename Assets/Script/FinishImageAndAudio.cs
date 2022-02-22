using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishImageAndAudio : MonoBehaviour
{

    public EnemyScript ES;
    public PlayerController PC;
    public TimeText timeSC;

    public AudioSource Result;
    public AudioClip WinAudio;
    public AudioClip LoseAudio;
    public AudioClip TimeUPAudio;

    public CanvasRenderer[] ImageCR;

    bool Playflag = true;

    //ImageCR[0] = win [1] = Lose

    // Start is called before the first frame update
    void Start()
    {
        foreach(CanvasRenderer CR in ImageCR)
        {
            CR.SetAlpha(0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PC.Loseflag) StartCoroutine(LoseImage());
        if (ES.Winflag) StartCoroutine(WinImage());
        if (timeSC.TimeUP) TimeUPImage();
    }

    //勝利画像を表示
    IEnumerator WinImage()
    {
        yield return new WaitForSeconds(3);
        ImageCR[0].SetAlpha(1);
        if (Playflag)
        {
            Result.PlayOneShot(WinAudio);
            Playflag = !Playflag;
        }
    }
    //敗北画像の表示
    IEnumerator LoseImage()
    {
        yield return new WaitForSeconds(3);
        ImageCR[1].SetAlpha(1);
        if (Playflag)
        {
            Result.PlayOneShot(LoseAudio);
            Playflag = !Playflag;
        }
    }

    //タイムアップ画像を表示
    void TimeUPImage()
    {
        ImageCR[2].SetAlpha(1);
        if (Playflag)
        {
            Result.PlayOneShot(TimeUPAudio);
            Playflag = !Playflag;
        }
    }
}
