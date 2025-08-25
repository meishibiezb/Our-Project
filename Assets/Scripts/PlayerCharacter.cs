using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

public class PlayerCharacter : MonoBehaviour, IPawn, IEntity
{
    Rigidbody2D rb;
    int health;
    bool isGrounded;
    bool isTowardsLeft;
    public GameObject triggeringObject;
    bool isClambering;
    GameObject wall;
    bool isAbsorbing; // �Ƿ�������״̬
    List<GameObject> abilityInstances;
    UnityEvent onDamaged;
    [SerializeField] bool enablePhysicalEffect = true; // �Ƿ���������Ч��
    [SerializeField] Vector2 sizeClamp = new Vector2(0.8f, 3f); // ��ɫ���ŷ�Χ
    [SerializeField] float jumpForce = 300f; // ��Ծ����
    [SerializeField] float moveSpeed = 5f; // �ƶ��ٶ�
    [SerializeField] int maxHealth = 100; // �������ֵ
    [SerializeField] float divisor = 200f; // ����ֵ�����ŵĳ���
    [SerializeField] GameObject[] abilities; // ���ܶ���

    // Start is called before the first frame update
    void Start()
    {

    }
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        health = maxHealth;
        tag = "Player";
        onDamaged = new UnityEvent();
        // ʵ��������
        if (abilities != null)
        {
            abilityInstances = new List<GameObject>();
            for (int i = 0; i < abilities.Length; i++)
            {
                AddAbility(abilities[i]);
            }
        }
        isTowardsLeft = false;
        isClambering = false;
        isAbsorbing = false;
    }

    // Update is called once per frame
    void Update()
    {
        SetPhysicalEffect();
        ScaleByHealth();
        UsePassiveAbilities();
    }

    //��ײ
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            // ������ǽ�����ײ�߼�
            var contacts = new ContactPoint2D[collision.contactCount];
            collision.GetContacts(contacts);
            foreach (var contact in contacts)
            {
                if (Mathf.Abs(contact.normal.y) < 0.4f)
                {
                    wall = collision.gameObject;
                    break;
                }

            }
        }

        // �����������������ײ�߼�
        if (collision.gameObject.GetComponent<IProjectile>() != null && CompareTag(collision.gameObject.tag))
        {
            return; // �����Լ����ӵ�
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

    // ����
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag(tag) && collision.isTrigger && collision.gameObject.GetComponent<LevelData>() == null)
        {
            triggeringObject = collision.gameObject;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag(tag) && collision.isTrigger && collision.gameObject.GetComponent<LevelData>() == null)
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

    //�ڲ��߼�
    private void Die()
    {
        Debug.Log($"{tag} has died.");
        rb.velocity = Vector2.zero; // ֹͣ��ɫ�ƶ�
        GetComponent<CapsuleCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false; // ���ؽ�ɫ
        Destroy(gameObject);
    }
    private void SetPhysicalEffect()
    {
        // ��������Ч��
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
    }
    private void ScaleByHealth()
    {
        // ��������ֵ����
        float healthRatio = 1f + ((float)(health - 100) / divisor);
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
    private void UsePassiveAbilities()
    {
        for (int abilityIndex = 0; abilityIndex < abilityInstances.Count; abilityIndex++)
        {
            if (abilityInstances != null && abilityIndex >= 0 && abilityIndex < abilityInstances.Count)
            {
                var abilityInstance = abilityInstances[abilityIndex].GetComponent<IAbility>();
                if (abilityInstance == null)
                {
                    continue;
                }
                if (abilityInstance.IsPassive())
                {
                    abilityInstance?.EffectBeforeExecute()?.ApplyEffect(GetComponent<IEntity>());
                    abilityInstance?.Activate(GetComponent<IEntity>());
                }
            }
        }
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

    //ʵ�ֽӿ�IPawn
    public void Jump()
    {
        if (rb != null && IsOnTheWall() && !isGrounded)
        {
            return;// ��ǽʱ������������Ծ
        }
        //��Ծ������ǽ��
        if (rb != null && isGrounded) // ����Ƿ��ڵ�����
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
        // ��ǽ���ҳ���ǽ��ʱ��������ǽ�ƶ�
        if (rb != null && IsOnTheWall() && (wall.transform.position.x - rb.position.x) * direction > 0 && !isClambering)
        {
            rb.velocity *= new Vector2(0.1f, 0.5f);
            //Debug.Log($"direction:{direction},wall.transform.position.x - rb.position.x:{wall.transform.position.x - rb.position.x}");

            return;// ��ǽʱ�����������ƶ�
        }
        // �ƶ�������ǽ��
        if (math.abs(rb.velocity.x) < moveSpeed)
        {
            rb.velocity = new Vector2(moveDirection.x * moveSpeed + rb.velocity.x, rb.velocity.y);
        }
        if (rb.velocity.x * moveDirection.x < 0)
        {
            rb.velocity = new Vector2(moveDirection.x * moveSpeed, rb.velocity.y);
        }
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

    // ʵ�ֽӿ�IEntity
    public int GetHealth()
    {
        return health;
    }
    public void SetHealth(int health)
    {
        if (health > maxHealth)
        {
            this.health = maxHealth;
        }
        else
        {
            this.health = health;
        }
        this.health = math.max(this.health, 0);
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
        SetHealth(health - damage);
        onDamaged?.Invoke();
        Debug.Log($"{tag} took {damage} damage, current health: {health}");
        if (health == 0)
        {
            // �����ɫ��������
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
        var ab = Instantiate(ability, transform);
        abilityInstances.Add(ab);
        ab.GetComponent<IAbility>()?.EffectOnAdd()?.ApplyEffect(GetComponent<IEntity>());
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
    public UnityEvent GetEventByName(string eventName)
    {
        if (eventName == "onDamaged")
        {
            Debug.Log("Get onDamaged event");
            return onDamaged;
        }
        Debug.LogError($"Event {eventName} not found in {tag}");
        return null;
    }
}
