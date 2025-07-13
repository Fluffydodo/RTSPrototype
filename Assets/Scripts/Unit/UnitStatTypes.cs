using System;
using UnityEngine;

namespace RTS.Unit
{
    public class UnitStatTypes : ScriptableObject
    {
        [Serializable]
        public class Base
        {
            public float cost, health, attack, attackSpeed, aggroRange, range, armor;
        }
    }
}
