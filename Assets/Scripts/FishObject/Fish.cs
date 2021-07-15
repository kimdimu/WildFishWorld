using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    public MovingEntity movingEntity;
    public FishStat FishStat;
    public FIshSteeringBehavior fIshSteeringBehavior;
    public FishStateMachine<Fish> fishStateMachine;
    public GameObject target;

    Vector3 acceleration; // 가속도
    Vector2 steeringForce;// 힘
    Vector3 mass;// 질량
    public float speed;//곱해줄 속도

    public float wanderRadius;
    public float wanderDist;
    public float wanderJitter;

    void Start()
    {
        fIshSteeringBehavior = new FIshSteeringBehavior();
        fIshSteeringBehavior.SetPlayer(this);

        fishStateMachine = new FishStateMachine<Fish>();
        fishStateMachine.SetOwner(this);
        fishStateMachine.SetCS(Wander.Instance);
        fIshSteeringBehavior.WanderOn();

        fIshSteeringBehavior.SetWander(wanderRadius, wanderDist, wanderJitter);

        //fishStateMachine.SetGS(GlobalState.Instance);
    }
    void Update()
    {
        //transform.LookAt(transform.position + movingEntity.m_vVelocity * Time.deltaTime * speed);

        //transform.rotation = Mathf.Acos(Vector3.Dot(new Vector3(3, 5), new Vector3(0, 1) /
        //          new Vector3(3, 5).magnitude * new Vector3(0, 1).magnitude)) * Mathf.Rad2Deg;


        fishStateMachine.sUpdate();

        steeringForce = fIshSteeringBehavior.Calculate();

        Vector3 v = steeringForce - new Vector2(transform.position.x, transform.position.y);
        float ang = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0,0, transform.rotation.z+ang);

        acceleration = steeringForce;// /mass;
        movingEntity.m_vVelocity = acceleration * Time.deltaTime; //가 = 속도변화량 / 시
        transform.position += movingEntity.m_vVelocity * Time.deltaTime * speed; // 거 = 속 * 시
    }
    public FishStateMachine<Fish> GetFSM() { return fishStateMachine; }
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
