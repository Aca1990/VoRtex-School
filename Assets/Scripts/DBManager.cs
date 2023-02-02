﻿using System.Collections.Generic;
using UnityEngine;

public static class DBManager
{
    public static int id;
    public static string username;
    public static HashSet<string> achievements;
    public static string face_recognition_image_location;
    public static int role_id;
    public static MicroLesson microLesson;
    public static string avatar_type_id;
    public static string blockchain_address;

    static DBManager()
    {
        microLesson = new MicroLesson();
    }

    public static bool LoggedIn { get { return username != null; } }

    public static void LogOut()
    {
        username = null;
        Debug.Log("User logged out");
    }
}
