using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandMaxHealth : MonoBehaviour, IEffect
{
    [SerializeField] int healthIncreaseAmount = 20; // 增加的最大生命值
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 实现接口IEffect
    public void ApplyEffect(IEntity entity)
    {
        int currentMaxHealth = entity.GetMaxHealth();
        entity.SetMaxHealth(currentMaxHealth + healthIncreaseAmount); // 增加20点最大生命值
        entity.SetHealth(entity.GetHealth() + healthIncreaseAmount); // 同时增加当前生命值以匹配新的最大值
        Destroy(this.gameObject); // 应用效果后销毁该效果对象
    }
}
