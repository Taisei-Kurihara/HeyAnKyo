using System.Collections;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    private GameObject currentEnemy = null; // ���݂̐e�ɂȂ��Ă���G�i�߂܂����G�j

    float UmeTime = 10;


    void OnTriggerEnter(Collider other)
    {
        AnaAke anaAke = this.GetComponent<AnaAke>();
        
        if (anaAke.perfect)
        {


            EnemyMove enemyMove = other.gameObject.GetComponent<EnemyMove>();
            // "enemy" �^�O�̃I�u�W�F�N�g�� "hole" �ɐG�ꂽ�ꍇ
            if (enemyMove != null)
            {

                // �������ɓG���߂܂��Ă����ꍇ
                if (currentEnemy != null)
                {
                    EnableMovement(currentEnemy); // 1�̖ڂ̓G�𑦍��ɓ�����
                    return; // 2�̖ڂ͕߂܂��Ȃ��̂ŏ����I��
                }

                currentEnemy = other.gameObject;
                enemyMove.Stop(this.transform);

                // 10�b��ɓ�����悤�ɂ��鏈�����R���[�`���Ŏ��s
                StartCoroutine(EnableMovementAfterDelay(currentEnemy, UmeTime));
            }
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

            EnemyMove enemyMove = enemy.gameObject.GetComponent<EnemyMove>();
            enemyMove.reMove();
            // hole �I�u�W�F�N�g�i���̃X�N���v�g���A�^�b�`����Ă���I�u�W�F�N�g�j���폜
            Destroy(gameObject);
        }
    }
}
