using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    public bool aWasPressed = false;
    public bool dWasPressed = false;

    public float aTime { private set; get; } = 0f;
    public float dTime { private set; get; } = 0f;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (!aWasPressed)
            {
                aWasPressed = true;
                aTime = Time.time;
            }
            else
            {
                aWasPressed = false;
                //TODO: cair = true
            }
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            if (!dWasPressed)
            {
                dWasPressed = true;
                dTime = Time.time;
            }
            else
            {
                dWasPressed = false;
                //TODO: cair = true
            }
        }
    }

    public bool IsJumpButtonHold()
    {
        return Input.GetKey(KeyCode.W);
    }

    public bool IsSwipeDirectionButtonDown()
    {
        return Input.GetKeyDown(KeyCode.Q);
    }
}
