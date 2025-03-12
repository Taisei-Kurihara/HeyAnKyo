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
    /// �t�F�[�h�����i���Ԃɉ������t�F�[�h���̌v�Z�j
    /// </summary>
    /// <param name="maxTime">�t�F�[�h����</param>
    /// <param name="AbsmaxTime">�t�F�[�h�̐�Ύ���</param>
    /// <param name="time">���݂̌o�ߎ���</param>
    IEnumerator FadeWait(float maxTime, float AbsmaxTime, float time = 0)
    {
        // �t�F�[�h�̎��s����
        if (AnaHoribool[0])
        {
            yield return new WaitForSeconds(1 / 30);

            // �ő厞�Ԃ𒴂��Ȃ��悤�Ɏ��Ԃ����Z
            time = Mathf.Min(time + Time.deltaTime, AbsmaxTime);

            // �t�F�[�h�䗦���v�Z�i�t�F�[�h�C��: 0��1, �t�F�[�h�A�E�g: 1��0�j
            float fadeperc = Mathf.Abs((time / AbsmaxTime) - ((maxTime <= 0) ? 1 : 0));
            AnaHori(fadeperc);

            // �t�F�[�h����������܂ōċA�I�Ɏ��s
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
