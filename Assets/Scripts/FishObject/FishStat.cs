 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Grade
{
    A,B,C,D,F
}
public struct FishGrade
{
    Grade grade;
    int level;
}
public class FishStat
{
    int hp;
    int love;
    int full;
    int giveMoney;
    int life;
    FishGrade grades;
}
