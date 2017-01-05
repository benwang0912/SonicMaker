using UnityEngine;

public static class GameConstants
{
    public enum SonicState
    {
        NORMAL,
        DEAD,
        JUMPING,
        SQUATTING,
        TOROLL,
        ROLLING,
        ONTOP,
        CURVEMOTION
    }

    public static string BGMPath = @"Caramel\Sound\BGM\";
    public static string SoundEffectPath = @"Caramel\Sound\SoundEffect\";

    public static AudioClip CoinSoundEffect = Resources.Load<AudioClip>(SoundEffectPath + "Coin");
    public static AudioClip SpringSoundEffect = Resources.Load<AudioClip>(SoundEffectPath + "Spring");
    public static AudioClip SpeedUpSoundEffect = Resources.Load<AudioClip>(SoundEffectPath + "SpeedUp");
    public static AudioClip AttackSoundEffect = Resources.Load<AudioClip>(SoundEffectPath + "Attack");
    public static AudioClip HurtSoundEffect = Resources.Load<AudioClip>(SoundEffectPath + "Hurt");
}
