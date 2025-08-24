using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Absorb : MonoBehaviour, IAbility
{
    string AbilityName = "Absorb";
    [SerializeField] float continualTime = 0.5f; // 持续时间，0表示瞬发
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // 内部逻辑
    IEnumerator WaitAndDo(float seconds, System.Action action)
    {
        yield return new WaitForSeconds(seconds);
        action?.Invoke();
    }

    // 实现接口IAbility
    public void Activate(IEntity speller)
    {
        if (speller == null)
        {
            Debug.LogError("speller is null");
            return;
        }
        var triggeringObj = speller.TriggeringObject();
        if (triggeringObj == null)
        {
            Debug.LogError("speller.TriggeringObject() is null");
            return;
        }
        var entity = triggeringObj.GetComponent<IEntity>();
        if (entity == null)
        {
            Debug.LogError("triggeringObj.GetComponent<IEntity>() is null");
            return;
        }
        if (entity.GetGameObject() == null)
        {
            Debug.LogError("entity.GetGameObject() is null");
            return;
        }
        if (!entity.GetGameObject().activeInHierarchy)
        {
            Debug.LogError("entity.GetGameObject() is not active in hierarchy");
            return;
        }
        if (entity != null && entity.GetGameObject().activeInHierarchy && entity.IsDead())
        {
            speller.SetCertainStatus("isAbsorbing", true);
            StartCoroutine(WaitAndDo(continualTime, () =>
            {
                var abilities = entity.GetAbilities();
                // 先吸收技能
                foreach (var ability in abilities)
                {
                    speller.AddAbility(ability);
                }
                // 再吸收效果
                var effects = entity.GetEffects();
                foreach (var effect in effects)
                {
                    effect.GetComponent<IEffect>()?.ApplyEffect(speller);
                }
                speller.SetCertainStatus("isAbsorbing", false);
                Destroy(entity.GetGameObject());
            }));
        }
    }
    public bool IsContinual()
    {
        return continualTime > 0;
    }
    public IEffect EffectBeforeExecute()
    {
        //return new EffectBeforeAbsorb();
        return null;
    }
    public string GetAbilityName()
    {
        return AbilityName;
    }
}

// 内部类实现IEffect
class EffectBeforeAbsorb : IEffect
{
    public void ApplyEffect(IEntity entity)
    {
        entity.SetCertainStatus("isAbsorbing", true);
    }

    public IEntity GetCreator()
    {
        // 可根据需要返回null或其他逻辑
        return null;
    }
}
