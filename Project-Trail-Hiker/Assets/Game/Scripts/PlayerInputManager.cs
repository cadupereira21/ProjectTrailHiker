using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    public bool aWasPressed { private set; get; } = false;
    public bool dWasPressed { private set; get; } = false;
    public float aDownTime { private set; get; } = 0.00f;
    public float dDownTime { private set; get; } = 0.00f;
    public float walkInputAverageTime { private set; get; } = 0.00f;
    public bool wantsToMove { private set; get; }

    public void Update()
    {
        Debug.Log("walkInputAverageTime = " + walkInputAverageTime);

        if(aWasPressed && dWasPressed)
        {
            walkInputAverageTime = Mathf.Abs(aDownTime - dDownTime);
        }
        else if (aWasPressed)
        {
            walkInputAverageTime = Time.time - aDownTime;
        }
        else if (dWasPressed)
        {
            walkInputAverageTime = Time.time - dDownTime;
        }

        if(walkInputAverageTime > 1f)
        {
            wantsToMove = false;
            walkInputAverageTime = 0f;
        }
        else
        {
            wantsToMove = true;
        }

        ProcessMovement();
    }

    public void ProcessMovement()
    {
        if (IsAButtonDown())
        {
            aDownTime = Time.time;
            aWasPressed = true;
            wantsToMove = true;
        }

        if (IsDButtonDown())
        {
            dDownTime = Time.time;
            dWasPressed = true;
            wantsToMove = true;
        }

        if (aWasPressed && dWasPressed)
        {
            Debug.Log("I'm Here!");
            aWasPressed = false;
            dWasPressed = false;
            wantsToMove = false;
        }
    }

    public bool IsAButtonDown()
    {
        return Input.GetKeyDown(KeyCode.A);
    }

    public bool IsDButtonDown()
    {
        return Input.GetKeyDown(KeyCode.D);
    }

    //public bool IsAButtonHold()
    //{
    //    return Input.GetKey(KeyCode.A);
    //}

    //public bool IsDButtonHold()
    //{
     //   return Input.GetKey(KeyCode.D);
    //}

    public bool IsSwipeDirectionButtonDown()
    {
        return Input.GetKeyDown(KeyCode.Q);
    }
}
