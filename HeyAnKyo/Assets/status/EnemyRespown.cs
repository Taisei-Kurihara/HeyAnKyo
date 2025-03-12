using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyRespown : MonoBehaviour
{
    public GameObject enemy; // ��������G�I�u�W�F�N�g
    Transform player; // �v���C���[
    public int minEnemies = 5; // �ŏ��G��
    public int maxEnemies = 15; // �ő�G��
    private int currentEnemies = 0; // ���݂̓G��
    private float spawnInterval = 5f; // �X�|�[���Ԋu�i�b�j
    private float lastSpawnTime = 0f; // �Ō�ɃX�|�[����������
    private GameObject lastSpawnPoint = null; // �O��X�|�[�������n�_

    List<GameObject> spawnPoints;

    void Start()
    {
        Positions positions = Positions.Instance();
        player = positions.Player;
        spawnPoints = positions.GetSpawnPoints;

        // �����̓G���X�|�[��
        for (int i = 0; i < minEnemies; i++)
        {
            SpawnEnemy();
        }
    }

    void Update()
    {
        // ���Ԍo�߂œG�̐��𑝉��i�ő吔�܂Łj
        if (Time.time - lastSpawnTime > spawnInterval && currentEnemies < maxEnemies)
        {
            SpawnEnemy();
            lastSpawnTime = Time.time;
        }
    }

    void SpawnEnemy()
    {
        if (spawnPoints.Count == 0) return; // �X�|�[���n�_���Ȃ���Ή������Ȃ�

        // �v���C���[�ɍł��߂� 3 �̃X�|�[���n�_�����O
        List<GameObject> validSpawnPoints = spawnPoints
            .OrderBy(spawn => Vector3.Distance(spawn.transform.position, player.position))
            .Skip(3) // �߂� 3 ���X�L�b�v
            .Where(spawn => spawn != lastSpawnPoint) // �O��̃X�|�[���n�_�����O
            .ToList();

        if (validSpawnPoints.Count == 0) return; // �L���ȃX�|�[���n�_���Ȃ��ꍇ�͉������Ȃ�

        // �����_���ȃX�|�[���n�_��I��
        GameObject selectedSpawnPoint = validSpawnPoints[Random.Range(0, validSpawnPoints.Count)];
        Vector3 spawnPosition = selectedSpawnPoint.transform.position;

        // �G�𐶐�
        Instantiate(enemy, spawnPosition, Quaternion.identity);
        currentEnemies++;

        // �O��̃X�|�[���n�_���X�V
        lastSpawnPoint = selectedSpawnPoint;
    }
}
