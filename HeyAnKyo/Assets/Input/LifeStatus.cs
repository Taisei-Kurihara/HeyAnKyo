using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeStatus : Status
{
    const CharType type = CharType.heart;

    float attack = -1;

    private void OnTriggerEnter(Collider other)
    {
        Status status = other.gameObject.GetComponent<Status>();
        if (status != null)
        {
            if (status is PlayerStatus)
            {
                PlayerStatus playerStatus = (PlayerStatus)status;
                playerStatus.HPchange(attack);
                Destroy(this.gameObject);
            }
        }

    }

}
