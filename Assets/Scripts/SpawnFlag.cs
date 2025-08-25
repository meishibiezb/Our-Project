using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFlag : MonoBehaviour
{
    [SerializeField] GameObject player; // 玩家对象
    [SerializeField] Vector2 bias; // 偏移量

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 检测按下P键
        if (Input.GetKeyDown(KeyCode.P))
        {
            Spawn();
        }
    }

    //内部逻辑
    public void Spawn()
    {
        Instantiate(player, transform.position + (new Vector3(bias.x, bias.y, 0f)), Quaternion.identity);
    }
}
