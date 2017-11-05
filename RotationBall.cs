using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationBall : MonoBehaviour
{
    [SerializeField]
    private int m_Velocity;

    private void Update()
    {
        GetComponent<Transform>().Rotate(0, Time.deltaTime * m_Velocity, 0);
    }
}
