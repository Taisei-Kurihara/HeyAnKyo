using System.Collections;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    private GameObject currentEnemy = null; // 現在の親になっている敵（捕まえた敵）

    float UmeTime = 10;


    void OnTriggerEnter(Collider other)
    {
        AnaAke anaAke = this.GetComponent<AnaAke>();
        
        if (anaAke.perfect)
        {


            EnemyMove enemyMove = other.gameObject.GetComponent<EnemyMove>();
            // "enemy" タグのオブジェクトが "hole" に触れた場合
            if (enemyMove != null)
            {

                // もし既に敵が捕まっていた場合
                if (currentEnemy != null)
                {
                    EnableMovement(currentEnemy); // 1体目の敵を即座に動かす
                    return; // 2体目は捕まえないので処理終了
                }

                currentEnemy = other.gameObject;
                enemyMove.Stop(this.transform);

                // 10秒後に動けるようにする処理をコルーチンで実行
                StartCoroutine(EnableMovementAfterDelay(currentEnemy, UmeTime));
            }
        }
    }

    // 10秒後に敵の動きを再開する
    IEnumerator EnableMovementAfterDelay(GameObject enemy, float delay)
    {
        yield return new WaitForSeconds(delay); // 指定時間待機
        EnableMovement(enemy); // 敵を動けるようにする
    }

    // 敵を動けるようにする処理（敵が動き始めるタイミングで hole を削除）
    void EnableMovement(GameObject enemy)
    {
        if (enemy != null)
        {

            EnemyMove enemyMove = enemy.gameObject.GetComponent<EnemyMove>();
            enemyMove.reMove();
            // hole オブジェクト（このスクリプトがアタッチされているオブジェクト）を削除
            Destroy(gameObject);
        }
    }
}
