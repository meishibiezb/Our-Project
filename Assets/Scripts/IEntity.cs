using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IEntity
{
    int GetHealth()
    {
        return 0;
    }
    void SetHealth(int health)
    {
        return;
    }
    int GetMaxHealth()
    {
        return 0;
    }
    void SetMaxHealth(int maxHealth)
    {
        return;
    }
    void Damaged(int damage)
    {
        return;
    }
    string GetName()
    {
        return "";
    }
    bool IsDead()
    {
        return false;
    }
    GameObject GetGameObject();
    bool IsTowardsLeft()
    {
        return false;
    }
    GameObject TriggeringObject()
    {
        return null;
    }
    GameObject[] GetAbilities()
    {
        return null;
    }
    GameObject[] GetEffects()
    {
        return null;
    }
    void AddAbility(GameObject ability)
    {
        return;
    }
    bool IsCreature()
    {
        return false;
    }
    bool GetCertainStatus(string status)
    {
        return false;
    }
    void SetCertainStatus(string status, bool value)
    {
        return;
    }
    UnityEvent GetEventByName(string name)
    {
        return null;
    }
}