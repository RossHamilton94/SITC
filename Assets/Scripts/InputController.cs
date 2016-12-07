using UnityEngine;
using System.Collections;

#if UNITY_STANDALONE_WIN
using XInputDotNetPure;
#endif


public class InputController : MonoBehaviour
{
    bool playerIndexSet = false;

#if UNITY_STANDALONE_WIN
    PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;
#else
    int playerIndex;
    UnityGamePadState state = new UnityGamePadState();
    UnityGamePadState prevState = new UnityGamePadState();
#if UNITY_STANDALONE_OSX
    string platformCharacter = "M";
#else
    string platformCharacter = "L";
#endif

#endif

    void Update()
    {
        if (!playerIndexSet)
            return;

#if UNITY_STANDALONE_WIN
        prevState = state;
        state = GamePad.GetState(playerIndex);
#else
        state.SetState(prevState);
        state.GetState(platformCharacter, playerIndex);      
#endif

    }

    public void SetPlayer(int playerNumber)
    {
#if UNITY_STANDALONE_WIN
        playerIndex = (PlayerIndex)(playerNumber);
        playerIndexSet = true;
        //Debug.Log(playerIndex);
#else
        playerIndex = playerNumber;
        playerIndexSet = true;
#endif
    }

    private void CheckControllerState(int index)
    {

    }

    #region Buttons
    public bool PressedA()
    {
        bool pressed = false;
#if UNITY_STANDALONE_WIN
        if (prevState.Buttons.A == ButtonState.Released && state.Buttons.A == ButtonState.Pressed)
#else
        if (!prevState.buttonA && state.buttonA)
#endif
        {
            pressed = true;
        }
        else
        {

        }
        return pressed;
    }

    public bool PressedB()
    {
        bool pressed = false;
#if UNITY_STANDALONE_WIN
        if (prevState.Buttons.B == ButtonState.Released && state.Buttons.B == ButtonState.Pressed)
#else
        if (!prevState.buttonB && state.buttonB)
#endif
        {
            pressed = true;
        }
        else
        {

        }
        return pressed;
    }

    //    public bool HoldingB()
    //    {
    //        bool pressed = false;
    //#if UNITY_STANDALONE_WIN
    //        if (prevState.Buttons.B == ButtonState.Released && state.Buttons.B == ButtonState.Pressed)
    //#else
    //        if (!prevState.buttonB && state.buttonB)
    //#endif
    //        {
    //            pressed = true;
    //        }
    //        else
    //        {

    //        }
    //        return pressed;
    //    }

    public bool PressedX()
    {
        bool pressed = false;
#if UNITY_STANDALONE_WIN
        if (prevState.Buttons.X == ButtonState.Released && state.Buttons.X == ButtonState.Pressed)
#else
        if (!prevState.buttonX && state.buttonX)
#endif
        {
            pressed = true;
        }
        else
        {

        }
        return pressed;
    }

    //public bool PressedY()
    //{
    //    bool pressed = false;
    //    //if (prevState.Buttons.Y == ButtonState.Released && state.Buttons.Y == ButtonState.Pressed)
    //    if (!prevState.buttonY && state.buttonY)
    //    {
    //        pressed = true;
    //    }
    //    else
    //    {

    //    }
    //    return pressed;
    //}

    //public bool PressedBack()
    //{
    //    bool pressed = false;
    //    if (prevState.Buttons.Back == ButtonState.Released && state.Buttons.Back == ButtonState.Pressed)
    //        if (!prevState.back && state.back)
    //        {
    //            pressed = true;
    //        }
    //        else
    //        {

    //        }
    //    return pressed;
    //}

    public bool PressedStart()
    {
        bool pressed = false;
#if UNITY_STANDALONE_WIN
        if (prevState.Buttons.Start == ButtonState.Released && state.Buttons.Start == ButtonState.Pressed)
#else
        if (!prevState.start && state.start)
#endif
        {
            pressed = true;
        }
        else
        {

        }
        return pressed;
    }

    public bool PressedLeftShoulder()
    {
        bool pressed = false;
#if UNITY_STANDALONE_WIN
        if (prevState.Buttons.LeftShoulder == ButtonState.Released && state.Buttons.LeftShoulder == ButtonState.Pressed)
#else
        if (!prevState.leftShoulder && state.leftShoulder)
#endif
        {
            pressed = true;
        }
        else
        {

        }
        return pressed;
    }

    public bool PressedRightShoulder()
    {
        bool pressed = false;
#if UNITY_STANDALONE_WIN
        if (prevState.Buttons.RightShoulder == ButtonState.Released && state.Buttons.RightShoulder == ButtonState.Pressed)
#else
        if (!prevState.rightShoulder && state.rightShoulder)
#endif
        {
            pressed = true;
        }
        else
        {

        }
        return pressed;
    }

    //public bool PressedGuide()
    //{
    //    bool pressed = false;
    //    //if (prevState.Buttons.Guide == ButtonState.Released && state.Buttons.Guide == ButtonState.Pressed)
    //    if (!prevState.guide && state.guide)
    //    {
    //        pressed = true;
    //    }
    //    else
    //    {

    //    }
    //    return pressed;
    //}

    #endregion

    #region DPad
    //    public bool PressedDUp()
    //    {
    //        bool pressed = false;
    //#if UNITY_STANDALONE_WIN
    //        if (prevState.DPad.Up == ButtonState.Released && state.DPad.Up == ButtonState.Pressed)
    //#else
    //        if (!prevState.dUp && state.dUp)
    //#endif
    //        {
    //            pressed = true;
    //        }
    //        else
    //        {

    //        }
    //        return pressed;
    //    }

    //    public bool PressedDDown()
    //    {
    //        bool pressed = false;
    //        //if (prevState.DPad.Down == ButtonState.Released && state.DPad.Down == ButtonState.Pressed)
    //        if (!prevState.dDown && state.dDown)
    //        {
    //            pressed = true;
    //        }
    //        else
    //        {

    //        }
    //        return pressed;
    //    }

    //    public bool PressedDLeft()
    //    {
    //        bool pressed = false;
    //        //if (prevState.DPad.Left == ButtonState.Released && state.DPad.Left == ButtonState.Pressed)
    //        if (!prevState.dLeft && state.dLeft)
    //        {
    //            pressed = true;
    //        }
    //        else
    //        {

    //        }
    //        return pressed;
    //    }

    //    public bool PressedDRight()
    //    {
    //        bool pressed = false;
    //        //if (prevState.DPad.Right == ButtonState.Released && state.DPad.Right == ButtonState.Pressed)
    //        if (!prevState.dRight && state.dRight)
    //        {
    //            pressed = true;
    //        }
    //        else
    //        {

    //        }
    //        return pressed;
    //    }

    #endregion

    #region Analogues
    public float LeftHorizontal()
    {
#if UNITY_STANDALONE_WIN
        return state.ThumbSticks.Left.X;
#else
        return state.leftHorizontal;
#endif
    }

    public float LeftVertical()
    {
#if UNITY_STANDALONE_WIN
        return state.ThumbSticks.Left.Y;
#else
        return state.leftVertical;
#endif
    }

    public float RightHorizontal()
    {
#if UNITY_STANDALONE_WIN
        return state.ThumbSticks.Right.X;
#else
        return state.rightHorizontal;
#endif
    }

    public float RightVertical()
    {
#if UNITY_STANDALONE_WIN
        return state.ThumbSticks.Right.Y;
#else
        return state.rightVertical;
#endif
    }

    public float LeftTrigger()
    {
#if UNITY_STANDALONE_WIN
        return state.Triggers.Left;
#else
        return state.leftTrigger;
#endif
    }

    public float RightTrigger()
    {
#if UNITY_STANDALONE_WIN
        return state.Triggers.Right;
#else
        return state.rightTrigger;
#endif
    }

    #endregion

    public void VibrateStart()
    {
        StartCoroutine(VibrateController(0.5f));
    }

    IEnumerator VibrateController(float timeToWait)
    {
        GamePad.SetVibration(playerIndex, 0.5f, 0.5f);
        yield return new WaitForSeconds(timeToWait);
        GamePad.SetVibration(playerIndex, 0f, 0f);
    }
}

