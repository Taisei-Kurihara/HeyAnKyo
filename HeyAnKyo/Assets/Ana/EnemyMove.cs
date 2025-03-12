using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.HID;

public class EnemyMove : MonoBehaviour
{
    [SerializeField]
    Animator animator;

    // 敵の移動速度
    private float movespeed;

    // 物理演算用のRigidbody
    private Rigidbody theRB;

    // プレイヤーを追跡中かどうか
    private bool chasing;

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
    private List<Transform> goals;

    // 現在向かっているゴールのインデックス
    private int destNum = 0;

    // 追跡を続ける時間
    private float keepChasingTime = 2f;
    private float chaseCounter;

    private void Start()
    {
        // 初期位置を記録
        startPoint = transform.position;

        // NavMeshAgentコンポーネントを取得
        agent = GetComponent<NavMeshAgent>();

        // 最初の目的地を設定
        agent.destination = goals[destNum].position;
    }

    void PlayerCheck()
    {
        if (Vector3.Distance(transform.position, targetPoint) < distanceToChase)
        {
            chasing = true;
        }
        else
        {

        }
    }

    public void Stop()
    {
        agent.destination = transform.position;
    }


    private void Update()
    {
        // プレイヤーの現在位置を取得（Y座標は変えずに地面と同じ高さにする）
        //targetPoint = PlayerController.instance.transform.position; //コメントアウト
        targetPoint.y = transform.position.y;

        // 追跡していない場合の処理
        if (!chasing)
        {
            // 一定距離以内にプレイヤーが入ったら追跡を開始
            if (Vector3.Distance(transform.position, targetPoint) < distanceToChase)
            {
                chasing = true;
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
                // 次のゴールへの移動を3.5秒後に実行する場合（コメントアウト）
                // Invoke(nameof(nextGoal), 3.5f);
            }
        }
        else // 追跡中の処理
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
                chasing = false;
                chaseCounter = keepChasingTime; // 追跡を再開するまでのカウントダウンを開始
            }
        }
    }

    // 次の目的地を設定する処理
    void nextGoal()
    {
        // ランダムに目的地を選択
        destNum = Random.Range(0, goals.Length);
        agent.destination = goals[destNum].position;
    }



    float UmeTime = 10;

    private void OnTriggerEnter(Collider other)
    {
        AnaAke anaAke = other.gameObject.GetComponent<AnaAke>();
        if (anaAke != null)
        {
            transform.position = anaAke.transform.position;
            transform.parent = anaAke.transform;
            animator.SetTrigger("Falling");
        }
    }
}
