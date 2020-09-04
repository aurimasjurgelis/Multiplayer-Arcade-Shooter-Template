using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public static class DatabaseManager
{
    public static string username;
    public static int score;

    public static bool LoggedIn { get { return username != null; } }

    public static void LogOut()
    {
        username = null;
    }

    private static System.Random random = new System.Random();
    public static string SecurityCode(int length = 6)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        return new string(Enumerable.Repeat(chars, length)
          .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
