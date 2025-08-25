using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFlag : MonoBehaviour
{
    [SerializeField] GameObject player; // ��Ҷ���
    [SerializeField] Vector2 bias; // ƫ����

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // ��ⰴ��P��
        if (Input.GetKeyDown(KeyCode.P))
        {
            Spawn();
        }
    }

    //�ڲ��߼�
    public void Spawn()
    {
        Instantiate(player, transform.position + (new Vector3(bias.x, bias.y, 0f)), Quaternion.identity);
    }
}
