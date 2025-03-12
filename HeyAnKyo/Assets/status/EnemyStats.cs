using System.Collections;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    private GameObject currentEnemy = null; // 現在の親になっている敵（捕まえた敵）

    void OnTriggerEnter(Collider other)
    {
        // "enemy" タグのオブジェクトが "hole" に触れた場合
        if (other.CompareTag("enemy") && gameObject.CompareTag("hole"))
        {
            // もし既に敵が捕まっていた場合
            if (currentEnemy != null)
            {
                Destroy(gameObject); // hole オブジェクトを削除
                EnableMovement(currentEnemy); // 1体目の敵を即座に動かす
                return; // 2体目は捕まえないので処理終了
            }

            // enemy を hole の子オブジェクトに設定（捕まえる）
            currentEnemy = other.gameObject;
            currentEnemy.transform.parent = this.transform;

            // 10秒間動けなくする（物理演算を無効化）
            Rigidbody enemyRb = currentEnemy.GetComponent<Rigidbody>();
            if (enemyRb != null)
            {
                enemyRb.isKinematic = true; // 敵を停止させる
            }

            // 10秒後に動けるようにする処理をコルーチンで実行
            StartCoroutine(EnableMovementAfterDelay(currentEnemy, 10f));
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
            // 敵の Rigidbody を取得して動けるようにする
            Rigidbody enemyRb = enemy.GetComponent<Rigidbody>();
            if (enemyRb != null)
            {
                enemyRb.isKinematic = false; // 物理演算を再開（動けるようにする）
            }
            enemy.transform.parent = null; // 親子関係を解除
            currentEnemy = null; // クリア

            // hole オブジェクト（このスクリプトがアタッチされているオブジェクト）を削除
            Destroy(gameObject);
        }
    }
}
