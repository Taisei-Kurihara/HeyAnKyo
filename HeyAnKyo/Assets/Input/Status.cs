using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharType
{
    Player,
    Enemy,
    heart,
    timer,
    coffee
}

public class Status : MonoBehaviour
{
    const CharType type = CharType.Player;
    public CharType CharType { get { return type; }}

    protected EnemyRespown respown;
    public EnemyRespown SetEnemyRespown { set { respown = value; } }
}
