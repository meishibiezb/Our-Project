using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectile
{
    GameObject GetCreatorObject()
    {
        return null;
    }
    IEntity GetCreatorEntity()
    {
        return null;
    }
    void SetCreator(IEntity entity)
    {
        return;
    }
    void SetDamege()
    {
        return;
    }
    int GetDamage()
    {
        return 0;
    }
    float GetEnergy()
    {
        return 0f;
    }
}
