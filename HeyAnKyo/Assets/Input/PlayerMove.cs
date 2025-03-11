using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    Input Input;
    Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
        Input = GetComponent<Input>();
        Input.MethodSetting(PlayerInputNames.Move,ActionSettype.plus, OnMove, OutMove);
        Input.MethodSetting(PlayerInputNames.An,ActionSettype.plus,null,null);

        rb = GetComponent<Rigidbody>();
    }

    protected Vector2 DeadZone(Vector2 Input, float deadZone)
    {
        return (Input.magnitude <= deadZone) ? Input = Vector2.zero : Input;
    }


    // ˆÚ“®“ü—ÍŠÖ˜A
    private Vector2 MoveDis = Vector2.zero;
    private float MoveDeadZone = 0.2f;
    [SerializeField]
    private float speed = 4f;

    #region ˆÚ“®“ü—Í
    public void OnMove(InputAction.CallbackContext context)
    {
        MoveDis = DeadZone(Input.inputActions[(int)PlayerInputNames.Move].ReadValue<Vector2>(), MoveDeadZone);

        rb.velocity = new Vector3(MoveDis.x, 0, MoveDis.y) * speed;
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
}
