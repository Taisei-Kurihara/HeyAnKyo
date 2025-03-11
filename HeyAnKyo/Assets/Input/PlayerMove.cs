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




    #region 移動入力

    // 移動入力関連
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

    #region 穴掘り

    [SerializeField]
    GameObject Ana;

    GameObject nowAna;

    private float AnaHoriTime = 1f;

    float AnaSize = 5;

    BitArray AnaHoribool = new BitArray(2,false);

    public void OnAnaHori(InputAction.CallbackContext context)
    {
        AnaCheck();
        AnaHoribool = new BitArray(2, true);
    }

    public void OutAnaHori(InputAction.CallbackContext context)
    {

        AnaHoribool = new BitArray(2, false);
    }

    IEnumerator AnaHoriInputEndWait()
    {
        yield return new WaitUntil(() => !AnaHoribool[0] || !AnaHoribool[1]);
        AnaHoriEnd();
    }

    void AnaHoriEndWait() { AnaHoribool[1] = false; }

    void AnaHoriEnd()
    {
        if (nowAna != null)
        {
            AnaAke anaAke = nowAna.GetComponent<AnaAke>();
            anaAke.AnaUme(AnaHoriTime);
        }

        Input.EnableInput(PlayerInputNames.Move);
        nowAna = null;

    }

    public void AnaCheck()
    {// レイキャストの結果を格納する変数
        RaycastHit hit;

        // レイキャストの実行
        if (Physics.Raycast(transform.position + (transform.forward * 2), transform.forward, out hit, AnaSize/2))
        {
            // オブジェクトを生成
            AnaAke anaAke = hit.collider.GetComponent<AnaAke>();
            if (anaAke != null)
            {
                anaAke.AnaUme(AnaHoriTime);

                Input.DisableInput(PlayerInputNames.Move);
                Invoke(nameof(AnaHoriEndWait), AnaHoriTime);
            }
        }
        else
        {
            // 配置位置の計算
            Vector3 spawnPosition = transform.position + transform.forward * (AnaSize / 2);

            // オブジェクトを生成
            nowAna = Instantiate(Ana, spawnPosition, transform.rotation);
            AnaAke anaAke = nowAna.GetComponent<AnaAke>();
            anaAke.AnaHoriStart(AnaSize, AnaHoriTime);

            Input.DisableInput(PlayerInputNames.Move);
            Invoke(nameof(AnaHoriEndWait), AnaHoriTime);

        }
    }
    #endregion
}
