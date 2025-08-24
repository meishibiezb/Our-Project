using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAbility
{
    void Activate()
    {
        return;
    }
    void Activate(IEntity speller)
    {
        return;
    }
    float GetCooldown()
    {
        return 0f;
    }
    bool IsContinual()
    {
        return false;
    }
    IEffect EffectBeforeExecute()
    {
        return null;
    }
    bool IsPassive()
    {
        return false;
    }
}