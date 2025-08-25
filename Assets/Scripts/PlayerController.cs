using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    IPawn character;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // This method is called when the script instance is being loaded
    void Awake()
    {
        character = GetComponent<IPawn>();
    }

    // Update is called once per frame
    void Update()
    {
        //跳跃
        if (Input.GetButtonDown("Jump"))
        {
            character.Jump();
        }
        //使用技能
        if (Input.GetKeyDown(KeyCode.H))
        {
            character.UseAbility(0);
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            character.UseAbility(1);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            character.UseAbility(2);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            character.UseAbility(3);
        }
    }

    //FixedUpdate is called at a fixed interval and is used for physics calculations
    void FixedUpdate()
    {
        //左右移动
        float moveInput = Input.GetAxis("Horizontal");
        character.Move(moveInput);
        //攀爬
        if (Input.GetKey(KeyCode.W))
        {
            character.Clmaber(1f);
        }
    }
}
