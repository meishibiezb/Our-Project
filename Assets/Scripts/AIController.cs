using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    IPawn pawn;
    IEntity entity;
    float excuteThreshold = 0.1f;
    float excuteTimer = 0f;
    [SerializeField] GameObject[] tasks;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        pawn = GetComponent<IPawn>();
        entity = GetComponent<IEntity>();
        if (pawn == null)
        {
            Debug.LogError("AIController: No IPawn component found on " + gameObject.name);
        }
        if (entity == null)
        {
            Debug.LogError("AIController: No IEntity component found on " + gameObject.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        excuteTimer += Time.deltaTime;
        if (excuteTimer < excuteThreshold)
        {
            return;
        }
        foreach (var task in tasks)
        {
            //Debug.Log("Executing task: " + task.name);
            task.GetComponent<ITask>().Execute(pawn);
        }
        excuteTimer = 0f;
    }
}
