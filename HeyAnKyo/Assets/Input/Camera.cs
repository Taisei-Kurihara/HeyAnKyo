using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{

    [SerializeField]
    private Transform m_Camera;
    [SerializeField]
    private Transform player;

    Vector3 m_Position;

    private void Awake()
    {
        m_Position = m_Camera.position - player.position;
        StartCoroutine(CameraMoveWait());
    }

    IEnumerator CameraMoveWait()
    {
        yield return new WaitUntil(() => m_Position != (m_Camera.position - player.position));
        m_Camera.position = player.position + m_Position;
        StartCoroutine(CameraMoveWait());
    }
}
