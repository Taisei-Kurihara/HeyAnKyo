using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{

    [SerializeField]
    private Transform m_Camera;
    [SerializeField]
    public Transform player;

    Vector3 m_Position = new Vector3(0,17,-5);

    private void Awake()
    {
        StartCoroutine(CameraMoveWait());
    }

    IEnumerator CameraMoveWait()
    {

        yield return new WaitUntil(() => player != null);
        yield return new WaitUntil(() => m_Position != (m_Camera.position - player.position));
        m_Camera.position = player.position + m_Position;
        StartCoroutine(CameraMoveWait());
    }
}
