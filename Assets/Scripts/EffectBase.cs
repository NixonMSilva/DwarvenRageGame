using System.Collections;
using UnityEngine;

public enum EffectType
{
    heal,
    berserk,
    fortune,
    fireResistance,
    poisonResistance,
}

public abstract class EffectBase
{
    public EffectBase () { }

    public abstract EffectType Type { get; }

    public abstract void ApplyEffect (StatusController target, Effect data);

    public abstract void StatusEnd (StatusController target);
}

public class Heal : EffectBase
{
    public Heal () { }
    public override EffectType Type => EffectType.heal;

    public override void ApplyEffect (StatusController target, Effect data)
    {
        target.Health += data.magnitude;
    }

    public override void StatusEnd (StatusController target)
    {

    }
}


public class Berserker : EffectBase
{
    public Berserker () { }

    public override EffectType Type => EffectType.berserk;

    public override void ApplyEffect (StatusController target, Effect data)
    {
        target.Attack.Berserk = true;
        target.Attack.TemporaryDamage = target.Attack.Damage * data.magnitude;
        target.WearStatus(this, data.duration);

        // If it's the player, double the speed as well
        if (target.gameObject.CompareTag("Player"))
        {
            target.Speed *= data.magnitude;
        }
    }

    public override void StatusEnd (StatusController target)
    {
        target.Attack.Berserk = false;
        target.Attack.TemporaryDamage = target.Attack.Damage;
        target.Speed = target.DefaultSpeed;
    }
}
public class FireResistance : EffectBase
{
    public FireResistance () { }

    public override EffectType Type => EffectType.fireResistance;

    public override void ApplyEffect (StatusController target, Effect data)
    {
        target.Health += data.duration;
    }

    public override void StatusEnd (StatusController target)
    {

    }
}

public class PoisonResistance : EffectBase
{
    public PoisonResistance () { }

    public override EffectType Type => EffectType.poisonResistance;

    public override void ApplyEffect (StatusController target, Effect data)
    {
        target.Health += data.duration;
    }

    public override void StatusEnd (StatusController target)
    {

    }
}

public class Fortune : EffectBase
{
    public Fortune () { }

    public override EffectType Type => EffectType.fortune;

    public override void ApplyEffect (StatusController target, Effect data)
    {
        target.Health += data.duration;
    }

    public override void StatusEnd (StatusController target)
    {

    }
}



