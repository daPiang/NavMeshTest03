using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum AIState
    {
        Action1,
        Action2
    }

    public static AIState aiState;

    public void Action1()
    {
        aiState = AIState.Action1;
    }

    public void Action2()
    {
        aiState = AIState.Action2;
    }
}
