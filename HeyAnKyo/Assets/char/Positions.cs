using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Positions : SingletonMonoBehaviourBase<Positions>
{
    Transform player;

    public Transform Player { get { return player; }  set { player = value; } }

    List<GameObject> spawnPoints = new List<GameObject>();
    public List<GameObject> GetSpawnPoints {  get { return spawnPoints; } }
    public GameObject SetSpawnPoints {  set { spawnPoints.Add(value); } }


}
