using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    Rigidbody2D rb;
    Animator anim;
    GameObject animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = transform.Find("Animator").gameObject;
        anim = animator.GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        AnimationController();
    }

    private void AnimationController()
    {
        bool isMoving = rb.velocity.x != 0;
        bool isGrounded = GetComponent<IPawn>().IsGrounded();

        anim.SetFloat("yVelocity", rb.velocity.y);
;
        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isGrounded", isGrounded);

        if (GetComponent<IEntity>().IsTowardsLeft())
        {
            transform.localScale = new Vector3(-1,1,1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        if (GetComponent<IPawn>().IsOnTheWall())
        {
            anim.SetBool("isClimb", GetComponent<IPawn>().IsOnTheWall());
        }
        else
        {
            anim.SetBool("isClimb", GetComponent<IPawn>().IsOnTheWall());
        }

    }


}
