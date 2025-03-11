using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Collections;
using UnityEngine.Events;
using Unity.VisualScripting;
public class PlayerMove : MonoBehaviour
{
    Input Input;
    Rigidbody rb;
    


    // Start is called before the first frame update
    void Start()
    {
        Input = GetComponent<Input>();
        Input.MethodSetting(PlayerInputNames.Move,ActionSettype.plus, OnMove, OutMove);
        Input.MethodSetting(PlayerInputNames.An,ActionSettype.plus, OnAnaHori, OutAnaHori);

        rb = GetComponent<Rigidbody>();
    }

    protected Vector2 DeadZone(Vector2 Input, float deadZone)
    {
        return (Input.magnitude <= deadZone) ? Input = Vector2.zero : Input;
    }




    #region �ړ�����

    // �ړ����͊֘A
    private Vector2 MoveDis = Vector2.zero;
    private float MoveDeadZone = 0.2f;
    [SerializeField]
    private float speed = 4f;

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveDis = DeadZone(Input.inputActions[(int)PlayerInputNames.Move].ReadValue<Vector2>(), MoveDeadZone);

        rb.velocity = new Vector3(MoveDis.x, 0, MoveDis.y) * speed;

        transform.rotation = Quaternion.LookRotation(rb.velocity);
        //Debug.Log($"Move Input! & x:{MoveDis.x} / y:{MoveDis.y}");
        //Debug.Log(rb.velocity.magnitude);
        //Debug.Log("Move pressed!");
    }

    public void OutMove(InputAction.CallbackContext context)
    {
        MoveDis = Vector2.zero;
        rb.velocity = Vector3.zero;
        //Debug.Log("Move released!");
    }
    #endregion

    #region ���@��

    [SerializeField]
    GameObject Ana;

    GameObject nowAna;

    private float AnaHoriTime = 1f;

    float AnaSize = 5;

    bool AnaHoribool = false;


    public void OnAnaHori(InputAction.CallbackContext context)
    {
        if(!AnaHoribool)
        {
            AnaHoribool = true;
            AnaCheck();
        }
    }

    public void OutAnaHori(InputAction.CallbackContext context)
    {
        AnaHoribool = false;
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
            yield return new WaitForSeconds(1/30);

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
            Destroy(nowAna);
        }
    }

    public void AnaCheck()
    {// ���C�L���X�g�̌��ʂ��i�[����ϐ�
        RaycastHit hit;

        // ���C�L���X�g�̎��s
        if (Physics.Raycast(transform.position, transform.forward, out hit, AnaSize))
        {
            StartCoroutine(FadeWait(-AnaHoriTime, AnaHoriTime));
        }
        else
        {
            // �z�u�ʒu�̌v�Z
            Vector3 spawnPosition = transform.position + transform.forward * (AnaSize / 2);

            // �I�u�W�F�N�g�𐶐�
            nowAna = Instantiate(Ana, spawnPosition, transform.rotation);

            StartCoroutine(FadeWait(AnaHoriTime, AnaHoriTime));
        }
    }

    public void AnaHori(float time)
    {
        //Debug.Log("Ana:"+ time);
        nowAna.transform.localScale = new Vector3(AnaSize * time,1,AnaSize * time);
        if(time == 1) { nowAna = null; }
    }
    #endregion
}
