using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : MonoBehaviour, ITask
{
    [SerializeField] float chasingRadius = 10f;
    [SerializeField] Vector2 bias = new Vector2(0.6f, 0f);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 实现接口ITask
    public void Execute(IPawn pawn)
    {
        float debugDrawDuration = 0.08f;
        if (pawn == null || pawn.GetEntity().IsDead()) return;
        Vector2 rayOrigin = pawn.GetGameObject().transform.position;
        Vector2 rayDirection;
        if (pawn.GetEntity().IsTowardsLeft())
        {
            rayDirection = Vector2.left;
            bias = new Vector2(-bias.x, bias.y);
        }
        else
        {
            rayDirection = Vector2.right;
            bias = new Vector2(bias.x, bias.y);
        }
        Debug.DrawRay(rayOrigin + bias, rayDirection * chasingRadius, Color.red, debugDrawDuration);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin + bias, rayDirection, chasingRadius);
        Debug.Log($"{hit.collider != null},{hit.collider.gameObject.name},{hit.collider.CompareTag("Player")},{hit.collider.gameObject.GetComponent<IEntity>().IsCreature()}");
        if (hit.collider != null && hit.collider.CompareTag("Player") && hit.collider.gameObject.GetComponent<IEntity>().IsCreature())
        {
            Debug.Log($"{hit}");
            pawn.GetEntity().SetCertainStatus("isRunning", true);
            float direction = pawn.GetEntity().IsTowardsLeft() ? -1f : 1f;
            pawn.Move(direction);
        }
        else
        {
            pawn.GetEntity().SetCertainStatus("isRunning", false);
            pawn.Move(0f);
        }
    }
}
