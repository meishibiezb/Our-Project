using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultItem : MonoBehaviour, IEntity
{
    [SerializeField] GameObject[] abilities; // ���ܶ���
    [SerializeField] GameObject[] effectAppliedOnAbsorb; // ����ʱӦ�õ�Ч��
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // ʵ�ֽӿ�IEntity
    public GameObject GetGameObject()
    {
        return gameObject;
    }
    public bool IsDead()
    {
        return true;
    }
    public GameObject[] GetAbilities()
    {
        return abilities;
    }
    public bool IsCreature()
    {
        return false;
    }
    public GameObject[] GetEffects()
    {
        return effectAppliedOnAbsorb;
    }
}
