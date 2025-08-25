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

    //��ײ
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // ��������ҵ���ײ�߼�
            
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // ��������˵���ײ�߼�
            
        }
    }

    //ʵ�ֽӿ�IBrick
    public void StepOn(GameObject obj)
    {
        if (obj.CompareTag("Player"))
        {
            Debug.Log("Player stepped on DefaultBrick");
            // �����������Ҳ���ש���ϵ��߼�
        }
    }
}
