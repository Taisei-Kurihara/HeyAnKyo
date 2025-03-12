using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyRespown : MonoBehaviour
{
    public GameObject enemy; // 生成する敵オブジェクト
    public GameObject player; // プレイヤー
    public int minEnemies = 10; // 最小敵数
    public int maxEnemies = 20; // 最大敵数
    private int currentEnemies = 0; // 現在の敵数
    private float spawnInterval = 5f; // スポーン間隔（秒）
    private float lastSpawnTime = 0f; // 最後にスポーンした時間
    private GameObject lastSpawnPoint = null; // 前回スポーンした地点

    // ゲーム開始時に初期の敵を最小数だけ生成
    void Start()
    {
        for (int i = 0; i < minEnemies; i++)
        {
            SpawnEnemy(); // 初期の敵を生成
        }
    }

    // 毎フレーム呼ばれる
    void Update()
    {
        // 時間経過で敵の数を増加（最大数を超えないように）
        if (Time.time - lastSpawnTime > spawnInterval && currentEnemies < maxEnemies)
        {
            SpawnEnemy(); // 新しい敵をスポーン
            lastSpawnTime = Time.time; // 最後にスポーンした時間を更新
        }
    }

    // 敵を生成する処理
    void SpawnEnemy()
    {
        // "Respawn" タグを持つ全ての空のオブジェクト（リスポーン地点）を取得
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("Respawn");

        if (spawnPoints.Length == 0) return; // スポーン地点が存在しなければ何もしない

        // プレイヤーに最も近い 3 つのスポーン地点を除外
        List<GameObject> validSpawnPoints = spawnPoints
            .OrderBy(spawn => Vector3.Distance(spawn.transform.position, player.transform.position)) // プレイヤーとの距離で並べ替え
            .Skip(3) // 近い 3 つのスポーン地点を除外
            .Where(spawn => spawn != lastSpawnPoint) // 前回スポーンした地点を除外
            .ToList(); // リストとして取得

        // 有効なスポーン地点が1つもなければ処理を中止
        if (validSpawnPoints.Count == 0) return;

        // ランダムに選ばれたスポーン地点を選択
        GameObject selectedSpawnPoint = validSpawnPoints[Random.Range(0, validSpawnPoints.Count)];
        Vector3 spawnPosition = selectedSpawnPoint.transform.position; // スポーン位置を設定

        // 敵を生成
        Instantiate(enemy, spawnPosition, Quaternion.identity);
        currentEnemies++; // 敵の数を増加

        // 前回スポーンした地点を更新
        lastSpawnPoint = selectedSpawnPoint;
    }
}
