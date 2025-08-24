using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class BreakableBrick : MonoBehaviour, IEntity
{
    int health;
    float fadeTime;
    [SerializeField] int maxHealth = 100; // 最大生命值
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void Awake()
    {
        health = maxHealth; // 初始化生命值
        fadeTime = 5f;
        var rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 内部逻辑
    private void Die()
    {
        var rb = GetComponent<Rigidbody2D>();
        var cb = GetComponent<BoxCollider2D>();
        if (rb == null)
        {
            return;
        }
        Debug.Log($"{tag} has died.");
        rb.velocity = Vector2.zero; // 停止角色移动
        rb.constraints = RigidbodyConstraints2D.None;
        cb.enabled = false;
        StartCoroutine(WaitAndDo(fadeTime, () =>
        {
            Destroy(gameObject);
        }));
    }
    IEnumerator WaitAndDo(float seconds, System.Action action)
    {
        yield return new WaitForSeconds(seconds);
        action?.Invoke();
    }

    // 实现接口IEntity
    public GameObject GetGameObject()
    {
        return gameObject;
    }
    public void Damaged(int damage)
    {
        health -= damage;
        if (health < 0) health = 0;
        Debug.Log($"{tag} took {damage} damage, current health: {health}");
        if (health <= 0)
        {
            // 处理角色死亡逻辑
            Die();
        }
    }
    public bool IsDead()
    {
        return health <=0;
    }
}
