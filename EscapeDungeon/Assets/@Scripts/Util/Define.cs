using UnityEngine;

public static class Define
{
    public enum EObjectType
    {
        None,
        Creature,
        Projectile,
        Env,
        Effect,
    }

    public enum EGameState 
    {
        None,
        Room1,
        Room1Clear,
        Room2,
        Room2Clear,

    }

    public enum ECreatureType
    {
        None,
        Hero,
        Monster,
    }

    public enum ECreatureState
    {
        None,
        Idle,
        Move,
        Dash,
        Skill,
        OnDamaged,
        Dead
    }

    public enum UserAction
    {
        MoveForward,
        MoveBackward,
        MoveLeft,
        MoveRight,

        Attack,
        Dash,
        Postion,

        UI_Status
    }
}
