using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.HID;
using UnityEngine.UIElements;

public class EnemyMove : MonoBehaviour
{
    [SerializeField]
    Animator animator;

    // �G�̈ړ����x
    private float movespeed;

    // �������Z�p��Rigidbody
    private Rigidbody theRB;

    // �v���C���[��ǐՒ����ǂ���
    private BitArray chasing = new BitArray(2,false);

    // �ǐՊJ�n�E��~�E������ۂ��߂̂������l
    private float distanceToChase = 10f;  // �v���C���[��ǐՂ��n�߂鋗��
    private float distanceToLose = 15f;   // �ǐՂ���߂鋗��
    private float distanceToStop = 2f;    // �v���C���[�Ƃ̋���������ȉ��Ȃ��~

    // �ړI�n�̈ʒu���
    private Vector3 targetPoint, startPoint;

    // NavMeshAgent�i�o�H�T��AI�j
    private NavMeshAgent agent;

    // ���񂷂�|�C���g�i�S�[���j�̔z��
    [SerializeField]
    private List<GameObject> goals;

    // ���݌������Ă���S�[���̃C���f�b�N�X
    private int destNum = 0;

    // �ǐՂ𑱂��鎞��
    private float keepChasingTime = 2f;
    private float chaseCounter;

    CapsuleCollider capsuleCollider;

    private void Start()
    {
        // �����ʒu���L�^
        startPoint = transform.position;

        // NavMeshAgent�R���|�[�l���g���擾
        agent = GetComponent<NavMeshAgent>();

        // �ŏ��̖ړI�n��ݒ�
        Positions positions = Positions.Instance();
        goals = positions.GetSpawnPoints;
        agent.destination = goals[destNum].transform.position;


        capsuleCollider = GetComponent<CapsuleCollider>();

        StartCoroutine(PlayerCheck());
    }

    IEnumerator PlayerCheck()
    {
        yield return new WaitForSeconds(1 / 30);

        animator.SetFloat("Speed", 1);

        if (chasing[1]) { yield break; }

        if (!chasing[0])
        {
            Positions positions = Positions.Instance();
            // ��苗���ȓ��Ƀv���C���[����������ǐՂ��J�n
            if (Vector3.Distance(transform.position, positions.Player.transform.position) < distanceToChase)
            {
                chasing[0] = true;

                agent.destination = positions.Player.transform.position;
                StartCoroutine( PlayerStalker());
                yield break;
            }

            // �ǐՏI����̃J�E���g�_�E��
            if (chaseCounter > 0)
            {
                chaseCounter -= Time.deltaTime;
                if (chaseCounter <= 0)
                {
                    // ���̃S�[���ֈړ�
                    nextGoal();
                }
            }

            // �ړI�n�ɓ��B�����玟�̃S�[���ֈړ�
            if (agent.remainingDistance < 0.5f)
            {
                nextGoal();
            }

            StartCoroutine(PlayerCheck());
        }
    }

    IEnumerator PlayerStalker()
    {
        yield return new WaitForSeconds(1/30);
        if (chasing[1]) { yield break; }

        if (chasing[0])
        {
            // �v���C���[����苗����艓���ꍇ�A�v���C���[��ǂ�������
            if (Vector3.Distance(transform.position, targetPoint) > distanceToStop)
            {
                agent.destination = targetPoint;
            }
            else
            {
                // �v���C���[�Ƃ̋������߂�����ꍇ�͂��̏�ɗ��܂�
                agent.destination = transform.position;
            }

            // �v���C���[����苗���𒴂��ė��ꂽ��ǐՂ���߂�
            if (Vector3.Distance(transform.position, targetPoint) > distanceToLose)
            {
                chasing[0] = false;
                chaseCounter = keepChasingTime; // �ǐՂ��ĊJ����܂ł̃J�E���g�_�E�����J�n
                StartCoroutine(PlayerCheck());
                yield break;
            }

            StartCoroutine(PlayerStalker());
        }
    }

    public void Stop(Transform ana)
    {
        animator.SetFloat("Speed",0);
        chasing[1] = true;

        transform.position = ana.transform.position;
        transform.parent = ana.transform;
        animator.SetBool("IsFallen", true);
        animator.SetTrigger("Falling");

        capsuleCollider.enabled = false;

        agent.destination = transform.position;
    }

    public void reMove()
    {
        this.transform.parent = null; // �e�q�֌W������
        this.transform.localScale = Vector3.one;
        animator.SetBool("IsFallen", false);

        capsuleCollider.enabled = true;

        chasing[1] = false;
        StartCoroutine(PlayerCheck());
    }

    // ���̖ړI�n��ݒ肷�鏈��
    void nextGoal()
    {
        // �����_���ɖړI�n��I��
        destNum = Random.Range(0, goals.Count);
        agent.destination = goals[destNum].transform.position;

        capsuleCollider.enabled = true;
    }
}
