using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flee : State<Fish>
{
    static private Flee instance = null;
    static public Flee Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject container = new GameObject();
                container.name = "FleeC";
                instance = container.AddComponent(typeof(Flee)) as Flee;
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
        Debug.Log("Enter Flee");
    }
    public override void Execute(Fish player)
    {
        player.GetFSM().ChangeState(Wander.Instance);
    }
    public override void Exit(Fish player)
    {
    }
}
