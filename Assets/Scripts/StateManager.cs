
using System;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public float health;
    public State currentState;

    public float delta;
    public Transform mTransform;

    private void Start()
    {
        mTransform = transform;
    }

    private void Update()
    {
        if(!currentState) return;
        currentState.Tick(this);
    }
}
