using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISystem : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI HP;

    [SerializeField]
    TextMeshProUGUI TimeText;
    float time = 0;

    bool dead = false;
    public bool isDead { get { return dead; } set { dead = value; } }

    bool GameOver = false;
    public void GameOverFlag() { GameOver = true; }

    [SerializeField]
    TextMeshProUGUI Persons;
    [SerializeField]
    TextMeshProUGUI scoreText;

    int score = 0;
    public int addscore { set { score += value; scoreText.text = score + "score"; } }

    [SerializeField]
    GameObject GameOverObject;

    [SerializeField]
    GameObject retryObject;
    [SerializeField]
    Button retryButton;


    public float SetHP { set { HP.text = "life" + value.ToString("F0"); } }
    public int SetPersons { set { Persons.text = value + "pers"; } }

    private void Awake()
    {
        GameOverObject.active = false;
        retryObject.active = false;
        StartCoroutine(TimecountWait());
    }

    IEnumerator TimecountWait()
    {
        yield return new WaitForSeconds(0);

        if (GameOver)
        {
            GameOverObject.active = true;
            yield break;
        }
        else if (dead)
        {
            retryObject.active = true;

            retryButton.onClick.AddListener(Retry);
            yield return new WaitUntil(() => !dead);
        }



        time += Time.deltaTime;
        TimeText.text = time.ToString("F2") + "sec";
        StartCoroutine(TimecountWait());
    }

    void Retry()
    {
        retryButton.onClick.RemoveListener(Retry);
        dead = false;
        retryObject.active = false;
    }

}
