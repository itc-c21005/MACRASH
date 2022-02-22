using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public GameObject Player;
    public PlayerController PC;

    //��object
    public GameObject EMAKURA;
    public MakuraShot makurasc;


    //�ړI�n�X�V�̂��߂̃|�C���^
    public GameObject[] pointers;

    //Z�}�[�N���i�[����z��
    public SpriteRenderer[] Zimage;

    //�G�t�F�N�g
    public GameObject effectobj;

    public string PR;
    public string PL;

    //���s���x
    public float RunSpeed;

    //pointer�̃^�O
    public string pointertag;

    //�ړI�n�����߂邽�߂̃����_���֐�
    private int RandomInt;

    //�G�����𓊂��Ă���N�[���^�C��
    public float CoolTime;

    //�搶�̃A�j���[�V����
    public Animator TCanim;

    //�v���C���[�̒e
    public string pwepon;

    //�e
    public GameObject Shadow;

    //Z�A�C�R���Ǘ��ϐ�
    public int Sleep = 0;
    public int MaxSleep;

    public int moveint = 0;

    //�G���A�̒[���ǂ����𔻒f����
    public bool moveEndR = false;
    public bool moveEndL = false;

    public TimeText timeSC;
    bool startset = true;

    //���������̃t���O����
    public bool Winflag = false;

    //�G��SE
    public AudioSource EnemyAudio;
    public AudioClip HitSE;
    public AudioClip ThrowSE;

    //Hit�����ǂ����̃t���O
    public bool hitflag = false;

    //��Փx�h��l���Ȃ��h���̃t���O
    bool otonagenai = false;

    // Start is called before the first frame update
    void Start()
    {

        //Z�}�[�N�����ׂĔ�������
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
        //�펞�v���C���[�̕���������
        transform.LookAt(Player.transform);

        EnemyMove();

        //�J�E���g�_�E���̊ԁ������t���O���������Ƃ��͓����Ȃ�
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

    //�A�j���[�V�����ɂ���Ĉړ����������ς���
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
    //�������������炚�}�[�N���X�V���ăq�b�g���_�E���A�j���[�V�������Đ�����
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

    //�Ǎۂɂ���t���O�𗧂Ă�
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
        //�[�ɂ���Ԃ͕ǂɌ������Ĉړ����Ȃ��悤�ɂ���
        if (!PC.Loseflag && !timeSC.TimeUP)
        {
            if (moveEndR) TCanim.SetTrigger("Left");

            if (moveEndL) TCanim.SetTrigger("Right");

            if (!moveEndR && !moveEndL)
            {
                RandomInt = Random.Range(1, 3);
                //�����_���ō��E�̈ړ��A�j���[�V���������肷��
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


    //���𓊂���֐�
    IEnumerator Shot()
    {
        yield return new WaitForSeconds(CoolTime);
        if (!PC.Loseflag && !timeSC.TimeUP)
        {
            TCanim.SetTrigger("Throw");
            StartCoroutine(Shot());
        }
    }

    //���𓊂���A�j���[�V�������s��ꂽ�ۂɌĂ΂��֐�
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

    //�ړ��̃A�j���[�V�������s��ꂽ�ۂɌĂ΂��֐�
    public int MoveInt(int move)
    {
        return moveint = move;
    }


    public void HitReset()
    {
        hitflag = false;
    }
}
