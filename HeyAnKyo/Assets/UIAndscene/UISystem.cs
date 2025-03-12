using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UISystem : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI HP;

    [SerializeField]
    TextMeshProUGUI TimeText;
    float time = 0;

    bool GameOver = false;

    [SerializeField]
    TextMeshProUGUI Persons;

    [SerializeField]
    GameObject GameOverObject;

    public float SetHP { set { HP.text = value.ToString("F0") + "HP"; } }
    public int SetPersons { set { Persons.text = value + "pers"; } }

    [System.Obsolete]
    private void Awake()
    {
        GameOverObject.active = false;
        StartCoroutine(TimeWait());
    }

    IEnumerator TimeWait()
    {
        yield return null;
        if (GameOver)
        {
            GameOverObject.active = true;
            yield break; 
        }
        time += Time.deltaTime;
        TimeText.text = time.ToString("F2") + "sec";
    }

}
