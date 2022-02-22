using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public GameObject Player;
    public PlayerController PC;

    //枕object
    public GameObject EMAKURA;
    public MakuraShot makurasc;


    //目的地更新のためのポインタ
    public GameObject[] pointers;

    //Zマークを格納する配列
    public SpriteRenderer[] Zimage;

    //エフェクト
    public GameObject effectobj;

    public string PR;
    public string PL;

    //歩行速度
    public float RunSpeed;

    //pointerのタグ
    public string pointertag;

    //目的地を決めるためのランダム関数
    private int RandomInt;

    //敵が枕を投げてくるクールタイム
    public float CoolTime;

    //先生のアニメーション
    public Animator TCanim;

    //プレイヤーの弾
    public string pwepon;

    //影
    public GameObject Shadow;

    //Zアイコン管理変数
    public int Sleep = 0;
    public int MaxSleep;

    public int moveint = 0;

    //エリアの端かどうかを判断する
    public bool moveEndR = false;
    public bool moveEndL = false;

    public TimeText timeSC;
    bool startset = true;

    //勝った時のフラグ建て
    public bool Winflag = false;

    //敵のSE
    public AudioSource EnemyAudio;
    public AudioClip HitSE;
    public AudioClip ThrowSE;

    //Hit中かどうかのフラグ
    public bool hitflag = false;

    //難易度”大人げない”かのフラグ
    bool otonagenai = false;

    // Start is called before the first frame update
    void Start()
    {

        //Zマークをすべて半透明に
        foreach (SpriteRenderer SR in Zimage)
        {
            SR.color = new Color(1, 1, 1, 0.3f);
        }

        if (GameControll.otonagenaiflag)
        {
            otonagenai = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //常時プレイヤーの方向を見る
        transform.LookAt(Player.transform);

        EnemyMove();

        //カウントダウンの間＆負けフラグが立ったときは動かない
        if (timeSC.startflag && !PC.Loseflag)
        {
            if (startset)
            {
                StartCoroutine(Shot());

                StartCoroutine(Move());

                startset = !startset;
            }
        }

        if (Winflag)
        {
            if (Shadow.transform.position.z < 7)
            {
                Shadow.transform.Translate(0, -2 * Time.deltaTime, 0);
            }
        }

    }

    //アニメーションによって移動する方向を変える
    void EnemyMove()
    {
        if (!PC.Loseflag && !timeSC.TimeUP)
        {
            if (!hitflag)
            {
                Transform EnemyTransform = transform;
                Vector3 vct = EnemyTransform.position;

                switch (moveint)
                {
                    case 0:
                        EnemyTransform.position = vct;
                        break;

                    case 1:
                        vct.x = Mathf.Clamp(transform.position.x + RunSpeed * Time.deltaTime, -8, 8);
                        EnemyTransform.position = vct;
                        break;

                    case 2:
                        vct.x = Mathf.Clamp(transform.position.x - RunSpeed * Time.deltaTime, -8, 8);
                        EnemyTransform.position = vct;
                        break;
                }
            }
        }
    }
    //枕が当たったらｚマークを更新してヒット＆ダウンアニメーションを再生する
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == pwepon)
        {
            Sleep++;
            Zimage[Sleep - 1].color = new Color(1, 1, 1, 1);

            if (Sleep == MaxSleep)
            {
                if (!timeSC.TimeUP)
                {
                    Winflag = true;
                    EnemyAudio.PlayOneShot(HitSE);
                    TCanim.SetTrigger("Down");
                    Instantiate(effectobj, transform.position + transform.forward * 1.5f + transform.up, transform.rotation);
                }
            }
            else
            {
                EnemyAudio.PlayOneShot(HitSE);
                TCanim.SetTrigger("Hit");
                hitflag = true;
                Instantiate(effectobj, transform.position + transform.forward * 1.5f + transform.up, transform.rotation);
            }
        }
    }

    //壁際にいるフラグを立てる
    private void OnTriggerStay(Collider other)
    {

        if (other.gameObject.tag == PR) moveEndR = true;
        if (other.gameObject.tag == PL) moveEndL = true;

    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == PR) moveEndR = false;
        if (other.gameObject.tag == PL) moveEndL = false;
    }

    IEnumerator Move()
    {
        //端にいる間は壁に向かって移動しないようにする
        if (!PC.Loseflag && !timeSC.TimeUP)
        {
            if (moveEndR) TCanim.SetTrigger("Left");

            if (moveEndL) TCanim.SetTrigger("Right");

            if (!moveEndR && !moveEndL)
            {
                RandomInt = Random.Range(1, 3);
                //ランダムで左右の移動アニメーションを決定する
                switch (RandomInt)
                {

                    case 1:

                        TCanim.SetTrigger("Left");
                        break;

                    case 2:

                        TCanim.SetTrigger("Right");
                        break;

                }
            }

            yield return new WaitForSeconds(2);

            StartCoroutine(Move());
        }

    }


    //枕を投げる関数
    IEnumerator Shot()
    {
        yield return new WaitForSeconds(CoolTime);
        if (!PC.Loseflag && !timeSC.TimeUP)
        {
            TCanim.SetTrigger("Throw");
            StartCoroutine(Shot());
        }
    }

    //枕を投げるアニメーションが行われた際に呼ばれる関数
    public void Throw()
    {
        EnemyAudio.PlayOneShot(ThrowSE);
        if (!PC.Loseflag && !timeSC.TimeUP && !Winflag)
        {
            if (otonagenai)
            {
                int ThrowSpeed = Random.Range(1, 4);
                switch (ThrowSpeed)
                {
                    case 1:
                        makurasc.shotspeed = 400;
                        break;

                    case 2:
                        makurasc.shotspeed = 600;
                        break;

                    case 3:
                        makurasc.shotspeed = 700;
                        break;

                }
            }
            EMAKURA.transform.rotation = transform.rotation;
            EMAKURA.transform.position = transform.position + transform.forward * 2 + transform.up;
            makurasc.stayflag = false;
        }
    }

    //移動のアニメーションが行われた際に呼ばれる関数
    public int MoveInt(int move)
    {
        return moveint = move;
    }


    public void HitReset()
    {
        hitflag = false;
    }
}
