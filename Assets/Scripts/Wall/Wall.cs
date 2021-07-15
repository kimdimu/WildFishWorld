using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public Vector2 a, b;
    Vector2 n;

    private void OnEnable()
    {
        CalculateNormal();
    }

    void CalculateNormal()
    {
        Vector2 t = (b - a).normalized;

        n.x = -t.y;
        n.y = t.x;
    }


}
