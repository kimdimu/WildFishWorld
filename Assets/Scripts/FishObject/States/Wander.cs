using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : State<Fish>
{
    static private Wander instance = null;
    static public Wander Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject container = new GameObject();
                container.name = "WanderC";
                instance = container.AddComponent(typeof(Wander)) as Wander;
            }
            return instance;
        }
        set
        {
            instance = value;
        }
    }

    public override void Enter(Fish fish)
    {
        Debug.Log("Enter Wander");
        fish.fIshSteeringBehavior.WanderOn();
    }
    public override void Execute(Fish fish)
    {
        //fish.GetFSM().ChangeState(Flee.Instance);
    }
    public override void Exit(Fish fish)
    {
        fish.fIshSteeringBehavior.WanderOff();
    }
}
