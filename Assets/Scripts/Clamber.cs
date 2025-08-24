using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clamber : MonoBehaviour, IAbility
{
    string AbilityName = "Clamber";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 实现接口IAbility
    public bool IsPassive()
    {
        return true;
    }
    public string GetAbilityName()
    {
        return AbilityName;
    }
}
