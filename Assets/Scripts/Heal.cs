using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : MonoBehaviour, IEffect
{
    [SerializeField] int healAmount = 20; // ���Ƶ�����ֵ

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
        int currentHealth = entity.GetHealth();
        entity.SetHealth(currentHealth + healAmount);
        Destroy(this.gameObject); // Ӧ��Ч�������ٸ�Ч������
    }
}
