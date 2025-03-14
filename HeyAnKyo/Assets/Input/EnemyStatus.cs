using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using static UnityEditor.PlayerSettings;
using static UnityEditor.Progress;

[System.Serializable]
class DropItem
{
    [SerializeField]
    GameObject Item;

    public GameObject GetItem { get { return Item; }}

    [SerializeField, Header("n/100")]
    float percent = 8f;

    int leverage = 1000;
    public bool AdventSelection()
    {
        if((UnityEngine.Random.Range(0, (100 * leverage) + 1) / leverage) <= percent)
        {
            return true;
        }
        return false;
    }
}

public class EnemyStatus : Status
{
    const CharType type = CharType.Enemy;

    private EnemyMove enemyMove;

    float attack = 1;

    [SerializeField]
    int score = 100;

    [SerializeField]
    List<DropItem> dropItems;

    private void Start()
    {
        enemyMove = GetComponent<EnemyMove>();
        enemyMove.Firststep();
        StartCoroutine(Deadcheck());
    }

    public void Dead()
    {
        respown.currentEnemiesCount(-1);
        respown.GetUI.addscore = score;

        foreach (DropItem item in dropItems)
        {
            if(item.AdventSelection())
            {
                Instantiate(item.GetItem, this.transform.position, Quaternion.identity);
            }
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
