using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyRespown : MonoBehaviour
{
    public GameObject enemy; // 生成する敵オブジェクト
    Transform player; // プレイヤー
    public int minEnemies = 5; // 最小敵数
    public int maxEnemies = 15; // 最大敵数
    private int currentEnemies = 0; // 現在の敵数
    private float spawnInterval = 5f; // スポーン間隔（秒）
    private float lastSpawnTime = 0f; // 最後にスポーンした時間
    private GameObject lastSpawnPoint = null; // 前回スポーンした地点

    List<GameObject> spawnPoints;

    void Start()
    {
        Positions positions = Positions.Instance();
        player = positions.Player;
        spawnPoints = positions.GetSpawnPoints;

        // 初期の敵をスポーン
        for (int i = 0; i < minEnemies; i++)
        {
            SpawnEnemy();
        }
    }

    void Update()
    {
        // 時間経過で敵の数を増加（最大数まで）
        if (Time.time - lastSpawnTime > spawnInterval && currentEnemies < maxEnemies)
        {
            SpawnEnemy();
            lastSpawnTime = Time.time;
        }
    }

    void SpawnEnemy()
    {
        if (spawnPoints.Count == 0) return; // スポーン地点がなければ何もしない

        // プレイヤーに最も近い 3 つのスポーン地点を除外
        List<GameObject> validSpawnPoints = spawnPoints
            .OrderBy(spawn => Vector3.Distance(spawn.transform.position, player.position))
            .Skip(3) // 近い 3 つをスキップ
            .Where(spawn => spawn != lastSpawnPoint) // 前回のスポーン地点を除外
            .ToList();

        if (validSpawnPoints.Count == 0) return; // 有効なスポーン地点がない場合は何もしない

        // ランダムなスポーン地点を選択
        GameObject selectedSpawnPoint = validSpawnPoints[Random.Range(0, validSpawnPoints.Count)];
        Vector3 spawnPosition = selectedSpawnPoint.transform.position;

        // 敵を生成
        Instantiate(enemy, spawnPosition, Quaternion.identity);
        currentEnemies++;

        // 前回のスポーン地点を更新
        lastSpawnPoint = selectedSpawnPoint;
    }
}
