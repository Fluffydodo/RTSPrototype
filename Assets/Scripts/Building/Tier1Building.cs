using UnityEngine;

namespace RTS.Building
{

    [CreateAssetMenu(fileName = "New Building", menuName = "New Building/Tier 1")]
    public class Tier1Building : ScriptableObject
    {
        public enum BuildingType
        {
            Barracks,
            Tower,
            TownHall,
            House
        }

        [Header("Building Settings")]
        [Space(15)]
        public BuildingType type;
        public int buildingID;
        public new string name;
        public GameObject buildingPrefab;
        public GameObject icon;
        public float spawnTime;

        [Space(40)]
        [Header("Building Base Stats")]
    
        [Space(15)]
        public BuildingStatTypes.Base baseStats;

    }
}
