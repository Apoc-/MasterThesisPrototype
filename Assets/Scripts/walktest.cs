using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class walktest : MonoBehaviour
{
    public Transform[] Waypoints;

    public int TargetIdx;
    public int LastIdx;

    public float Speed = 1000;

    private float startTime = 0;
    
    private void Start()
    {
        this.transform.position = Waypoints[0].position;

        LastIdx = 0;
        TargetIdx = 1;
        startTime = Time.time;
    }

    
    private void FixedUpdate()
    {
        var covered = (Time.time - startTime) * Speed;
        var progress = covered / Vector3.Magnitude(Target() - Last());

        transform.position = Vector3.Lerp(Last(), Target(), progress);

        if (Vector3.Distance(Target(), transform.position) <= .01f)
        {
            LastIdx = TargetIdx;
            TargetIdx += 1;
            startTime = Time.time;

            if (TargetIdx >= Waypoints.Length)
            {
                TargetIdx = 0;
            }
        }
        
        SetLookingDirection();
    }

    private void SetLookingDirection()
    {
        var x = transform.position.x;
        var targetX = Target().x;

        var scale = transform.localScale;
        scale.x = Mathf.Sign(x-targetX);
        transform.localScale = scale;
    }

    private Vector3 Target()
    {
        return Waypoints[TargetIdx].transform.position;
    }

    private Vector3 Last()
    {
        return Waypoints[LastIdx].transform.position;
    }
}