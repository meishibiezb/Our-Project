using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IPawn, IEntity
{
    Rigidbody2D rb;
    int health;
    bool isGrounded;
    CapsuleCollider2D bd;
    BoxCollider2D tg;
    [SerializeField] float jumpForce = 300f; // 跳跃力度
    [SerializeField] float moveSpeed = 5f; // 移动速度
    [SerializeField] int maxHealth = 100; // 最大生命值
    [SerializeField] GameObject[] abilities; // 技能对象
    [SerializeField] GameObject[] effectAppliedOnAbsorb; // 吸收时应用的效果

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        tg = GetComponent<BoxCollider2D>();
        bd = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        health = maxHealth; // 初始化生命值
        tag = "Enemy"; // 设置标签为 Enemy
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 碰撞
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
        {
            var player = collision.gameObject.GetComponent<IEntity>();
            if ((player != null) && GetComponent<IEntity>().IsDead())
            {
                Physics2D.IgnoreCollision(bd, collision.collider);
            }
        }
    }

    // 触发
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var player = collision.gameObject.GetComponent<IEntity>();
            if (player != null)
            {
                Debug.Log("Enemy triggered by Player");
            }
        }
    }

    // 内部逻辑
    private void Die()
    {
        if (rb == null)
        {
            return;
        }
        Debug.Log($"{tag} has died.");
        rb.velocity = Vector2.zero; // 停止角色移动
        rb.constraints = RigidbodyConstraints2D.None;
    }

    // 实现接口IPawn
    public void Move(float direction)
    {
        Vector2 movement = new Vector2(direction * moveSpeed, rb.velocity.y);
        rb.velocity = movement; // 设置刚体的速度
    }

    // 实现接口IEntity
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
        return health <= 0;
    }
    public GameObject GetGameObject()
    {
        return gameObject;
    }
    public GameObject[] GetAbilities()
    {
        return abilities;
    }
    public bool IsCreature()
    {
        return true;
    }
    public GameObject[] GetEffects()
    {
        return effectAppliedOnAbsorb;
    }
}
