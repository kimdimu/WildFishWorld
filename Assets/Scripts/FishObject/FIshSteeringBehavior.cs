using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum SteeringState
{
    NONE, SEEK, FLEE, ARRIVE, WANDER, ATTACKMOVE, COHESION, SEPARATION, END
}
//벽피하기를 추가할것. 절단 뭐시기도 추가해볼것
public class FIshSteeringBehavior
{
    private bool[] OnState;
    Vector3 steeringF; //힘
    Vector3 zero; // 벡터 0,0,0
    Fish owner;

    Vector3 target;//wwander
    float wanderRadius; //원의 반경
    float wanderDist; //원이 투사되는 거리. 원 중심과의 거리.
    float wanderJitter; // 무작위 변위의 최대 크기
    Vector3 wanderTarget; //무작위변위를 더할 벡터
    float playerAngle;
    float angleGoal;
    Vector2 heading;

    float time; //시간
    bool isReady; //공격준비상태인가?
    bool isAttack;//공격상태인가?

    Vector3[] feeler = new Vector3[3];

    public FIshSteeringBehavior()
    {
        OnState = new bool[(int)SteeringState.END];
        zero = new Vector3(0, 0, 0);
    }
    public void SetPlayer(Fish owner)
    {
        this.owner = new Fish();
        this.owner = owner;
    }
    public void SetWander(float rad, float dist, float jit)
    {
        wanderRadius = rad;
        wanderDist = dist;
        wanderJitter = jit;
    }
    public void FleeOn() { OnState[(int)SteeringState.FLEE] = true; }
    public void SeekOn() { OnState[(int)SteeringState.SEEK] = true; }
    public void WanderOn() { OnState[(int)SteeringState.WANDER] = true; }
    public void ArriveOn() { OnState[(int)SteeringState.ARRIVE] = true; }
    public void SeparationOn() { OnState[(int)SteeringState.SEPARATION] = true; }
    public void CohetionOn() { OnState[(int)SteeringState.COHESION] = true; }
    public void AttackmoveOn() { OnState[(int)SteeringState.ATTACKMOVE] = true; }

    public void FleeOff() { OnState[(int)SteeringState.FLEE] = false; }
    public void SeekOff() { OnState[(int)SteeringState.SEEK] = false; }
    public void WanderOff() { OnState[(int)SteeringState.WANDER] = false; }
    public void ArriveOff() { OnState[(int)SteeringState.ARRIVE] = false; }
    public void SeparationOff() { OnState[(int)SteeringState.SEPARATION] = false; }
    public void CohetionOff() { OnState[(int)SteeringState.COHESION] = false; }
    public void AttackmoveOff() { OnState[(int)SteeringState.ATTACKMOVE] = false; }

    public bool On(SteeringState state) { return OnState[(int)state]; }

    public Vector3 Calculate()
    {
        steeringF = zero;

        if (On(SteeringState.SEEK))
            steeringF += Seek(owner.target.transform.position);
        if (On(SteeringState.FLEE))
            steeringF += Flee(owner.target.transform.position);
        if (On(SteeringState.ARRIVE))
            steeringF += Arrive(owner.target.transform.position, 2);
        if (On(SteeringState.WANDER))
            steeringF += Wander();
        if (On(SteeringState.ATTACKMOVE))
            steeringF += AttackMove(owner.target);
        if (On(SteeringState.COHESION))
            steeringF += Cohesion(owner.GetTargets(10f));
        if (On(SteeringState.SEPARATION))
            steeringF += Separation(owner.GetTargets(1f));
        Debug.Log(steeringF);

        return steeringF;
    }
    Vector3 Seek(Vector3 targetPos)
    {
        Vector3 playerVelo = (targetPos - owner.transform.position).normalized;

        return playerVelo;
    }
    Vector3 Flee(Vector3 targetPos)
    {
        Vector3 playerVelo = (owner.transform.position - targetPos).normalized;

        return playerVelo;
    }

    Vector3 Arrive(Vector3 targetPos, float deceleration)
    {
        Vector3 Totarget = (targetPos - owner.transform.position);

        float dist = Totarget.magnitude;

        if (dist > 0)
        {
            float DecelerationTweaker = 0.3f;

            float speed = dist / deceleration * DecelerationTweaker;

            Vector3 DesireV;
            DesireV.x = Totarget.x * speed / dist;
            DesireV.y = Totarget.y * speed / dist;
            DesireV.z = Totarget.z * speed / dist;

            return DesireV;
        }

        //속도 리턴
        return Vector3.zero;
    }
    Vector3 Wander()
    {
        time -= Time.deltaTime;
        //heading = new Vector2(owner.transform.position.x,owner.transform.position.y).normalized;
        if (time < 0f)
        {
            //Debug.Log("초기화~ 단번에 늒껴");

            wanderTarget = zero; //초기화
                                 //소량의 무작위 벡터를 목표물 위치에 더한다.
            wanderTarget.x += Random.Range(-1f, 1f) * wanderJitter;
            wanderTarget.z += Random.Range(-1f, 1f) * wanderJitter;
            //정규화 후 원의 반지름에 맞춘다.
            wanderTarget.Normalize();
            wanderTarget *= wanderRadius;

            //playerAngle = owner.transform.eulerAngles.y;
            time = Random.Range(2,8 );
        }

        //반지름에 맞춘 위치에 현재 보고있는 방향 * 투사 거리만큼 더해준다. + 현재 위치 더해서 월드 좌표로 옮기기
        target.x = wanderTarget.x + owner.transform.position.x + owner.transform.forward.x * wanderDist;
        target.y = wanderTarget.z + owner.transform.position.z + owner.transform.forward.z * wanderDist;
        //target.z = wanderTarget.z + owner.transform.position.z + owner.transform.forward.z * wanderDist;
        //그 쪽으로 간다.
        Vector3 Velo = (target - owner.transform.position).normalized;
        return Velo;
    }
    Vector3 AttackMove(GameObject target)
    {
        if (isReady) // 공격 준비상태인가?
        {
            time += Time.deltaTime;
            if (time > 1)
            {
                isReady = false;
                isAttack = true;
                time = 0;
            }
            return zero; // 움직이지 않는다. 회전은 한다.
        }
        else if (isAttack) // 공격중인가?
        {
            time += Time.deltaTime;
            if (time > 0.3f)
            {
                isReady = false;
                isAttack = false;
                time = 0;
            }
            Vector3 jumppower = target.transform.position - owner.transform.position;
            jumppower.Normalize();
            jumppower *= 10; //빠른 속도로 전진한다.

            return jumppower;
        }
        else
        {
            //거리가 10보다 작다면
            if (Vector3.Distance(target.transform.position, owner.transform.position) < 10)
            {
                isReady = true; // 공격 준비 상태로 전환
                return zero; // 0 움직이지 않는다.
            }
            //거리가 10 이상이라면 
            else
            {
                isReady = false;
                isAttack = false;
                return Seek(target.transform.position);

            }

        }
    }
    Vector3 Cohesion(List<GameObject> target)
    {
        Vector3 mass = new Vector3();
        Vector3 sforce = new Vector3();
        int targetsCount = 0;

        for (int i = 0; i < target.Count; ++i)
        {
            if (target[i].gameObject.CompareTag("Player") && target[i] != owner.gameObject)
            {
                mass += target[i].transform.position;
                ++targetsCount;
            }
        }
        if (targetsCount > 0)
        {
            //mass /= targetsCount;
            //sforce = Seek(mass);
            mass /= targetsCount;
            Vector3 toTarget = mass - owner.transform.position;
            sforce += toTarget.normalized / (toTarget.magnitude);//거리가 길어질수록 힘이 적게 추가됨
            //sforce = Seek(mass);
        }

        return sforce;
    }
    Vector3 Separation(List<GameObject> target)
    {
        Vector3 sforce = new Vector3();
        for (int i = 0; i < target.Count; ++i)
        {
            if (target[i].gameObject.CompareTag("Player") && target[i] != owner.gameObject)
            {
                //타겟(다른놈)이 플레이어에게 가는 방향. 즉 도망침
                Vector3 toPlayer = owner.transform.position - target[i].transform.position;
                sforce += toPlayer.normalized / (toPlayer.magnitude);//거리가 길어질수록 힘이 적게 추가됨
                                                                     //Debug.Log(sforce);
            }
        }
        return sforce;
    }
    Vector2 WallAvoidance(Vector2[] walls)
    {
        Vector3 sforce = new Vector3();


        return sforce;
    }
    void CreateFeelers()
    {
        feeler[0] = owner.transform.forward;

        feeler[1].x = feeler[0].x*Mathf.Cos(45) - feeler[0].z*Mathf.Sin(45);
        feeler[1].z = feeler[0].x * Mathf.Sin(45) + feeler[0].z * Mathf.Cos(45);

        feeler[2].x = feeler[0].x * Mathf.Cos(-45) - feeler[0].z * Mathf.Sin(-45);
        feeler[2].z = feeler[0].x * Mathf.Sin(-45) + feeler[0].z * Mathf.Cos(-45);
    }
    bool LineIntersection2D(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
    {
        //float firstCCW =


        return false;
    }
    //int CCW(Vector2 a, Vector2 b, Vector2 c)
    //{

  //  }
    //Vector2 WallAvoidance(Vector2[] walls)
    //{
        Vector3 sforce = new Vector3();

        //// 촉수는 std::vector 내에, m_Feelers로 포함되어 있다.
        //CreateFeelers();
        //float DistToThisIP = 0.0f;


        //float DistToClosestIP = 10000f;
        //int ClosestWall = -1; // 이것은 벽 벡터들의 색인을 보유할 것이다.
        //Vector2 SteeringForce, point, /*일시적인 정보 저장*/ ClosestPoint; /*최근접교차점*/
        ////                                                            // 각 촉수를 차례로 조사한다.
        //for (int flr = 0; flr < feeler.Length; ++flr) {
        //    //교차하는 점들이 하나라도 있는지 확인하면서 각 벽을 조사
        //    for (int w = 0; w < walls.size(); ++w) {
        ////        if (LineIntersection2D(m_pVehiclePos(), feeler[flr], walls[w].From(), Walls[w].To(),
        ////        DistToThisIP, point)) {
        ////            //이것이 지금까지 발견된 것들 중 가장 근접한 것이라면, 기록을 유지한다.
        ////            if (DistToThisIP < DistToClosestIP) {
        ////                DistToClosestIP = DistToThisIP; ClosestWall = w;
        ////                ClosestPoint = point;
        ////            }
        ////        }
        ////    } //다음 벽// 교차점이 감지되었다면, 에이전트를 멀어지는 쪽으로 향하게 해줄 힘을 계산한다.
        ////    if (ClosestWall >= 0) {
        ////        //에이전트가 어떤 거리에서 투사한 위치가 벽을 지나치게 되는지를 계산한다.
        ////        Vector2 OverShoot = feeler[flr] – ClosestPoint;
        ////        //지나쳐버린 크기와 함께, 벽의 법선방향으로의 힘을 생성
        ////        SteeringForce = walls[ClosestWall].Normal() * OverShoot.magnitude;
        //    }
        //} //다음 촉수
        ////return SteeringForce;
       // return sforce;
   // }


}
