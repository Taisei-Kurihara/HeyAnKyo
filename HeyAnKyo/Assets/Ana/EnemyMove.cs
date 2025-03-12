using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.HID;
using UnityEngine.UIElements;

public class EnemyMove : MonoBehaviour
{
    [SerializeField]
    Animator animator;

    // 敵の移動速度
    private float movespeed;

    // 物理演算用のRigidbody
    private Rigidbody theRB;

    // プレイヤーを追跡中かどうか
    private BitArray chasing = new BitArray(2,false);

    // 追跡開始・停止・距離を保つためのしきい値
    private float distanceToChase = 10f;  // プレイヤーを追跡し始める距離
    private float distanceToLose = 15f;   // 追跡をやめる距離
    private float distanceToStop = 2f;    // プレイヤーとの距離がこれ以下なら停止

    // 目的地の位置情報
    private Vector3 targetPoint, startPoint;

    // NavMeshAgent（経路探索AI）
    private NavMeshAgent agent;

    // 巡回するポイント（ゴール）の配列
    [SerializeField]
    private List<GameObject> goals;

    // 現在向かっているゴールのインデックス
    private int destNum = 0;

    // 追跡を続ける時間
    private float keepChasingTime = 2f;
    private float chaseCounter;

    CapsuleCollider capsuleCollider;

    private void Start()
    {
        // 初期位置を記録
        startPoint = transform.position;

        // NavMeshAgentコンポーネントを取得
        agent = GetComponent<NavMeshAgent>();

        // 最初の目的地を設定
        Positions positions = Positions.Instance();
        goals = positions.GetSpawnPoints;
        agent.destination = goals[destNum].transform.position;


        capsuleCollider = GetComponent<CapsuleCollider>();

        StartCoroutine(PlayerCheck());
    }

    IEnumerator PlayerCheck()
    {
        yield return new WaitForSeconds(1 / 30);

        animator.SetFloat("Speed", 1);

        if (chasing[1]) { yield break; }

        if (!chasing[0])
        {
            Positions positions = Positions.Instance();
            // 一定距離以内にプレイヤーが入ったら追跡を開始
            if (Vector3.Distance(transform.position, positions.Player.transform.position) < distanceToChase)
            {
                chasing[0] = true;

                agent.destination = positions.Player.transform.position;
                StartCoroutine( PlayerStalker());
                yield break;
            }

            // 追跡終了後のカウントダウン
            if (chaseCounter > 0)
            {
                chaseCounter -= Time.deltaTime;
                if (chaseCounter <= 0)
                {
                    // 次のゴールへ移動
                    nextGoal();
                }
            }

            // 目的地に到達したら次のゴールへ移動
            if (agent.remainingDistance < 0.5f)
            {
                nextGoal();
            }

            StartCoroutine(PlayerCheck());
        }
    }

    IEnumerator PlayerStalker()
    {
        yield return new WaitForSeconds(1/30);
        if (chasing[1]) { yield break; }

        if (chasing[0])
        {
            // プレイヤーが一定距離より遠い場合、プレイヤーを追いかける
            if (Vector3.Distance(transform.position, targetPoint) > distanceToStop)
            {
                agent.destination = targetPoint;
            }
            else
            {
                // プレイヤーとの距離が近すぎる場合はその場に留まる
                agent.destination = transform.position;
            }

            // プレイヤーが一定距離を超えて離れたら追跡をやめる
            if (Vector3.Distance(transform.position, targetPoint) > distanceToLose)
            {
                chasing[0] = false;
                chaseCounter = keepChasingTime; // 追跡を再開するまでのカウントダウンを開始
                StartCoroutine(PlayerCheck());
                yield break;
            }

            StartCoroutine(PlayerStalker());
        }
    }

    public void Stop(Transform ana)
    {
        animator.SetFloat("Speed",0);
        chasing[1] = true;

        transform.position = ana.transform.position;
        transform.parent = ana.transform;
        animator.SetBool("IsFallen", true);
        animator.SetTrigger("Falling");

        capsuleCollider.enabled = false;

        agent.destination = transform.position;
    }

    public void reMove()
    {
        this.transform.parent = null; // 親子関係を解除
        this.transform.localScale = Vector3.one;
        animator.SetBool("IsFallen", false);

        capsuleCollider.enabled = true;

        chasing[1] = false;
        StartCoroutine(PlayerCheck());
    }

    // 次の目的地を設定する処理
    void nextGoal()
    {
        // ランダムに目的地を選択
        destNum = Random.Range(0, goals.Count);
        agent.destination = goals[destNum].transform.position;

        capsuleCollider.enabled = true;
    }
}
