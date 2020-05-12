using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class BallAgent : Agent
{
    private Rigidbody ballRigidbody; // 볼의 리지드바디
    public Transform pivotTransform; // 위치의 기준점
    public Transform target; // 아이템 목표
    
    public float moveforce = 10f; // 이동 힘
    private bool targetEaten = false;
    private bool dead = false;

    void Awake()
    {
        ballRigidbody = GetComponent<Rigidbody>();
    }
}
