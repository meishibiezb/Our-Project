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
    bool isTowardsLeft;
    bool isRunning;
    float hitTimer;
    [SerializeField] float jumpForce = 150f; // 跳跃力度
    [SerializeField] float moveSpeed = 5f; // 移动速度
    [SerializeField] int maxHealth = 100; // 最大生命值
    [SerializeField] bool hitCanDamage = true;
    [SerializeField] int hitDamage = 10; // 碰撞伤害
    [SerializeField] float hitCooldown = 1f; // 碰撞冷却时间
    [SerializeField] Vector2 hitForece = new Vector2(500f, 300f); // 碰撞时的力
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
        isTowardsLeft = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        hitTimer += Time.deltaTime;
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
        if (collision.gameObject.CompareTag("Player") && hitCanDamage && hitTimer >= hitCooldown)
        {
            var entity = collision.gameObject.GetComponent<IEntity>();
            entity.Damaged(hitDamage);
            entity.GetGameObject().GetComponent<Rigidbody2D>().velocity += rb.velocity;
            entity.GetGameObject().GetComponent<Rigidbody2D>().AddForce(new Vector2(hitForece.x, hitForece.y));
            //entity.GetGameObject().GetComponent<IPawn>()?.Jump();
            hitTimer = 0f;
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
        direction = Mathf.Clamp(direction, -1f, 1f);
        Vector2 movement = new Vector2(direction * moveSpeed, rb.velocity.y);
        if (isRunning)
        {
            movement *= 1.5f;
        }
        rb.velocity = movement;
        Debug.Log($"{rb.velocity}");
    }
    public void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(new Vector2(0, jumpForce));
            isGrounded = false; // 跳跃后设置为非地面状态
        }
    }
    public GameObject GetGameObject()
    {
        return gameObject;
    }
    public IEntity GetEntity()
    {
        return GetComponent<IEntity>();
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
    public void SetCertainStatus(string status, bool value)
    {
        if (status == "isRunning")
        {
            isRunning = value;
        }
    }
    public bool IsTowardsLeft()
    {
        return isTowardsLeft;
    }
}
