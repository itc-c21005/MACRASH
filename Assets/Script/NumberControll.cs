using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberControll : MonoBehaviour
{

    public CanvasRenderer[] ImageCR;

    public int nowNumber;
    int cangenumber;

    // Start is called before the first frame update
    void Start()
    {
        AllClear();
        cangenumber = nowNumber;
    }

    // Update is called once per frame
    void Update()
    {
        if (cangenumber != nowNumber && nowNumber < 10)
        {
            OnNumber();
            cangenumber = nowNumber;
        }

        if (nowNumber >= 10)
        {
            AllClear();
        }
    }

    void AllClear()
    {
        foreach (CanvasRenderer CR in ImageCR)
        {
            CR.SetAlpha(0);

        }
    }

    void OnNumber()
    {
        ImageCR[cangenumber].SetAlpha(0);
        ImageCR[nowNumber].SetAlpha(1);
    }
}
