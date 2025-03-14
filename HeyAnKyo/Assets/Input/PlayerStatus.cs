using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : Status
{
    const CharType type = CharType.Player;

    float hp = 5;

    private PlayerMove playerMove;


    private void Start()
    {
        playerMove = GetComponent<PlayerMove>();
    }

    public void HPchange(float damage)
    {
        if((float)(DateTime.Now - startTime).TotalSeconds < 3 && respown.GetUI.isDead) { return; }
        hp = (hp - damage < 0) ? 0 : hp - damage;
        respown.GetUI.SetHP = hp;

        if (hp <= 0)
        {
            respown.GetUI.GameOverFlag();
        }
        else if(damage > 0)
        {
            Kill();
        }
    }

    public void Kill()
    {
        playerMove.Dead();
        
        if(hp > 0)
        {
            respown.GetUI.isDead = true;
            StartCoroutine(RevivalWait());
        }
    }

    DateTime startTime = DateTime.Now;

    IEnumerator RevivalWait()
    {
        yield return new WaitUntil(() => !respown.GetUI.isDead);
        playerMove.Revival();

        // ŠJŽnŽžŠÔ‚ðŽæ“¾
        startTime = DateTime.Now;

    }
}
