/* Author:          熊哲
 * CreateTime:      2018-04-18 17:02:37
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZUnity.GroupedKeyCode
{
    public enum JoystickKeyCode
    {
        None = KeyCode.None,

        Button0 = KeyCode.JoystickButton0,
        Button1 = KeyCode.JoystickButton1,
        Button2 = KeyCode.JoystickButton2,
        Button3 = KeyCode.JoystickButton3,
        Button4 = KeyCode.JoystickButton4,
        Button5 = KeyCode.JoystickButton5,
        Button6 = KeyCode.JoystickButton6,
        Button7 = KeyCode.JoystickButton7,
        Button8 = KeyCode.JoystickButton8,
        Button9 = KeyCode.JoystickButton9,
        Button10 = KeyCode.JoystickButton10,
        Button11 = KeyCode.JoystickButton11,
        Button12 = KeyCode.JoystickButton12,
        Button13 = KeyCode.JoystickButton13,
        Button14 = KeyCode.JoystickButton14,
        Button15 = KeyCode.JoystickButton15,
        Button16 = KeyCode.JoystickButton16,
        Button17 = KeyCode.JoystickButton17,
        Button18 = KeyCode.JoystickButton18,
        Button19 = KeyCode.JoystickButton19,
    }

    public enum AlphaKeyCode
    {
        None = KeyCode.None,

        Alpha0 = KeyCode.Alpha0,
        Alpha1 = KeyCode.Alpha1,
        Alpha2 = KeyCode.Alpha2,
        Alpha3 = KeyCode.Alpha3,
        Alpha4 = KeyCode.Alpha4,
        Alpha5 = KeyCode.Alpha5,
        Alpha6 = KeyCode.Alpha6,
        Alpha7 = KeyCode.Alpha7,
        Alpha8 = KeyCode.Alpha8,
        Alpha9 = KeyCode.Alpha9,

        A = KeyCode.A,
        B = KeyCode.B,
        C = KeyCode.C,
        D = KeyCode.D,
        E = KeyCode.E,
        F = KeyCode.F,
        G = KeyCode.G,
        H = KeyCode.H,
        I = KeyCode.I,
        J = KeyCode.J,
        K = KeyCode.K,
        L = KeyCode.L,
        M = KeyCode.M,
        N = KeyCode.N,
        O = KeyCode.O,
        P = KeyCode.P,
        Q = KeyCode.Q,
        R = KeyCode.R,
        S = KeyCode.S,
        T = KeyCode.T,
        U = KeyCode.U,
        V = KeyCode.V,
        W = KeyCode.W,
        X = KeyCode.X,
        Y = KeyCode.Y,
        Z = KeyCode.Z,
    }

    public enum FunctionKeyCode
    {
        None = KeyCode.None,

        Escape = KeyCode.Escape,
        F1 = KeyCode.F1,
        F2 = KeyCode.F2,
        F3 = KeyCode.F3,
        F4 = KeyCode.F4,
        F5 = KeyCode.F5,
        F6 = KeyCode.F6,
        F7 = KeyCode.F7,
        F8 = KeyCode.F8,
        F9 = KeyCode.F9,
        F10 = KeyCode.F10,
        F11 = KeyCode.F11,
        F12 = KeyCode.F12,
        F13 = KeyCode.F13,
        F14 = KeyCode.F14,
        F15 = KeyCode.F15,
        Print = KeyCode.Print,
        ScrollLock = KeyCode.ScrollLock,
        Pause = KeyCode.Pause,
        Break = KeyCode.Break,
    }

    public enum KeypadKeyCode
    {
        None = KeyCode.None,

        Numlock = KeyCode.Numlock,
        KeypadPeriod = KeyCode.KeypadPeriod,
        KeypadDivide = KeyCode.KeypadDivide,
        KeypadMultiply = KeyCode.KeypadMultiply,
        KeypadMinus = KeyCode.KeypadMinus,
        KeypadPlus = KeyCode.KeypadPlus,
        KeypadEnter = KeyCode.KeypadEnter,
        KeypadEquals = KeyCode.KeypadEquals,

        Keypad0 = KeyCode.Keypad0,
        Keypad1 = KeyCode.Keypad1,
        Keypad2 = KeyCode.Keypad2,
        Keypad3 = KeyCode.Keypad3,
        Keypad4 = KeyCode.Keypad4,
        Keypad5 = KeyCode.Keypad5,
        Keypad6 = KeyCode.Keypad6,
        Keypad7 = KeyCode.Keypad7,
        Keypad8 = KeyCode.Keypad8,
        Keypad9 = KeyCode.Keypad9,
    }

    public enum PunctuationKeyCode
    {
        None = KeyCode.None,

        Exclaim = KeyCode.Exclaim,
        At = KeyCode.At,
        Hash = KeyCode.Hash,
        Dollar = KeyCode.Dollar,
        Caret = KeyCode.Caret,
        Ampersand = KeyCode.Ampersand,
        Asterisk = KeyCode.Asterisk,
        LeftParen = KeyCode.LeftParen,
        RightParen = KeyCode.RightParen,
        Minus = KeyCode.Minus,
        Underscore = KeyCode.Underscore,
        Equals = KeyCode.Equals,
        Plus = KeyCode.Plus,

        LeftBracket = KeyCode.LeftBracket,
        RightBracket = KeyCode.RightBracket,
        Backslash = KeyCode.Backslash,
        Semicolon = KeyCode.Semicolon,
        Colon = KeyCode.Colon,
        Quote = KeyCode.Quote,
        BackQuote = KeyCode.BackQuote,
        DoubleQuote = KeyCode.DoubleQuote,
        Comma = KeyCode.Comma,
        Less = KeyCode.Less,
        Period = KeyCode.Period,
        Greater = KeyCode.Greater,
        Slash = KeyCode.Slash,
        Question = KeyCode.Question,
        Space = KeyCode.Space,
    }

    public enum EditKeyCode
    {
        None = KeyCode.None,

        Insert = KeyCode.Insert,
        Delete = KeyCode.Delete,
        Home = KeyCode.Home,
        End = KeyCode.End,
        PageUp = KeyCode.PageUp,
        PageDown = KeyCode.PageDown,

        UpArrow = KeyCode.UpArrow,
        DownArrow = KeyCode.DownArrow,
        RightArrow = KeyCode.RightArrow,
        LeftArrow = KeyCode.LeftArrow,
    }

    public enum OtherKeyCode
    {
        None = KeyCode.None,

        Tab = KeyCode.Tab,
        CapsLock = KeyCode.CapsLock,
        LeftShift = KeyCode.LeftShift,
        LeftControl = KeyCode.LeftControl,
        LeftAlt = KeyCode.LeftAlt,

        Backspace = KeyCode.Backspace,
        Clear = KeyCode.Clear,
        Return = KeyCode.Return,
        RightShift = KeyCode.RightShift,
        RightControl = KeyCode.RightControl,
        RightAlt = KeyCode.RightAlt,

        LeftWindows = KeyCode.LeftWindows,
        RightWindows = KeyCode.RightWindows,

        LeftCommand = KeyCode.LeftCommand,
        RightApple = KeyCode.RightApple,
        AltGr = KeyCode.AltGr,
        Help = KeyCode.Help,
        SysReq = KeyCode.SysReq,
        Menu = KeyCode.Menu,
    }
}
