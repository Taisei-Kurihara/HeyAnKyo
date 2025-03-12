using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    float hp = 3;

    private void OnTriggerEnter(Collider other)
    {

        EnemyMove enemyMove = other.gameObject.GetComponent<EnemyMove>();
        if (enemyMove != null)
        {

        }
    }
}
