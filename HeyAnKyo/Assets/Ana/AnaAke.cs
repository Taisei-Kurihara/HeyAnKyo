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
    /// �t�F�[�h�����i���Ԃɉ������t�F�[�h���̌v�Z�j
    /// </summary>
    /// <param name="maxTime">�t�F�[�h����</param>
    /// <param name="AbsmaxTime">�t�F�[�h�̐�Ύ���</param>
    /// <param name="time">���݂̌o�ߎ���</param>
    IEnumerator FadeWait(float maxTime, float AbsmaxTime, float time = 0)
    {
        // �t�F�[�h�̎��s����
        if (AnaHoribool)
        {
            yield return new WaitForSeconds(1 / 30);

            // �ő厞�Ԃ𒴂��Ȃ��悤�Ɏ��Ԃ����Z
            time = Mathf.Min(time + Time.deltaTime, AbsmaxTime);

            // �t�F�[�h�䗦���v�Z�i�t�F�[�h�C��: 0��1, �t�F�[�h�A�E�g: 1��0�j
            float fadePerc = Mathf.Abs((time / AbsmaxTime) - ((maxTime <= 0) ? 1 : 0));
            AnaHori(fadePerc);

            // �t�F�[�h����������܂ōċA�I�Ɏ��s
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
