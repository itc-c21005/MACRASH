using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    //画面タッチのスクリプト
    [SerializeField]
    ScreenInput Input;

    //プレイヤーの移動速度
    public float playerspeed;

    //敵のスクリプト取得
    public EnemyScript ES;

    //枕object
    public GameObject PMAKURA;
    public MakuraShot makurasc;

    //投げる（キャッチ）ボタンのスクリプト
    public BottunCT BCT;

    //キャッチ動作のスクリプト
    public CatchScript catchSC;

    //エフェクト
    public GameObject effectobj;

    //移動抽選用の変数
    public int moveint = 0;

    //プレイヤーのアニメーション
    public Animator Panim;

    public string Ewepon;

    public int PlayerHP = 0;

    //プレイヤーZマークUI
    public CanvasRenderer[] Zimage;

    public TimeText timeSC;

    //bool Delayflag;

    //影
    public GameObject Shadow;

    //負けたフラグを立てる
    public bool Loseflag = false;

    //プレイヤー周りの音
    public AudioSource PlayerSE;
    public AudioClip HitSE;
    public AudioClip ThrowSE;
    public AudioClip CatchSE;

    //ヒット判定
    public bool hitflag = false;

    //キャッチ判定
    public bool catchflag = false;

    // Start is called before the first frame update
    void Start()
    {
        //Zマークをすべて半透明化
        foreach (CanvasRenderer CR in Zimage)
        {
            CR.SetAlpha(0.3f);
        }
    }

    void Update()
    {
        //最初のカウントダウンが終わり次第操作を可能にする
        if (timeSC.startflag)
        {
            PlayerMoveSelect();
            PlayerMove();

            if (transform.position.x == 8 || transform.position.x == -8)
            {
                BCT.ReloadMakura();
            }
        }

        //勝利したフラグが立てばモーションを行う
        if (ES.Winflag)
        {
            StartCoroutine(WinAnim());
        }

        if (Loseflag)
        {
            if(Shadow.transform.position.z > -10)
            {
                Shadow.transform.Translate(0, -2 * Time.deltaTime, 0);
            }
        }

    }

    //投げるアニメーション
    public void Shot()
    {
        if (timeSC.startflag && !timeSC.TimeUP && !ES.Winflag)
        {
            Panim.SetTrigger("Throw");
        }
    }

    //キャッチアニメーション
    public void Catch()
    {
        if (timeSC.startflag && !timeSC.TimeUP && !ES.Winflag)
        {
            Panim.SetTrigger("Catch");
        }
    }

    //勝利アニメーション
    IEnumerator WinAnim()
    {
        yield return new WaitForSeconds(3);
        Panim.SetTrigger("Win");
    }

    //キャッチのアニメーションが行われた際に呼ばれる関数
    public void AnimCatch()
    {
        if (catchSC.catchtrigger)
        {
            catchSC.Destoryflag = true;
            catchflag = true;
            PlayerSE.PlayOneShot(CatchSE);
            BCT.ReloadMakura();
            StartCoroutine(CatchflagOff());
 
        }
    }

    //投げるアニメーションが行われた際に呼ばれる関数
    public void Throw()
    {
        if (!timeSC.TimeUP)
        {
            PlayerSE.PlayOneShot(ThrowSE);
            PMAKURA.transform.position
                = new Vector3(transform.position.x, 
                              transform.position.y + 1,
                              transform.position.z + 2f);
            makurasc.stayflag = false;
        }
    }


    void PlayerMoveSelect()
    {
        //勝利する or 制限時間が無くなると操作ができなくなる
        if (!ES.Winflag && !timeSC.TimeUP)
        {
            switch (Input.GetNowSwipe())
            {

                case ScreenInput.SwipeDirection.LEFT:
                    if (Panim.GetCurrentAnimatorStateInfo(0).IsName("Idle") && !Panim.IsInTransition(0)) Panim.SetTrigger("Left");
                    break;
                case ScreenInput.SwipeDirection.RIGHT:
                    if (Panim.GetCurrentAnimatorStateInfo(0).IsName("Idle") && !Panim.IsInTransition(0)) Panim.SetTrigger("Right");
                    break;
            }
        }
    }

    //アニメーションによって移動の方向を変える
    public void PlayerMove()
    {

        if (!hitflag)
        {
            Transform PlayerTransform = transform;
            Vector3 vct = PlayerTransform.position;

            switch (moveint)
            {
                case 0:
                    PlayerTransform.position = vct;
                    break;

                case 1:
                    vct.x = Mathf.Clamp(transform.position.x - playerspeed * Time.deltaTime, -8, 8);
                    PlayerTransform.position = vct;
                    break;

                case 2:
                    vct.x = Mathf.Clamp(transform.position.x + playerspeed * Time.deltaTime, -8, 8);
                    PlayerTransform.position = vct;
                    break;
            }
        }

    }

    //アニメーションから引数を受け取る
    public int MoveInt(int move)
    {
        return moveint = move;
    }

    //枕に当たった時のモーション
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == Ewepon)
        {
            if (!catchflag)
            {
                PlayerHP++;
                Zimage[PlayerHP - 1].SetAlpha(1);
                //当たった時とやられたときのアニメーション分け
                if (PlayerHP == 3)
                {
                    Loseflag = true;  //負けフラグをたてる
                    PlayerSE.PlayOneShot(HitSE);
                    Panim.SetTrigger("Down");
                    Instantiate(effectobj, transform.position + transform.forward * 1.5f + transform.up, transform.rotation);
                }
                else
                {
                    PlayerSE.PlayOneShot(HitSE);
                    Panim.SetTrigger("Hit");
                    hitflag = true;
                    Instantiate(effectobj, transform.position + transform.forward * 1.5f + transform.up, transform.rotation);
                }
            }
        }
    }

    public void HitReset()
    {
        hitflag = false;
    }

    //無敵時間にならないようにflagを切っておく
    IEnumerator CatchflagOff()
    {
        yield return new WaitForSeconds(1f);
        catchflag = false;
    }

}

