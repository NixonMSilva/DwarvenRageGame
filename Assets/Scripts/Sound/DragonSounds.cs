using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonSounds : MonoBehaviour
{
    public void ScreamSound ()
    {
        AudioManager.instance.PlaySoundAt(gameObject, "dragon_scream");
    }

    public void BiteSound ()
    {
        AudioManager.instance.PlaySoundAt(gameObject, "dragon_bite");
    }
    
    public void ClawSound ()
    {
        AudioManager.instance.PlaySoundRandomAt(gameObject, "axe_generic");
    }

    public void BreathSound ()
    {
        AudioManager.instance.PlaySound("dragon_flame_attack");
    }
}
