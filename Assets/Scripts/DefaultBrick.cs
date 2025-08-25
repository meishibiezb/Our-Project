using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultBrick : MonoBehaviour, IBrick
{
    private Collider2D collidingBox;

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
            
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // 处理与敌人的碰撞逻辑
            
        }
    }

    //实现接口IBrick
    public void StepOn(GameObject obj)
    {
        if (obj.CompareTag("Player"))
        {
            Debug.Log("Player stepped on DefaultBrick");
            // 在这里添加玩家踩在砖块上的逻辑
        }
    }
}
