using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour, ITask
{
    Vector2 bias;
    float groundDetectRadius = 1.5f;
    [SerializeField] LayerMask lm;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 实现接口ITask
    public void Execute(IPawn pawn)
    {
        if (pawn.GetEntity().GetCertainStatus("isRunning"))
        {
            //Debug.Log("is running, stop patrol");
            return;
        }
        float debugDrawDuration = 0.08f;
        if (pawn == null || pawn.GetEntity().IsDead()) return;
        Vector2 rayOrigin = pawn.GetGameObject().transform.position;
        Vector2 rayDirection = Vector2.down;
        if (pawn.GetEntity().IsTowardsLeft())
        {
            bias = new Vector2(-0.6f, 0f);
        }
        else
        {
            bias = new Vector2(0.6f, 0f);
        }
        Debug.DrawRay(rayOrigin + bias, rayDirection * groundDetectRadius, Color.blue, debugDrawDuration);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin + bias, rayDirection, groundDetectRadius);
        if (hit.collider != null)
        {
            float direction = pawn.GetEntity().IsTowardsLeft() ? -1f : 1f;
            pawn.Move(direction);
            //Debug.Log("Ground detected, continue patrol");
        }
        else
        {
            Debug.Log("No ground detected, turn around");
            Debug.Log("Current LayerMask: " + lm.value + " (should include Ground)");
            Debug.Log($"Ray from {rayOrigin + bias} to down {groundDetectRadius} units, LayerMask: {lm}");
            pawn.Move(0f);
            pawn.GetEntity().SetCertainStatus("isTowardsLeft", !pawn.GetEntity().IsTowardsLeft());
        }
    }
}
