using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Xml.Linq;
using System;

public class EnemyRespown : MonoBehaviour
{
    public GameObject enemy; // ��������G�I�u�W�F�N�g
    public GameObject player; // �v���C���[
    private Transform playerPos; // �v���C���[
    public int minEnemies = 5; // �ŏ��G��
    public int maxEnemies = 15; // �ő�G��
    private int currentEnemies = 0; // ���݂̓G��
    private float spawnInterval = 10f; // �X�|�[���Ԋu�i�b�j
    private float lastSpawnTime = 0f; // �Ō�ɃX�|�[����������
    private GameObject lastSpawnPoint = null; // �O��X�|�[�������n�_

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
  

        
        // �����̓G���X�|�[��
        for (int i = 0; i < minEnemies; i++)
        {
            Spawn();
        }

        StartCoroutine(RespownWait());
    }

    IEnumerator RespownWait()
    {
        // �J�n���Ԃ��擾
        DateTime startTime = DateTime.Now;

        yield return new WaitUntil(() => (currentEnemies < maxEnemies) && (float)(DateTime.Now - startTime).TotalSeconds > spawnInterval);

        Spawn();

        StartCoroutine(RespownWait());
    }

    void Spawn(GameObject spown = null)
    {
        if (spawnPoints.Count == 0) return; // �X�|�[���n�_���Ȃ���Ή������Ȃ�

        // �v���C���[�ɍł��߂� 3 �̃X�|�[���n�_�����O
        List<GameObject> validSpawnPoints = spawnPoints
            .OrderBy(spawn => Vector3.Distance(spawn.transform.position, (playerPos != null) ? playerPos.position : Vector3.zero))
            .Skip(5) // �߂� 3 ���X�L�b�v
            .Where(spawn => spawn != lastSpawnPoint) // �O��̃X�|�[���n�_�����O
            .ToList();

        if (validSpawnPoints.Count == 0) return; // �L���ȃX�|�[���n�_���Ȃ��ꍇ�͉������Ȃ�

        // �����_���ȃX�|�[���n�_��I��
        GameObject selectedSpawnPoint = validSpawnPoints[UnityEngine.Random.Range(0, validSpawnPoints.Count)];
        Vector3 spawnPosition = selectedSpawnPoint.transform.position;

        if (spown == null)
        {
            spown = enemy;
            currentEnemiesCount(1);
        }
        // �G�𐶐�
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


        // �O��̃X�|�[���n�_���X�V
        lastSpawnPoint = selectedSpawnPoint;
    }

    public void currentEnemiesCount(int pm)
    {
        currentEnemies += pm;
        UI.SetPersons = currentEnemies;
    }
}
