using UnityEngine;

public static class Game
{
    public static GameConstants.SonicState sonicstate;
    public static Vector3 velocity;
    public static Transform sonic, rollingball;
    public static UILabel coinslabel, timelabel;
    public static int coins = 0;
    public static float time = 0f;
}
