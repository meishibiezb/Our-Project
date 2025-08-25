using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandMaxHealth : MonoBehaviour, IEffect
{
    [SerializeField] int healthIncreaseAmount = 20; // ���ӵ��������ֵ
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // ʵ�ֽӿ�IEffect
    public void ApplyEffect(IEntity entity)
    {
        int currentMaxHealth = entity.GetMaxHealth();
        entity.SetMaxHealth(currentMaxHealth + healthIncreaseAmount); // ����20���������ֵ
        entity.SetHealth(entity.GetHealth() + healthIncreaseAmount); // ͬʱ���ӵ�ǰ����ֵ��ƥ���µ����ֵ
        Destroy(this.gameObject); // Ӧ��Ч�������ٸ�Ч������
    }
}
