using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

//ボタン列挙
public enum XButtonType
{
    A,
    B,
    X,
    Y,

    Start,
    Back,
    Guide,
    LShoulder,
    RShoulder,

    //Stick押し込み
    LThumbStick,
    RThumbStick,
    //Trigger
    LTrigger,
    RTrigger,
    //Sticks
    LThumbStickUp,
    LThumbStickDown,
    LThumbStickLeft,
    LThumbStickRight,
    RThumbStickUp,
    RThumbStickDown,
    RThumbStickLeft,
    RThumbStickRight,
}




public class XInputManager : MonoBehaviour
{
    private void Update()
    {
        UpdateXInput();
    }

    private void FixedUpdate()
    {
        FixedUpdateXInput();
    }

    //**********************************************************
    //static
    //**********************************************************
    //構造体
    struct GamePadThumbStickState
    {
        public ButtonState up;
        public ButtonState down;
        public ButtonState left;
        public ButtonState right;
    }

    struct GamePadTriggersState
    {
        public ButtonState leftTrigger;
        public ButtonState rightTrigger;
    }

    struct GamePadStatePlus
    {
        public GamePadState origin;

        public GamePadTriggersState triggers;

        public GamePadThumbStickState leftStick;
        public GamePadThumbStickState rightStick;

        public float leftVibration;
        public float rightVibration;
    }

    //スティックのトリガー発動最低値
    static float THUMB_STICK_TRIGGER_MIN = 0.9f;


    //最大接続数
    private const int MAX_GAMEPAD = 4;

    //GamePad状態保持
    static GamePadStatePlus[] gamePadState = new GamePadStatePlus[MAX_GAMEPAD];
    static GamePadStatePlus[] prevGamePadState = new GamePadStatePlus[MAX_GAMEPAD];

    private static void UpdateXInput()
    {
        for (int i = 0; i < MAX_GAMEPAD; i++)
        {
            //すべての状態を一時記録
            prevGamePadState[i] = gamePadState[i];

            //ゲームパッド状態の更新
            gamePadState[i].origin = GamePad.GetState((PlayerIndex)i);

            //Triggerの更新
            UpdateTrigger(ref gamePadState[i].triggers.leftTrigger, GetLeftTrigger(i));
            UpdateTrigger(ref gamePadState[i].triggers.rightTrigger, GetRightTrigger(i));

            //Stickの更新
            UpdateThumbStick(ref gamePadState[i].leftStick, gamePadState[i].origin.ThumbSticks.Left);
            UpdateThumbStick(ref gamePadState[i].rightStick, gamePadState[i].origin.ThumbSticks.Right);
        }
    }

    private static void FixedUpdateXInput()
    {
        for (int i = 0; i < 4; i++)
        {
            GamePad.SetVibration((PlayerIndex)i, gamePadState[i].leftVibration, gamePadState[i].rightVibration);
            gamePadState[i].leftVibration = 0.0f;
            gamePadState[i].rightVibration = 0.0f;
        }
    }

    public static void SetVibration(int _index, float _leftMotor, float _rightMotor)
    {
        //GamePad.SetVibration((PlayerIndex)_index, _leftMotor, _rightMotor);
        //直接値を変更せず、FixedUpdateに任せる
        gamePadState[_index].leftVibration = _leftMotor;
        gamePadState[_index].rightVibration = _rightMotor;
    }

    public static void SetVibrationHigher(int _index, float _leftMotor, float _rightMotor)
    {
        if (gamePadState[_index].leftVibration < _leftMotor)
        {
            gamePadState[_index].leftVibration = _leftMotor;
        }
        if (gamePadState[_index].rightVibration < _rightMotor)
        {
            gamePadState[_index].rightVibration = _rightMotor;
        }
    }

    public static float GetLeftTrigger(int _index)
    {
        return gamePadState[_index].origin.Triggers.Left;
    }

    public static float GetRightTrigger(int _index)
    {
        return gamePadState[_index].origin.Triggers.Right;
    }

    public static float GetThumbStickLeftX(int _index)
    {
        return gamePadState[_index].origin.ThumbSticks.Left.X;
    }

    public static float GetThumbStickLeftY(int _index)
    {
        return gamePadState[_index].origin.ThumbSticks.Left.Y;
    }

    public static float GetThumbStickRightX(int _index)
    {
        return gamePadState[_index].origin.ThumbSticks.Right.X;
    }

    public static float GetThumbStickRightY(int _index)
    {
        return gamePadState[_index].origin.ThumbSticks.Right.Y;
    }

    public static bool GetButtonTrigger(int _index, XButtonType _type)
    {
        return GetButtonState(gamePadState[_index], _type) == ButtonState.Pressed && GetButtonState(prevGamePadState[_index], _type) == ButtonState.Released;
    }

    public static bool GetButtonRelease(int _index, XButtonType _type)
    {
        return GetButtonState(gamePadState[_index], _type) == ButtonState.Released && GetButtonState(prevGamePadState[_index], _type) == ButtonState.Pressed;
    }

    public static bool GetButtonPress(int _index, XButtonType _type)
    {
        return GetButtonState(gamePadState[_index], _type) == ButtonState.Pressed;
    }

    public static bool IsConnected(int _index)
    {
        return gamePadState[_index].origin.IsConnected;
    }

    //PressedかReleaseかをGamePadStatePlusの内容からXButtonTypeから判断
    private static ButtonState GetButtonState(GamePadStatePlus _target, XButtonType _type)
    {
        switch (_type)
        {
            case XButtonType.A:
                return _target.origin.Buttons.A;
            case XButtonType.B:
                return _target.origin.Buttons.B;
            case XButtonType.X:
                return _target.origin.Buttons.X;
            case XButtonType.Y:
                return _target.origin.Buttons.Y;

            case XButtonType.LThumbStick:
                return _target.origin.Buttons.LeftStick;
            case XButtonType.RThumbStick:
                return _target.origin.Buttons.RightStick;

            case XButtonType.Start:
                return _target.origin.Buttons.Start;
            case XButtonType.Back:
                return _target.origin.Buttons.Back;
            case XButtonType.Guide:
                return _target.origin.Buttons.Guide;
            case XButtonType.LShoulder:
                return _target.origin.Buttons.LeftShoulder;
            case XButtonType.RShoulder:
                return _target.origin.Buttons.RightShoulder;

            case XButtonType.LTrigger:
                return _target.triggers.leftTrigger;
            case XButtonType.RTrigger:
                return _target.triggers.rightTrigger;

            case XButtonType.LThumbStickUp:
                return _target.leftStick.up;
            case XButtonType.LThumbStickDown:
                return _target.leftStick.down;
            case XButtonType.LThumbStickLeft:
                return _target.leftStick.left;
            case XButtonType.LThumbStickRight:
                return _target.leftStick.right;

            case XButtonType.RThumbStickUp:
                return _target.rightStick.up;
            case XButtonType.RThumbStickDown:
                return _target.rightStick.down;
            case XButtonType.RThumbStickLeft:
                return _target.rightStick.left;
            case XButtonType.RThumbStickRight:
                return _target.rightStick.right;

            default:
                Debug.LogError("返すButtonStateが設定されていません");
                return new ButtonState();
        }
    }

    private static void UpdateTrigger(ref ButtonState _target, float _value)
    {
        if (_value > 0.0f)
        {
            _target = ButtonState.Pressed;
        }
        else
        {
            _target = ButtonState.Released;
        }
    }

    private static void UpdateThumbStick(ref GamePadThumbStickState _target, GamePadThumbSticks.StickValue _value)
    {
        if (_value.X < -THUMB_STICK_TRIGGER_MIN)
        {
            _target.left = ButtonState.Pressed;
        }
        else
        {
            _target.left = ButtonState.Released;
        }

        if (_value.X > THUMB_STICK_TRIGGER_MIN)
        {
            _target.right = ButtonState.Pressed;
        }
        else
        {
            _target.right = ButtonState.Released;
        }

        if (_value.Y > THUMB_STICK_TRIGGER_MIN)
        {
            _target.up = ButtonState.Pressed;
        }
        else
        {
            _target.up = ButtonState.Released;
        }

        if (_value.Y < -THUMB_STICK_TRIGGER_MIN)
        {
            _target.down = ButtonState.Pressed;
        }
        else
        {
            _target.down = ButtonState.Released;
        }
    }

}
