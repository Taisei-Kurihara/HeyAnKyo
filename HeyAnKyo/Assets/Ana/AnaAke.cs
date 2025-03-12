using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnaAke : MonoBehaviour
{


    BitArray AnaHoribool = new BitArray(4,true);

    public bool Ume { get { return !AnaHoribool[1]; } }


    float AnaSize = 5;

    public void AnaHoriStart(float AnaSize, float AnaHoriTime)
    {
        this.AnaSize = AnaSize;
        StartCoroutine(FadeWait(AnaHoriTime, AnaHoriTime));
    }

    public void AnaUme(float AnaHoriTime)
    {
        if (AnaHoribool[3])
        {
            AnaHoribool[1] = AnaHoribool[2];
            AnaHoribool[0] = !AnaHoribool[2];
        }

        AnaHoribool[3] = false;
    }

    /// <summary>
    /// フェード処理（時間に応じたフェード率の計算）
    /// </summary>
    /// <param name="maxTime">フェード時間</param>
    /// <param name="AbsmaxTime">フェードの絶対時間</param>
    /// <param name="time">現在の経過時間</param>
    IEnumerator FadeWait(float maxTime, float AbsmaxTime, float time = 0)
    {
        // フェードの実行判定
        if (AnaHoribool[0])
        {
            yield return new WaitForSeconds(1 / 30);

            // 最大時間を超えないように時間を加算
            time = Mathf.Min(time + Time.deltaTime, AbsmaxTime);

            // フェード比率を計算（フェードイン: 0→1, フェードアウト: 1→0）
            float fadeperc = Mathf.Abs((time / AbsmaxTime) - ((maxTime <= 0) ? 1 : 0));
            AnaHori(fadeperc);

            // フェードが完了するまで再帰的に実行
            if (time < AbsmaxTime) { StartCoroutine(FadeWait(maxTime, AbsmaxTime, time)); }
            else
            {
                if (AnaHoribool[1])
                {
                    AnaHoribool[2] = false;
                    yield return new WaitUntil(() => !AnaHoribool[1]);
                    AnaHoribool[0] = true;
                    StartCoroutine(FadeWait(maxTime * -1, AbsmaxTime));
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
        else
        {
            if (AnaHoribool[1])
            {
                AnaHoribool[0] = true;
                AnaHoribool[1] = false;
                StartCoroutine(FadeWait(maxTime * -1, AbsmaxTime, (time - AbsmaxTime) * -1));
            }
        }
    }

    void AnaHori(float fadeperc)
    {
        //Debug.Log("Ana:"+ time);
        this.transform.localScale = new Vector3(AnaSize * fadeperc, 1, AnaSize * fadeperc);
    }
}
