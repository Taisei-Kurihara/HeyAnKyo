using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpownPoint : MonoBehaviour
{
    private void Awake()
    {
        Positions positions = Positions.Instance();
        positions.SetSpawnPoints = this.gameObject;
    }
}
