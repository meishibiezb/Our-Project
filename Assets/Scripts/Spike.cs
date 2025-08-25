using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour, IBrick
{
    private Collider2D collidingBox;
    [SerializeField] int spikeDamage = 100; // �����ɵ��˺�ֵ

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
            Debug.Log("Player collided with Spike");
            // ����������������˵��߼�
            var player = collision.gameObject.GetComponent<IEntity>();
            if (player != null)
            {
                player.Damaged(spikeDamage); // �������˺�
            }
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Enemy collided with Spike");
            var enemy = collision.gameObject.GetComponent<IEntity>();
            if (enemy != null)
            {
                enemy.Damaged(spikeDamage); // �������˺�
            }
        }
    }

    //ʵ�ֽӿ�IBrick
    public void StepOn(GameObject obj)
    {
        if (obj.CompareTag("Player"))
        {
            Debug.Log("Player stepped on Spike");
            // �����������Ҳ���ש���ϵ��߼�
        }
    }
}
