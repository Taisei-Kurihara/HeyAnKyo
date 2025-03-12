using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyRespown : MonoBehaviour
{
    public GameObject enemy; // ��������G�I�u�W�F�N�g
    public GameObject player; // �v���C���[
    public int minEnemies = 10; // �ŏ��G��
    public int maxEnemies = 20; // �ő�G��
    private int currentEnemies = 0; // ���݂̓G��
    private float spawnInterval = 5f; // �X�|�[���Ԋu�i�b�j
    private float lastSpawnTime = 0f; // �Ō�ɃX�|�[����������
    private GameObject lastSpawnPoint = null; // �O��X�|�[�������n�_

    // �Q�[���J�n���ɏ����̓G���ŏ�����������
    void Start()
    {
        for (int i = 0; i < minEnemies; i++)
        {
            SpawnEnemy(); // �����̓G�𐶐�
        }
    }

    // ���t���[���Ă΂��
    void Update()
    {
        // ���Ԍo�߂œG�̐��𑝉��i�ő吔�𒴂��Ȃ��悤�Ɂj
        if (Time.time - lastSpawnTime > spawnInterval && currentEnemies < maxEnemies)
        {
            SpawnEnemy(); // �V�����G���X�|�[��
            lastSpawnTime = Time.time; // �Ō�ɃX�|�[���������Ԃ��X�V
        }
    }

    // �G�𐶐����鏈��
    void SpawnEnemy()
    {
        // "Respawn" �^�O�����S�Ă̋�̃I�u�W�F�N�g�i���X�|�[���n�_�j���擾
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("Respawn");

        if (spawnPoints.Length == 0) return; // �X�|�[���n�_�����݂��Ȃ���Ή������Ȃ�

        // �v���C���[�ɍł��߂� 3 �̃X�|�[���n�_�����O
        List<GameObject> validSpawnPoints = spawnPoints
            .OrderBy(spawn => Vector3.Distance(spawn.transform.position, player.transform.position)) // �v���C���[�Ƃ̋����ŕ��בւ�
            .Skip(3) // �߂� 3 �̃X�|�[���n�_�����O
            .Where(spawn => spawn != lastSpawnPoint) // �O��X�|�[�������n�_�����O
            .ToList(); // ���X�g�Ƃ��Ď擾

        // �L���ȃX�|�[���n�_��1���Ȃ���Ώ����𒆎~
        if (validSpawnPoints.Count == 0) return;

        // �����_���ɑI�΂ꂽ�X�|�[���n�_��I��
        GameObject selectedSpawnPoint = validSpawnPoints[Random.Range(0, validSpawnPoints.Count)];
        Vector3 spawnPosition = selectedSpawnPoint.transform.position; // �X�|�[���ʒu��ݒ�

        // �G�𐶐�
        Instantiate(enemy, spawnPosition, Quaternion.identity);
        currentEnemies++; // �G�̐��𑝉�

        // �O��X�|�[�������n�_���X�V
        lastSpawnPoint = selectedSpawnPoint;
    }
}
