using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{

    Rigidbody2D rb;
    Animator anim;
    GameObject animator;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = transform.Find("Animator").gameObject;
        anim = animator.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        AnimationController();
    }


    private void AnimationController()
    {
        bool isMoving = rb.velocity.x != 0;

        if (!GetComponent<IEntity>().IsTowardsLeft())
        {
            transform.localScale = new Vector3(-1 , transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        }

        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isDead", GetComponent<IEntity>().IsDead());
    }

}
