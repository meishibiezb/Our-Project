using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour, IAbility
{
    [SerializeField] GameObject bullet;
    [SerializeField] Vector3 bias;
    [SerializeField] Vector2 shootForce;
    [SerializeField] float cooldownThreshold;
    float cooldownTimer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Awake is called when the script instance is being loaded
    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        cooldownTimer += Time.deltaTime;
    }

    // 实现接口IAbility
    public void Activate(IEntity speller)
    {
        if (cooldownTimer < cooldownThreshold)
        {
            Debug.LogError("Waiting for cooldown:" + cooldownTimer + "<" + cooldownThreshold);
            return;
        }
        cooldownTimer = 0f;
        var pos = speller.GetGameObject().transform.position;
        var isLeft = speller.IsTowardsLeft();
        if (isLeft)
        {
            pos -= bias;
        }
        else
        {
            pos += bias;
        }
        var projectile = Instantiate(bullet, pos, Quaternion.identity);
        projectile.GetComponent<Rigidbody2D>().AddForce(new Vector2(isLeft ? -shootForce.x : shootForce.x, shootForce.y));
        projectile.GetComponent<IProjectile>()?.SetCreator(speller);
        projectile.tag = speller.GetGameObject().tag;
    }
}
