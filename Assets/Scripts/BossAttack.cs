using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class BossAttack : MonoBehaviour, ITask
{
    Vector2 bias;
    float cooldownTimer = 0f;
    [SerializeField] float dectectRadius = 6f;
    [SerializeField] float cooldownThreshold;
    [SerializeField] float continualTime = 0.5f; // 持续时间，0表示瞬发

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        bias = new Vector2(0.6f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        cooldownTimer += Time.deltaTime;
    }

    // 内部逻辑
    IEnumerator WaitAndDo(float seconds, System.Action action)
    {
        yield return new WaitForSeconds(seconds);
        action?.Invoke();
    }

    void Attack(IPawn pawn)
    {
        if (cooldownTimer < cooldownThreshold)
        {
            cooldownTimer = 0f;
            return;
        }
        pawn.GetEntity().SetCertainStatus("isAttacking", true);
        StartCoroutine(WaitAndDo(continualTime, () =>
        {
            pawn.UseAbility(0);
            pawn.GetEntity().SetCertainStatus("isAttacking", false);
        }));
    }

    // 实现接口ITask
    public void Execute(IPawn pawn)
    {
        float debugDrawDuration = 0.08f;
        if (pawn == null || pawn.GetEntity().IsDead()) return;
        Vector2 rayOrigin = pawn.GetGameObject().transform.position;
        Vector2 rayDirection;
        // 先检查3次正面
        for (int i = 0; i < 3; i++)
        {
            if (pawn.GetEntity().IsTowardsLeft())
            {
                rayDirection = Vector2.left;
                bias = new Vector2(-bias.x, bias.y - (0.3f * i));
            }
            else
            {
                rayDirection = Vector2.right;
                bias = new Vector2(bias.x, bias.y - (0.3f * i));
            }
            Debug.DrawRay(rayOrigin + bias, rayDirection * dectectRadius, Color.green, debugDrawDuration);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin + bias, rayDirection, dectectRadius);
            if (hit.collider != null && hit.collider.CompareTag("Player") && hit.collider.gameObject.GetComponent<IEntity>().IsCreature())
            {
                //Debug.Log("Boss Attack!");
                Attack(pawn);
                break;
            }
        }
        bias = new Vector2(0.6f, 1f);
        // 再检查3次背面
        for (int i = 0; i < 3; i++)
        {
            if (!pawn.GetEntity().IsTowardsLeft())
            {
                rayDirection = Vector2.left;
                bias = new Vector2(-bias.x, bias.y - (0.45f * i));
            }
            else
            {
                rayDirection = Vector2.right;
                bias = new Vector2(bias.x, bias.y - (0.45f * i));
            }
            Debug.DrawRay(rayOrigin + bias, rayDirection * dectectRadius, Color.green, debugDrawDuration);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin + bias, rayDirection, dectectRadius);
            if (hit.collider != null && hit.collider.CompareTag("Player") && hit.collider.gameObject.GetComponent<IEntity>().IsCreature())
            {
                //Debug.Log("Boss Attack!");
                pawn.GetEntity().SetCertainStatus("isTowardsLeft", !pawn.GetEntity().IsTowardsLeft());
                Attack(pawn);
                break;
            }
        }
        bias = new Vector2(0.6f, 1f);
    }
}
