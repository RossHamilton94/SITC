using UnityEngine;
//using System.Collections;

public class UnityGamePadState
{
    public bool buttonA = false;
    public bool buttonB = false;
    public bool buttonX = false;
    public bool buttonY = false;
    public bool leftShoulder = false;
    public bool rightShoulder = false;
    public bool start = false;
    public bool back = false;
    public bool guide = false;
    public bool dUp = false;
    public bool dDown = false;
    public bool dLeft = false;
    public bool dRight = false;
    public float leftHorizontal = 0;
    public float leftVertical = 0;
    public float rightHorizontal = 0;
    public float rightVertical = 0;
    public float leftTrigger = 0;
    public float rightTrigger = 0;

    public void GetState(string platform, int index)
    {
        buttonA = Input.GetButton((platform + "ButtonAP" + (index + 1)));
        buttonB = Input.GetButton((platform + "ButtonBP" + (index + 1)));
        buttonX = Input.GetButton((platform + "ButtonXP" + (index + 1)));
        //buttonY = Input.GetButton(("ButtonYP" + (index + 1)));
        leftShoulder = Input.GetButton((platform + "LeftShoulderP" + (index + 1)));
        rightShoulder = Input.GetButton((platform + "RightShoulderP" + (index + 1)));
        start = Input.GetButton((platform + "StartP" + (index + 1)));
        //back = Input.GetButton(("BackP" + (index + 1)));
        //guide = Input.GetButton(("GuideP" + (index + 1)));
        //dUp = Input.GetButton(("DUpP" + (index + 1)));
        //dDown = Input.GetButton(("DDownP" + (index + 1)));
        //dLeft = Input.GetButton(("DLeftP" + (index + 1)));
        //dRight = Input.GetButton(("DRightP" + (index + 1)));
        leftHorizontal = Input.GetAxis(("LeftHorizontalP" + (index + 1)));
        leftVertical = Input.GetAxis(("LeftVerticalP" + (index + 1)));
        rightHorizontal = Input.GetAxis(("RightHorizontalP" + (index + 1)));
        rightVertical = Input.GetAxis(("RightVerticalP" + (index + 1)));
        leftTrigger = Input.GetAxis((platform + "LeftTriggerP" + (index + 1)));
        rightTrigger = Input.GetAxis(("RightTriggerP" + (index + 1)));
    }
    public void SetState(UnityGamePadState prevState)
    {
        prevState.buttonA = this.buttonA;
        prevState.buttonB = this.buttonB;
        prevState.buttonX = this.buttonX;
        prevState.leftShoulder = this.leftShoulder;
        prevState.rightShoulder = this.rightShoulder;
        prevState.start = this.start;
    }
}
