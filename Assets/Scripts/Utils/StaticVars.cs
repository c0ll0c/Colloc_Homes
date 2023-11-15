using UnityEngine;

public static class StaticVars
{
    public static string PREFS_NICKNAE = "nickname";
    public static byte MAX_PLAYERS_PER_ROOM = 6;

    // time variables
    public static double GAME_TIME = 300.0f; // 5m
    public static double VACCINE_DROP_INTERVAL = 60.0f; // 1m
    public static float DETOX_TIME = 15.0f; // 15s
    public static float ATTACK_TIME = 15.0f; // 15s

    // homes spawn position
    public static Vector3[] SpawnPosition =
    {
        new Vector3(-9.0f, -6.0f, 0),
        new Vector3(-3.0f, 15.0f, 0),
        new Vector3(14.5f, 1.5f, 0),
        new Vector3(9.0f, 12.5f, 0),
        new Vector3(7.0f, 6.0f, 0),
        new Vector3(-2.0f, 0, 0),
    };

    public static Vector2[] CluePosition = {
        new Vector2(4.0f, 4.0f),
        new Vector2(-0.2f, -3.0f),
        new Vector2(4.0f, -10.0f),
        new Vector2(15.0f, 4.0f),
        new Vector2(-2.5f, 16.0f),
        new Vector2(-3.4f, -3.0f),
        new Vector2(-1.2f, 4.0f),
        new Vector2(12.0f, -9.0f),
        new Vector2(-8.4f, -6.0f),
        new Vector2(7.0f, -1.1f),
        new Vector2(1.8f, 1.3f),
        new Vector2(-6.0f, 8.0f),
        new Vector2(-4.0f, 6.0f),
        new Vector2(-4.5f, -4.6f),
        new Vector2(9.3f, 7.2f),
        new Vector2(13.3f, 4.4f),
        new Vector2(6.2f, 18.4f),
        new Vector2(-7.1f, -3.1f),
        new Vector2(5.0f, 7.7f),
    };
}
