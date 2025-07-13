using System;
using UnityEngine;

namespace RTS.Building
{

    public class BuildingStatTypes : ScriptableObject
    {
        [Serializable]

        public class Base
        {
            public float cost, health, attack, attackSpeed, aggroRange, range, armor;
        }
    }
}

