using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public enum InputNames
{
    PlayerInputNames,
    UIInputNames
}

public enum PlayerInputNames
{
    Set = -1,
    Move = 0,
    An = 1
}

public enum ActionSettype
{
    plus,
    minus,
}

public class Input : MonoBehaviour
{
    private PlayerInput input = null;
    protected InputAction[] actions = new InputAction[32];
    public InputAction[] inputActions { get { return actions; } }
    protected List<string>[] MethodNames = new List<string>[32];

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        SetUp();
    }

    protected void SetUp() { SetUp(PlayerInputNames.Set); }

    private void SetUp<T>(T useEnum) where T : Enum
    {
        if (input == null)
        {
            Debug.LogError("Not found " + "Input" + "_|_" + this.name);
            return;
        }

        for (int i = 0; i < actions.Length; i++)
        {
            if (Enum.IsDefined(typeof(T), i))
            {
                T enumValue = (T)Enum.ToObject(typeof(T), i);
                actions[i] = input.actions[enumValue.ToString()];
            }
        }
    }

    public void MethodSetting(Enum inputName, ActionSettype actionSettype, Action<InputAction.CallbackContext>? callPerformed, Action<InputAction.CallbackContext>? callCanceled)
    {
        int index = inputName.GetHashCode();

        if (index < 0 || index >= actions.Length) return;
        if (MethodNames[index] == null) MethodNames[index] = new List<string>();

        switch (actionSettype)
        {
            case ActionSettype.plus:
                if (callPerformed != null && !MethodNames[index].Contains(callPerformed.Method.Name))
                {
                    actions[index].performed += callPerformed;
                    MethodNames[index].Add(callPerformed.Method.Name);
                }
                if (callCanceled != null && !MethodNames[index].Contains(callCanceled.Method.Name))
                {
                    actions[index].canceled += callCanceled;
                    MethodNames[index].Add(callCanceled.Method.Name);
                }
                break;
            case ActionSettype.minus:
                if (callPerformed != null && MethodNames[index].Contains(callPerformed.Method.Name))
                {
                    actions[index].performed -= callPerformed;
                    MethodNames[index].Remove(callPerformed.Method.Name);
                }
                if (callCanceled != null && MethodNames[index].Contains(callCanceled.Method.Name))
                {
                    actions[index].canceled -= callCanceled;
                    MethodNames[index].Remove(callCanceled.Method.Name);
                }
                break;
        }
    }

    #region 入力の有効化/無効化
    /// <summary> 入力を有効化 </summary>
    public void AllOn() { input.actions.Enable(); }
    /// <summary> 入力を無効化 </summary>
    public void AllOff() { input.actions.Disable(); }

    /// <summary> 指定した入力を有効化 </summary>
    public void EnableInput(Enum inputName)
    {
        int index = inputName.GetHashCode();
        if (index >= 0 && index < actions.Length && actions[index] != null)
        {
            actions[index].Enable();
        }
    }

    /// <summary> 指定した入力を無効化 </summary>
    public void DisableInput(Enum inputName)
    {
        int index = inputName.GetHashCode();
        if (index >= 0 && index < actions.Length && actions[index] != null)
        {
            actions[index].Disable();
        }
    }

    #endregion
}
