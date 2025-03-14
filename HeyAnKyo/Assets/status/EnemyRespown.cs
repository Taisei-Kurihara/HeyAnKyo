using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Xml.Linq;
using System;

public class EnemyRespown : MonoBehaviour
{
    public GameObject enemy; // 生成する敵オブジェクト
    public GameObject player; // プレイヤー
    private Transform playerPos; // プレイヤー
    public int minEnemies = 5; // 最小敵数
    public int maxEnemies = 15; // 最大敵数
    private int currentEnemies = 0; // 現在の敵数
    private float spawnInterval = 10f; // スポーン間隔（秒）
    private float lastSpawnTime = 0f; // 最後にスポーンした時間
    private GameObject lastSpawnPoint = null; // 前回スポーンした地点

    List<GameObject> spawnPoints;

    [SerializeField]
    UISystem UI;
    public UISystem GetUI { get { return UI; }}

    [SerializeField]
    Camera Camera;

    void Start()
    {
        Positions positions = Positions.Instance();
        spawnPoints = positions.GetSpawnPoints;


        Spawn(player);
  

        
        // 初期の敵をスポーン
        for (int i = 0; i < minEnemies; i++)
        {
            Spawn();
        }

        StartCoroutine(RespownWait());
    }

    IEnumerator RespownWait()
    {
        // 開始時間を取得
        DateTime startTime = DateTime.Now;

        yield return new WaitUntil(() => (currentEnemies < maxEnemies) && (float)(DateTime.Now - startTime).TotalSeconds > spawnInterval);

        Spawn();

        StartCoroutine(RespownWait());
    }

    void Spawn(GameObject spown = null)
    {
        if (spawnPoints.Count == 0) return; // スポーン地点がなければ何もしない

        // プレイヤーに最も近い 3 つのスポーン地点を除外
        List<GameObject> validSpawnPoints = spawnPoints
            .OrderBy(spawn => Vector3.Distance(spawn.transform.position, (playerPos != null) ? playerPos.position : Vector3.zero))
            .Skip(5) // 近い 3 つをスキップ
            .Where(spawn => spawn != lastSpawnPoint) // 前回のスポーン地点を除外
            .ToList();

        if (validSpawnPoints.Count == 0) return; // 有効なスポーン地点がない場合は何もしない

        // ランダムなスポーン地点を選択
        GameObject selectedSpawnPoint = validSpawnPoints[UnityEngine.Random.Range(0, validSpawnPoints.Count)];
        Vector3 spawnPosition = selectedSpawnPoint.transform.position;

        if (spown == null)
        {
            spown = enemy;
            currentEnemiesCount(1);
        }
        // 敵を生成
        GameObject x = Instantiate(spown, spawnPosition, Quaternion.identity);

        Status status = x.GetComponent<Status>();
        if (status != null)
        {
            status.SetEnemyRespown = this.gameObject.GetComponent<EnemyRespown>();
            if (status is PlayerStatus)
            {
                Camera.player = x.transform;
                playerPos = x.transform;
            }
        }


        // 前回のスポーン地点を更新
        lastSpawnPoint = selectedSpawnPoint;
    }

    public void currentEnemiesCount(int pm)
    {
        currentEnemies += pm;
        UI.SetPersons = currentEnemies;
    }
}
