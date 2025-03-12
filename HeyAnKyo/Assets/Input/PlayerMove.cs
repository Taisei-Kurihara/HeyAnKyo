using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Collections;
using UnityEngine.Events;
using Unity.VisualScripting;
using UnityEngine.InputSystem.HID;
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
        AnaHoribool = true;
        AnaCheck();
    }

    public void OutAnaHori(InputAction.CallbackContext context)
    {
        if(nowAna != null) {
            AnaAke anaAke = nowAna.GetComponent<AnaAke>();
            anaAke.AnaUme(AnaHoriTime);
        }

        AnaHoribool = false;
    }

    IEnumerator AnaHoriEndWait()
    {        
        // �J�n���Ԃ��擾
        DateTime startTime = DateTime.Now;

        yield return new WaitUntil(() => !AnaHoribool || (float)(DateTime.Now - startTime).TotalSeconds > AnaHoriTime);

        Debug.Log((float)(startTime - DateTime.Now).TotalSeconds);

        AnaHoriEnd();
    }

    void AnaHoriEnd()
    {
        Input.EnableInput(PlayerInputNames.Move);
        nowAna = null;

    }

    public void AnaCheck()
    {
        Input.DisableInput(PlayerInputNames.Move);
        StartCoroutine(AnaHoriEndWait());

        // ���C�L���X�g�̌��ʂ��i�[����ϐ�
        RaycastHit hit;

        // ���C�L���X�g�̎��s
        if (Physics.Raycast(transform.position + (transform.forward * -0.1f), transform.forward, out hit, AnaSize/2))
        {
            Debug.Log(hit.collider.gameObject.name);
            AnaAke anaAke = hit.collider.gameObject.GetComponent<AnaAke>();
            if (anaAke != null)
            {
                anaAke.AnaUme(AnaHoriTime);
            }
        }
        else
        {
            // �z�u�ʒu�̌v�Z
            Vector3 spawnPosition = transform.position + transform.forward * (AnaSize / 2);

            // �I�u�W�F�N�g�𐶐�
            nowAna = Instantiate(Ana, spawnPosition, transform.rotation);
            AnaAke anaAke = nowAna.GetComponent<AnaAke>();
            anaAke.AnaHoriStart(AnaSize, AnaHoriTime);


        }
    }
    #endregion
}
