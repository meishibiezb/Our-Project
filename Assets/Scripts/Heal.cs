using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : MonoBehaviour, IEffect
{
    [SerializeField] int healAmount = 20; // 治疗的生命值

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
        int currentHealth = entity.GetHealth();
        entity.SetHealth(currentHealth + healAmount);
        Destroy(this.gameObject); // 应用效果后销毁该效果对象
    }
}
