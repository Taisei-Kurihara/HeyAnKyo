using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class route : MonoBehaviour
{
    // �G�̈ړ����x
    public float movespeed;

    // �������Z�p��Rigidbody
    public Rigidbody theRB;

    // �v���C���[��ǐՒ����ǂ���
    public bool chasing;

    // �ǐՊJ�n�E��~�E������ۂ��߂̂������l
    public float distanceToChase = 10f;  // �v���C���[��ǐՂ��n�߂鋗��
    public float distanceToLose = 15f;   // �ǐՂ���߂鋗��
    public float distanceToStop = 2f;    // �v���C���[�Ƃ̋���������ȉ��Ȃ��~

    // �ړI�n�̈ʒu���
    private Vector3 targetPoint, startPoint;

    // NavMeshAgent�i�o�H�T��AI�j
    public NavMeshAgent agent;

    // ���񂷂�|�C���g�i�S�[���j�̔z��
    public Transform[] goals;

    // ���݌������Ă���S�[���̃C���f�b�N�X
    private int destNum = 0;

    // �ǐՂ𑱂��鎞��
    public float keepChasingTime = 5f;
    private float chaseCounter;

    private void Start()
    {
        // �����ʒu���L�^
        startPoint = transform.position;

        // NavMeshAgent�R���|�[�l���g���擾
        agent = GetComponent<NavMeshAgent>();

        // �ŏ��̖ړI�n��ݒ�
        agent.destination = goals[destNum].position;
    }

    private void Update()
    {
        // �v���C���[�̌��݈ʒu���擾�iY���W�͕ς����ɒn�ʂƓ��������ɂ���j
        //targetPoint = PlayerController.instance.transform.position; //�R�����g�A�E�g
        targetPoint.y = transform.position.y;

        // �ǐՂ��Ă��Ȃ��ꍇ�̏���
        if (!chasing)
        {
            // ��苗���ȓ��Ƀv���C���[����������ǐՂ��J�n
            if (Vector3.Distance(transform.position, targetPoint) < distanceToChase)
            {
                chasing = true;
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
                // ���̃S�[���ւ̈ړ���3.5�b��Ɏ��s����ꍇ�i�R�����g�A�E�g�j
                // Invoke(nameof(nextGoal), 3.5f);
            }
        }
        else // �ǐՒ��̏���
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
                chasing = false;
                chaseCounter = keepChasingTime; // �ǐՂ��ĊJ����܂ł̃J�E���g�_�E�����J�n
            }
        }
    }

    // ���̖ړI�n��ݒ肷�鏈��
    void nextGoal()
    {
        // �����_���ɖړI�n��I��
        destNum = Random.Range(0, goals.Length);
        agent.destination = goals[destNum].position;
    }
}
