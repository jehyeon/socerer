using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DataType
{
    Int, Float, Enum
}
public enum SkillElemental
{
    Fire, Ice
}

public enum SkillType
{
    Projectile, Area
}

public enum SkillAreaPivot
{
    Player, Ground
}

public enum SkillAreaForm
{
    Circle, Box, Fan, StraightProj
}

public enum LayerEnum
{
    Null = 0,
    Player = 7
}

public enum LayerMarskEnum
{
    Player = 1 << 7
}


public enum SkillEffectType
{
    Damage, Stun, Slow, CallSkill, Teleport, DamageOverTime
}