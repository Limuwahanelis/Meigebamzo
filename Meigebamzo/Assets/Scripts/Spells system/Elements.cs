using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Elements : MonoBehaviour
{
    public enum Element
    {
        PHYSICAL=0,FIRE=1,WATER,ELECTRICITY
    }
    public enum ElementAttackType
    {
        ELECTRICITY,SPREAD
    }
}
