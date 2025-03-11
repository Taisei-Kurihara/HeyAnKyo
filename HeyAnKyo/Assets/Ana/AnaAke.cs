using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnaAke : MonoBehaviour
{


    bool AnaHoribool = true;

    public void AnaHoriEnd() {  AnaHoribool = false; }

    float AnaSize = 5;

    public void AnaHoriStart(float AnaSize, float AnaHoriTime)
    {
        this.AnaSize = AnaSize;
        StartCoroutine(FadeWait(AnaHoriTime, AnaHoriTime));
    }

    public void AnaUme(float AnaHoriTime)
    {
        StartCoroutine(FadeWait(AnaHoriTime*-1, AnaHoriTime));
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
        if (AnaHoribool)
        {
            yield return new WaitForSeconds(1 / 30);

            // 最大時間を超えないように時間を加算
            time = Mathf.Min(time + Time.deltaTime, AbsmaxTime);

            // フェード比率を計算（フェードイン: 0→1, フェードアウト: 1→0）
            float fadePerc = Mathf.Abs((time / AbsmaxTime) - ((maxTime <= 0) ? 1 : 0));
            AnaHori(fadePerc);

            // フェードが完了するまで再帰的に実行
            if (time < AbsmaxTime) { StartCoroutine(FadeWait(maxTime, AbsmaxTime, time)); }
        }
        else
        {
            AnaHoribool = true;
            FadeWait(maxTime * -1, AbsmaxTime, (((time / AbsmaxTime) - 1) * -1) * AbsmaxTime);
        }
    }

    void AnaHori(float time)
    {
        //Debug.Log("Ana:"+ time);
        this.transform.localScale = new Vector3(AnaSize * time, 1, AnaSize * time);
    }
}
