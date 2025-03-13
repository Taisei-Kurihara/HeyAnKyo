using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Respown : MonoBehaviour
{
    public Text gameOverText;  // ゲームオーバー時のテキスト (Inspector で設定)
    public Text livesText;     // 残機表示テキスト (Inspector で設定)
    public Text respawnText;   // スペースキーでリスポーン可能であることを示すテキスト (Inspector で設定)

    private int lives = 3; // 残機数
    private Vector3 lastDeathPosition; // 死亡時の位置を記録

    private void Start()
    {
        UpdateLivesText();
        respawnText.text = ""; // 初期状態では非表示
    }

    private void OnTriggerEnter(Collider other)
    {
        // 敵 (タグ: enemy) に当たった場合のみ処理
        if (other.gameObject.CompareTag("enemy"))
        {
            lives--; // 残機を減らす
            lastDeathPosition = transform.position; // 死亡位置を記録
            UpdateLivesText(); // 残機の UI を更新

            if (lives <= 0)
            {
                gameOverText.text = "Game Over"; // ゲームオーバー表示
                respawnText.text = ""; // ゲームオーバー時はリスポーンメッセージを消す
                Destroy(gameObject); // プレイヤーオブジェクトを削除
            }
            else
            {
                respawnText.text = "Press SPACE to Respawn"; // リスポーン可能であることを通知
            }
        }
    }

    private void Update()
    {
        // スペースキーが押され、残機が残っている場合リスポーン
        if (Input.GetKeyDown(KeyCode.Space) && lives > 0)
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        transform.position = lastDeathPosition; // 最後に死んだ位置でリスポーン
        respawnText.text = ""; // リスポーン後はメッセージ非表示
    }

    private void UpdateLivesText()
    {
        livesText.text = "Lives: " + lives; // 残機数を UI に反映
    }
}
