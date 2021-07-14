using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    public FishStat FishStat;
    public FIshSteeringBehavior fIshSteeringBehavior;
    public FishStateMachine<Fish> fishStateMachine;
    public GameObject target;
    void Start()
    {
        fIshSteeringBehavior = new FIshSteeringBehavior();
        fishStateMachine = new FishStateMachine<Fish>();
    }
    void Update()
    {
        
    }
    public List<GameObject> GetTargets(float dis)
    {
        List<GameObject> enemiesInRange = new List<GameObject>();
        enemiesInRange.Clear();
        foreach (Collider c in Physics.OverlapSphere
                                    (transform.position, dis))
        {
            enemiesInRange.Add(c.gameObject);
        }
        return enemiesInRange;
    }
}
