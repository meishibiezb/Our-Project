using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPawn
{
    void Jump()
    {
        return;
    }
    void Clmaber(float direction)
    {
        return;
    }
    void Move(float direction)
    {
        return;
    }
    bool IsGrounded()
    {
        return true;
    }
    void UseAbility(int abilityIndex)
    {
        return;
    }
    bool IsOnTheWall()
    {
        return false;
    }
    bool IsClambering()
    {
        return false;
    }
    IEntity GetEntity()
    {
        return null;
    }
    GameObject GetGameObject()
    {
        return null;
    }
}