using System.Collections;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    private GameObject currentEnemy = null; // ���݂̐e�ɂȂ��Ă���G�i�߂܂����G�j

    float UmeTime = 10;


    void OnTriggerEnter(Collider other)
    {
        EnemyMove enemyMove = other.GetComponent<EnemyMove>();
        // "enemy" �^�O�̃I�u�W�F�N�g�� "hole" �ɐG�ꂽ�ꍇ
        if (enemyMove != null)
        {
            // �������ɓG���߂܂��Ă����ꍇ
            if (currentEnemy != null)
            {
                Destroy(gameObject); // hole �I�u�W�F�N�g���폜
                EnableMovement(currentEnemy); // 1�̖ڂ̓G�𑦍��ɓ�����
                return; // 2�̖ڂ͕߂܂��Ȃ��̂ŏ����I��
            }

            // enemy �� hole �̎q�I�u�W�F�N�g�ɐݒ�i�߂܂���j
            currentEnemy = other.gameObject;
            currentEnemy.transform.position = this.transform.position;
            currentEnemy.transform.parent = this.transform;

            // 10�b��ɓ�����悤�ɂ��鏈�����R���[�`���Ŏ��s
            StartCoroutine(EnableMovementAfterDelay(currentEnemy, UmeTime));
        }
    }

    // 10�b��ɓG�̓������ĊJ����
    IEnumerator EnableMovementAfterDelay(GameObject enemy, float delay)
    {
        yield return new WaitForSeconds(delay); // �w�莞�ԑҋ@
        EnableMovement(enemy); // �G�𓮂���悤�ɂ���
    }

    // �G�𓮂���悤�ɂ��鏈���i�G�������n�߂�^�C�~���O�� hole ���폜�j
    void EnableMovement(GameObject enemy)
    {
        if (enemy != null)
        {
            enemy.transform.parent = null; // �e�q�֌W������
            currentEnemy = null; // �N���A

            // hole �I�u�W�F�N�g�i���̃X�N���v�g���A�^�b�`����Ă���I�u�W�F�N�g�j���폜
            Destroy(gameObject);
        }
    }
}
