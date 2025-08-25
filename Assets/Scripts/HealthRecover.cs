using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class HealthRecover : MonoBehaviour, IAbility
{
    public bool isInBattle;
    float getRidOfBattleTimer;
    public float recoverTimer;
    [SerializeField] float getRidOfBattleThreshold = 5f;
    [SerializeField] float cooldownThreshold = 1f; // ��ȴʱ��
    [SerializeField] int recoverAmount = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        isInBattle = false;
        getRidOfBattleTimer = 0f;
        recoverTimer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isInBattle)
        {
            getRidOfBattleTimer += Time.deltaTime;
        }
        else
        {
            recoverTimer += Time.deltaTime;
        }
        if (getRidOfBattleTimer >= getRidOfBattleThreshold)
        {
            isInBattle = false;
            getRidOfBattleTimer = 0f;
        }
    }

    // �ڲ��߼�
    private void OnDamaged()
    {
        Debug.Log("HealthRecover detected OnDamaged event");
        isInBattle =true;
        getRidOfBattleTimer=0f;
    }

    // ʵ�ֽӿ�IAbility
    public IEffect EffectOnAdd()
    {
        return new EffectOnAddHealthRecover(this);
    }
    public void Activate(IEntity speller)
    {
        if (!isInBattle && recoverTimer >= cooldownThreshold)
        {
            speller.SetHealth(speller.GetHealth() + recoverAmount);
            recoverTimer = 0f;
        }
    }
    public bool IsPassive()
    {
        return true;
    }

    // �ڲ���ʵ��IEffect
    class EffectOnAddHealthRecover : IEffect
    {
        HealthRecover hr;
        public EffectOnAddHealthRecover(HealthRecover hr)
        {
            this.hr = hr;
        }
        public void ApplyEffect(IEntity entity)
        {
            var ue = entity.GetEventByName("onDamaged");
            if (ue == null)
            {
                Debug.LogError("HealthRecover: onDamaged event not found on entity");
                return;
            }
            ue.AddListener(hr.OnDamaged);
        }

        public IEntity GetCreator()
        {
            // �ɸ�����Ҫ����null�������߼�
            return null;
        }
    }
}
