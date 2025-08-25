using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEffect
{
    void ApplyEffect(IEntity entity)
    {
        return;
    }
    IEntity GetCreator()
    {
        return null;
    }
}