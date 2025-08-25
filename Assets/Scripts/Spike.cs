using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour, IBrick
{
    private Collider2D collidingBox;
    [SerializeField] int spikeDamage = 100; // 尖刺造成的伤害值

    // Start is called before the first frame update
    void Start()
    {

    }

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        collidingBox = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //碰撞
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 处理与玩家的碰撞逻辑
            Debug.Log("Player collided with Spike");
            // 这里可以添加玩家受伤的逻辑
            var player = collision.gameObject.GetComponent<IEntity>();
            if (player != null)
            {
                player.Damaged(spikeDamage); // 尖刺造成伤害
            }
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Enemy collided with Spike");
            var enemy = collision.gameObject.GetComponent<IEntity>();
            if (enemy != null)
            {
                enemy.Damaged(spikeDamage); // 尖刺造成伤害
            }
        }
    }

    //实现接口IBrick
    public void StepOn(GameObject obj)
    {
        if (obj.CompareTag("Player"))
        {
            Debug.Log("Player stepped on Spike");
            // 在这里添加玩家踩在砖块上的逻辑
        }
    }
}
