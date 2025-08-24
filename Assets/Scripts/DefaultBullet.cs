using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class DefaultBullet : MonoBehaviour, IEntity, IProjectile
{
    Rigidbody2D rb;
    float liftTimer = 0;
    IEntity creator;
    [SerializeField] int damage = 10;
    [SerializeField] bool removeOnHit = true;
    [SerializeField] float liftDuriation = 5;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        liftTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        liftTimer += Time.deltaTime;
        if (liftTimer > liftDuriation && liftDuriation > 0f)
        {
            Destroy(gameObject);
        }
    }

    // 碰撞
    private void OnCollisionEnter2D(Collision2D collision)
    {
        bool b = false;
        if (collision.gameObject.GetComponent<IEntity>() != null)
        {
            b = collision.gameObject.GetComponent<IEntity>().IsCreature();
        }
        if (!collision.gameObject.CompareTag(tag) && b)
        {
            var target = collision.gameObject.GetComponent<IEntity>();
            float relativeVelocity = collision.relativeVelocity.magnitude;
            float energy = math.pow(relativeVelocity, 2.0f) * rb.mass * 0.5f;
            if (target != null && energy > 15 && !target.IsDead())
            {
                target.Damaged(damage); // 子弹造成伤害
                if (removeOnHit)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    // 实现接口IEntity
    public GameObject GetGameObject()
    {
        return gameObject;
    }
    public bool IsCreature()
    {
        return false;
    }

    // 实现接口IProjectile
    public void SetCreator(IEntity creator)
    {
        this.creator = creator;
    }
    public IEntity GetCreator()
    {
        return creator;
    }
    public GameObject GetCreatorObject()
    {
        return creator?.GetGameObject();
    }
    public int GetDamage()
    {
        return damage;
    }
    public void SetDamage(int damage)
    {
        this.damage = damage;
    }
    public float GetEnergy()
    {
        return 0.5f * rb.mass * math.pow(rb.velocity.magnitude, 2.0f);
    }
}
