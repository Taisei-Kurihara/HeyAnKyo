using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharType
{
    Player,
    Enemy
}

public class Status : MonoBehaviour
{
    [SerializeField]
    CharType type;

    public CharType CharType { get { return type; } }


    float hp = 5;

    float attack = 1;

    public float damage { get { return attack; } }

    private PlayerMove playerMove;

    private EnemyMove enemyMove;

    EnemyRespown respown;

    [SerializeField]
    int score = 100;

    public EnemyRespown SetEnemyRespown { set { respown = value; } }

    private void Awake()
    {
        playerMove = GetComponent<PlayerMove>();
        enemyMove = GetComponent<EnemyMove>();

    }

    private void Start()
    {
        if (type == CharType.Enemy)
        {
            StartCoroutine(Deadcheck());
        }
    }

    void OnDestroy()
    {
        if(type == CharType.Enemy)
        {
            respown.currentEnemiesCount(-1);
            respown.GetUI.addscore = score;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if((float)(DateTime.Now - startTime).TotalSeconds > 3){
        Status status = other.gameObject.GetComponent<Status>();
            if (status != null)
            {
                if (type == CharType.Player && status.CharType == CharType.Enemy)
                {
                    HPchange(status.damage);
                }
            }
        }
    }

    public void  HPchange(float damage)
    {
        hp = (hp - damage < 0) ? 0 : hp - damage;
        respown.GetUI.SetHP = hp;

        playerMove.Dead();

        if (hp <= 0)
        {
            respown.GetUI.GameOverFlag();
        }
        else
        {
            respown.GetUI.isDead = true;
            StartCoroutine(RevivalWait());
        }
    }

    DateTime startTime = DateTime.Now;

    IEnumerator RevivalWait()
    {
        yield return new WaitUntil(()=> !respown.GetUI.isDead);
        playerMove.Revival();

        // ŠJŽnŽžŠÔ‚ðŽæ“¾
        startTime = DateTime.Now;

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
