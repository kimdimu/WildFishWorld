using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : State<Fish>
{
    static private Attack instance = null;
    static public Attack Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject container = new GameObject();
                container.name = "WanderC";
                instance = container.AddComponent(typeof(Attack)) as Attack;
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
    }
    public override void Execute(Fish player)
    {
        player.GetFSM().ChangeState(Wander.Instance);
    }
    public override void Exit(Fish player)
    {
    }
}
