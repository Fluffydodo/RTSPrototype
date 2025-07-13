using System.Collections.Generic;
using RTS.Building;
using RTS.Unit;
using UnityEngine;

namespace RTS.UI.HUD
{
    [CreateAssetMenu(fileName = "NewPlayerAction", menuName = "PlayerActions")]
    public class PlayerActions : ScriptableObject
    {
        [Space(5)]
        [Header("Units")]
        public List<UnitInformation> unitInformation = new List<UnitInformation>();

        [Space(15)]
        [Header("Buildings")]
        [Space(5)]
        public List<Tier1Building> buildingInformation = new List<Tier1Building>();
    }
}
