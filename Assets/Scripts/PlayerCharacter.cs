using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour, IPawn, IEntity
{
    Rigidbody2D rb;
    int health;
    bool isGrounded;
    bool isTowardsLeft;
    GameObject triggeringObject;
    bool isClambering;
    GameObject wall;
    bool isAbsorbing; // 是否处于吸收状态
    List<GameObject> abilityInstances;
    [SerializeField] bool enablePhysicalEffect = true; // 是否启用物理效果
    [SerializeField] Vector2 sizeClamp = new Vector2(0.8f, 3f); // 角色缩放范围
    [SerializeField] float jumpForce = 300f; // 跳跃力度
    [SerializeField] float moveSpeed = 5f; // 移动速度
    [SerializeField] int maxHealth = 100; // 最大生命值
    [SerializeField] GameObject[] abilities; // 技能对象

    // Start is called before the first frame update
    void Start()
    {

    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        health = maxHealth;
        tag = "Player";
        // 实例化技能
        if (abilities != null)
        {
            abilityInstances = new List<GameObject>();
            for (int i = 0; i < abilities.Length; i++)
            {
                abilityInstances.Add(Instantiate(abilities[i], transform));
            }
        }
        isTowardsLeft = false;
        isClambering = false;
        isAbsorbing = false;
    }

    // Update is called once per frame
    void Update()
    {
        // 设置物理效果
        isClambering = IsOnTheWall() && rb.velocity.y > 0;
        if (isGrounded || isClambering)
        {
            rb.rotation = 0f;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        else if (enablePhysicalEffect)
        {
            rb.constraints = RigidbodyConstraints2D.None;
        }

        // 根据生命值缩放
        float healthRatio = 1f + ((float)(health - 100) / 200f);
        healthRatio = Mathf.Clamp(healthRatio, sizeClamp.x, sizeClamp.y);
        if (IsTowardsLeft())
        {
            transform.localScale = new Vector3(-healthRatio, healthRatio, 1f);
        }
        else
        {
            transform.localScale = new Vector3(healthRatio, healthRatio, 1f);
        }
    }

    //碰撞
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            // 处理与墙体的碰撞逻辑
            var contacts = new ContactPoint2D[collision.contactCount];
            collision.GetContacts(contacts);
            foreach (var contact in contacts)
            {
                if (Mathf.Abs(contact.normal.y) < 0.1f)
                {
                    wall = collision.gameObject;
                    break;
                }

            }
        }

        // 处理与其他物体的碰撞逻辑
        if (collision.gameObject.GetComponent<IProjectile>() != null && CompareTag(collision.gameObject.tag))
        {
            return; // 不踩自己的子弹
        }
        var contactPoints = new ContactPoint2D[collision.contactCount];
        collision.GetContacts(contactPoints);
        foreach (var contact in contactPoints)
        {
            if (Mathf.Abs(contact.normal.x) < 0.5f)
            {
                rb.rotation = 0f;
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                isGrounded = true;
                break;
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject == wall)
        {
            wall = null;
        }
    }

    // 触发
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag(tag))
        {
            triggeringObject = collision.gameObject;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (triggeringObject == collision.gameObject)
        {
            triggeringObject = null;
        }
    }

    //内部逻辑
    private void Die()
    {
        Debug.Log($"{tag} has died.");
        rb.velocity = Vector2.zero; // 停止角色移动
        GetComponent<CapsuleCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false; // 隐藏角色
        Destroy(gameObject);
    }
    private bool FindPassiveAbility(string target)
    {
        foreach (var ability in abilityInstances)
        {
            var abilityInstance = ability.GetComponent<IAbility>();
            if (!abilityInstance.IsPassive())
            {
                continue;
            }
            if (abilityInstance.GetAbilityName() == target)
            {
                return true;
            }
        }
        return false;
    }

    //实现接口IPawn
    public void Jump()
    {
        if (rb != null && IsOnTheWall() && !isGrounded)
        {
            return;// 贴墙时不进行正常跳跃
        }
        //跳跃（不贴墙）
        if (rb != null && isGrounded) // 检查是否在地面上
        {
            rb.AddForce(new Vector2(0, jumpForce));
            isGrounded = false;
        }
    }
    public void Clmaber(float direction)
    {
        if (direction > 0 && wall != null && FindPassiveAbility("Clamber"))
        {
            rb.velocity = new Vector2(0f, moveSpeed * 0.45f);
            isGrounded= false;
        }
    }
    public void Move(float direction)
    {
        Vector2 moveDirection = new Vector2(direction, 0);
        // 贴墙并且朝向墙壁时，进行贴墙移动
        if (rb != null && IsOnTheWall() && (wall.transform.position.x - rb.position.x) * direction > 0 && !isClambering)
        {
            rb.velocity *= new Vector2(0.1f, 0.5f);
            //Debug.Log($"direction:{direction},wall.transform.position.x - rb.position.x:{wall.transform.position.x - rb.position.x}");

            return;// 贴墙时不进行正常移动
        }
        // 移动（不贴墙）
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, rb.velocity.y);
        if (direction < 0)
        {
            isTowardsLeft = true;
        }
        else if (direction > 0)
        {
            isTowardsLeft = false;
        }
        if (rb.constraints == RigidbodyConstraints2D.None)
        {
            if (math.abs(rb.angularVelocity) <= 360f)
            {
                rb.AddTorque(-direction * 180f);
            }
        }
    }
    public bool IsGrounded()
    {
        return isGrounded;
    }
    public void UseAbility(int abilityIndex)
    {
        if (abilityInstances != null && abilityIndex >= 0 && abilityIndex < abilityInstances.Count)
        {
            var abilityInstance = abilityInstances[abilityIndex].GetComponent<IAbility>();
            if (abilityInstance == null)
            {
                return;
            }
            if (abilityInstance.IsPassive())
            {
                UseAbility(abilityIndex + 1);
                return;
            }
            abilityInstance?.EffectBeforeExecute()?.ApplyEffect(GetComponent<IEntity>());
            abilityInstance?.Activate(GetComponent<IEntity>());
        }
    }
    public bool IsOnTheWall()
    {
        return wall != null;
    }
    public bool IsClambering()
    {
        return isClambering;
    }

    // 实现接口IEntity
    public int GetHealth()
    {
        return health;
    }
    public void SetHealth(int health)
    {
        if (this.health > maxHealth)
        {
            this.health = maxHealth;
                }
        else
        {
            this.health = health;
        }
    }
    public int GetMaxHealth()
    {
        return maxHealth;
    }
    public void SetMaxHealth(int maxHealth)
    {
        this.maxHealth = maxHealth;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }
    public string GetName()
    {
        return tag;
    }
    public bool IsDead()
    {
        return health <= 0;
    }
    public void Damaged(int damage)
    {
        health -= damage;
        if (health < 0) health = 0;
        Debug.Log($"{tag} took {damage} damage, current health: {health}");
        if (health == 0)
        {
            // 处理角色死亡流程
            Die();
        }
    }
    public GameObject GetGameObject()
    {
        return gameObject;
    }
    public bool IsTowardsLeft()
    {
        return isTowardsLeft;
    }
    public GameObject TriggeringObject()
    {
        return triggeringObject;
    }
    public void AddAbility(GameObject ability)
    {
        abilityInstances ??= new List<GameObject>();
        abilityInstances.Add(Instantiate(ability, transform));
    }
    public bool IsCreature()
    {
        return true;
    }
    public bool GetCertainStatus(string status)
    {
        if (status == "isAbsorbing")
        {
            return isAbsorbing;
        }
        if (status == "isClambering")
        {
            return isClambering;
        }
        if (status == "isGrounded")
        {
            return isGrounded;
        }
        return false;
    }
    public void SetCertainStatus(string status, bool value)
    {
        if (status == "isAbsorbing")
        {
            isAbsorbing = value;
            return;
        }
        if (status == "isClambering")
        {
            isClambering = value;
            return;
        }
        if (status == "isGrounded")
        {
            isGrounded = value;
            return;
        }
        return;
    }
}
