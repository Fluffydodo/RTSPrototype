using UnityEngine;

namespace RTS.Building
{
    public class BuildingHandler : MonoBehaviour
    {
        public static BuildingHandler instance;

        [SerializeField]
        private Tier1Building barracks, towers, townHalls, houses;

        private void Awake()
        {
            instance = this;
        }
         

        public BuildingStatTypes.Base GetTier1Stats(string type)
        {
            Tier1Building building;

            switch(type)
            {
                case "barrack":
                building = barracks;
                break; 
                case "towers":
                building = towers;
                break; 
                case "town halls":
                building = townHalls;
                break;
                case "houses":
                building = houses;
                break;
                default:
                Debug.Log($"Building Type: {type} could not be found");
                return null;
            }

            return building.baseStats;
        }
    }
}

