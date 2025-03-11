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
            yield return new WaitForSeconds(1/30);

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
            Destroy(nowAna);
        }
    }

    public void AnaCheck()
    {// レイキャストの結果を格納する変数
        RaycastHit hit;

        // レイキャストの実行
        if (Physics.Raycast(transform.position, transform.forward, out hit, AnaSize))
        {
            StartCoroutine(FadeWait(-AnaHoriTime, AnaHoriTime));
        }
        else
        {
            // 配置位置の計算
            Vector3 spawnPosition = transform.position + transform.forward * (AnaSize / 2);

            // オブジェクトを生成
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
