using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class EnemyStatus : Status
{
    const CharType type = CharType.Enemy;

    private EnemyMove enemyMove;

    float attack = 1;

    [SerializeField]
    int score = 100;

    private void Start()
    {
        enemyMove = GetComponent<EnemyMove>();
        StartCoroutine(Deadcheck());
    }

    void OnDestroy()
    {
        if (type == CharType.Enemy)
        {
            respown.currentEnemiesCount(-1);
            respown.GetUI.addscore = score;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Status status = other.gameObject.GetComponent<Status>();
        if (status != null)
        {
            if (status is PlayerStatus)
            {
                PlayerStatus playerStatus = (PlayerStatus)status;
                playerStatus.HPchange(attack);
            }
        }
        
    }

    IEnumerator Deadcheck()
    {
        bool dead = respown.GetUI.isDead;

        yield return new WaitUntil(() => dead != respown.GetUI.isDead);
        yield return new WaitForSeconds(3);
        enemyMove.Deadcheck = respown.GetUI.isDead;
        StartCoroutine(Deadcheck());
    }
}
