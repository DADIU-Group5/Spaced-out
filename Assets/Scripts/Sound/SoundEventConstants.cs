using UnityEngine;
using System.Collections;

public class SoundEventConstants
{
    // Ambient hazard sounds
    public const string EXPLOSIVE = "explosive";                           // works but we need this to be broken down in two
    public const string ELECTRICITY = "electricity";
    public const string GAS = "gas";
    public const string DOOR_OPEN = "doorOpen";
    public const string DOOR_SHUT = "doorShut";

    // Dave sounds
    public const string DAVE_CHARGE = "daveCharge";                        // v
    public const string DAVE_LAUNCH = "daveLaunch";                        // v
    public const string DAVE_FIRST_LAUNCH = "daveFirstLaunch";             // v
    public const string DAVE_FIRST_LAUNCH_1 = "daveFirstLaunch_01";        // v
    public const string DAVE_FIRST_LAUNCH_2 = "daveFirstLaunch_02";        // v
    public const string DAVE_FIRST_LAUNCH_3 = "daveFirstLaunch_03";
    public const string DAVE_FIRST_LAUNCH_4 = "daveFirstLaunch_04";
    public const string DAVE_FIRST_LAUNCH_5 = "daveFirstLaunch_05";
    public const string DAVE_OBJECT_COLLISION = "daveobjectCollision";
    public const string DAVE_STATIC_COLLISION = "daveStaticCollision";
    public const string DAVE_RANDOM_GRUNT = "daveRandomGrunt";
    public const string DAVE_VENT = "daveVent";

    // Dave hazard sounds
    public const string DAVE_CATCH_FIRE = "daveCatchFire";
    public const string DAVE_ELECTROCUTE = "daveElectrocute";
    public const string DAVE_FIRE_WOOSH = "daveFireWoosh";
    public const string DAVE_PUT_OUT_FIRE = "davePutOutFire";
    public const string DAVE_OUT_OF_OXYGEN = "daveOutofOxygen";

    // GAL sounds
    public const string GAL_DAVE_ON_FIRE = "galDaveOnFire";                // doesn't exist
    public const string GAL_DEATH_ELECTROCUTED = "galDeathElectrocuted";   // doesn't exist
    public const string GAL_HAZARDS_EXPLOSION = "galHazardsExplosion";     // doesn't exist
    public const string GAL_RANDOM_INSULT = "galRandomInsult";             // doesn't exist
    public const string GAL_RANDOM_NARATIVE = "galRandomNarative";         // v

    // Scene sounds
    public const string MUSIC = "musicPlay";                               // v
    public const string MUSIC_MAIN_PLAY = "musicMainPlay";
    public const string MUSIC_MAIN_STOP = "musicMainStop";
    public const string GAME_START = "gameStart";                          // v
}
