using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    //��ʃ^�b�`�̃X�N���v�g
    [SerializeField]
    ScreenInput Input;

    //�v���C���[�̈ړ����x
    public float playerspeed;

    //�G�̃X�N���v�g�擾
    public EnemyScript ES;

    //��object
    public GameObject PMAKURA;
    public MakuraShot makurasc;

    //������i�L���b�`�j�{�^���̃X�N���v�g
    public BottunCT BCT;

    //�L���b�`����̃X�N���v�g
    public CatchScript catchSC;

    //�G�t�F�N�g
    public GameObject effectobj;

    //�ړ����I�p�̕ϐ�
    public int moveint = 0;

    //�v���C���[�̃A�j���[�V����
    public Animator Panim;

    public string Ewepon;

    public int PlayerHP = 0;

    //�v���C���[Z�}�[�NUI
    public CanvasRenderer[] Zimage;

    public TimeText timeSC;

    //bool Delayflag;

    //�e
    public GameObject Shadow;

    //�������t���O�𗧂Ă�
    public bool Loseflag = false;

    //�v���C���[����̉�
    public AudioSource PlayerSE;
    public AudioClip HitSE;
    public AudioClip ThrowSE;
    public AudioClip CatchSE;

    //�q�b�g����
    public bool hitflag = false;

    //�L���b�`����
    public bool catchflag = false;

    // Start is called before the first frame update
    void Start()
    {
        //Z�}�[�N�����ׂĔ�������
        foreach (CanvasRenderer CR in Zimage)
        {
            CR.SetAlpha(0.3f);
        }
    }

    void Update()
    {
        //�ŏ��̃J�E���g�_�E�����I��莟�摀����\�ɂ���
        if (timeSC.startflag)
        {
            PlayerMoveSelect();
            PlayerMove();

            if (transform.position.x == 8 || transform.position.x == -8)
            {
                BCT.ReloadMakura();
            }
        }

        //���������t���O�����Ă΃��[�V�������s��
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

    //������A�j���[�V����
    public void Shot()
    {
        if (timeSC.startflag && !timeSC.TimeUP && !ES.Winflag)
        {
            Panim.SetTrigger("Throw");
        }
    }

    //�L���b�`�A�j���[�V����
    public void Catch()
    {
        if (timeSC.startflag && !timeSC.TimeUP && !ES.Winflag)
        {
            Panim.SetTrigger("Catch");
        }
    }

    //�����A�j���[�V����
    IEnumerator WinAnim()
    {
        yield return new WaitForSeconds(3);
        Panim.SetTrigger("Win");
    }

    //�L���b�`�̃A�j���[�V�������s��ꂽ�ۂɌĂ΂��֐�
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

    //������A�j���[�V�������s��ꂽ�ۂɌĂ΂��֐�
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
        //�������� or �������Ԃ������Ȃ�Ƒ��삪�ł��Ȃ��Ȃ�
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

    //�A�j���[�V�����ɂ���Ĉړ��̕�����ς���
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

    //�A�j���[�V��������������󂯎��
    public int MoveInt(int move)
    {
        return moveint = move;
    }

    //���ɓ����������̃��[�V����
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == Ewepon)
        {
            if (!catchflag)
            {
                PlayerHP++;
                Zimage[PlayerHP - 1].SetAlpha(1);
                //�����������Ƃ��ꂽ�Ƃ��̃A�j���[�V��������
                if (PlayerHP == 3)
                {
                    Loseflag = true;  //�����t���O�����Ă�
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

    //���G���ԂɂȂ�Ȃ��悤��flag��؂��Ă���
    IEnumerator CatchflagOff()
    {
        yield return new WaitForSeconds(1f);
        catchflag = false;
    }

}

