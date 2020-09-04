using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionSettings : MonoBehaviour
{
    //darkmode
    //bullet type
    //bullet color
    public enum Colors
    {
        red,
        orange,
        yellow,
        green,
        cyan,
        blue,
        violet
    };

    public enum BulletType
    {
        triangle,
        circle,
        star
    };

    public enum UIMode
    {
        lightMode,
        darkMode
    };

    public static UIMode uiMode = UIMode.darkMode;
    public static BulletType bulletType = BulletType.circle;
    public static Colors color = Colors.yellow;
}
