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

    [SerializeField]
    Animator animator;


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
    private float speed = 6f;

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveDis = DeadZone(Input.inputActions[(int)PlayerInputNames.Move].ReadValue<Vector2>(), MoveDeadZone);

        rb.velocity = new Vector3(MoveDis.x, 0, MoveDis.y) * speed;

        transform.rotation = Quaternion.LookRotation(rb.velocity);

        animator.SetFloat("Speed", rb.velocity.magnitude);
    }

    public void OutMove(InputAction.CallbackContext context)
    {
        SpeedZero();
    }

    private void SpeedZero()
    {
        MoveDis = Vector2.zero;
        rb.velocity = Vector3.zero;
        animator.SetFloat("Speed", rb.velocity.magnitude);
    }
    #endregion

    #region ���@��

    [SerializeField]
    GameObject Ana;

    GameObject nowAna;

    private float AnaHoriTime = 1f;

    float AnaSize = 1;

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

    IEnumerator AnaHoriEndWait(bool AnaUme)
    {        
        // �J�n���Ԃ��擾
        DateTime startTime = DateTime.Now;

        // ���͂̏I�� or ���@��ɗv���鎞�Ԃ��߂������Ƃ��m�F
        yield return new WaitUntil(() => (!AnaHoribool && !AnaUme) || (float)(DateTime.Now - startTime).TotalSeconds > AnaHoriTime);

        AnaHoriEnd();
    }

    void AnaHoriEnd()
    {
        Input.EnableInput(PlayerInputNames.Move);
        nowAna = null;

        animator.SetBool("IsDigging", false);
    }

    private void AnaCheck()
    {
        Input.DisableInput(PlayerInputNames.Move);
        
        SpeedZero();

        animator.SetBool("IsDigging", true);

        bool AnaUme = false;

        // ���C�L���X�g�̌��ʂ��i�[����ϐ�
        RaycastHit hit;

        Vector3 pos = transform.position + (transform.forward * -0.1f);
        pos.y = 1f;

        // ���C�L���X�g�̎��s
        if (Physics.Raycast(pos, transform.forward, out hit, AnaSize*2f))
        {
            Debug.Log(hit.collider.gameObject.name);
            AnaAke anaAke = hit.collider.gameObject.GetComponent<AnaAke>();
            if (anaAke != null)
            {
                if (anaAke.Ume)
                {
                    AnaAke();
                }
                else
                {
                    anaAke.AnaUme(AnaHoriTime);
                    AnaUme = true;
                }
            }
        }
        else
        {
            AnaAke();
        }

        StartCoroutine(AnaHoriEndWait(AnaUme));
    }

    private void AnaAke()
    {
        // �z�u�ʒu�̌v�Z
        Vector3 spawnPosition = transform.position + transform.forward * (AnaSize * 1.25f);
        spawnPosition.y += 0.5f;
        // �I�u�W�F�N�g�𐶐�
        nowAna = Instantiate(Ana, spawnPosition, transform.rotation);
        AnaAke anaAke = nowAna.GetComponent<AnaAke>();
        anaAke.AnaHoriStart(AnaSize, AnaHoriTime);
    }
    #endregion
}
