using UnityEngine;

namespace RTS.Unit
{
    public class UnitHandler : MonoBehaviour
    {
        public static UnitHandler instance;
        public LayerMask pUnitLayer, eUnitLayer;

        [SerializeField]
        private UnitInformation worker, warrior, archer;

        private void Awake()
        {
            instance = this;
        }
         

        public UnitStatTypes.Base GetTier1Stats(string type)
        {
            UnitInformation unit;

            switch(type)
            {
                case "worker":
                unit = worker;
                break; 
                case "warrior":
                unit = warrior;
                break; 
                case "archer":
                unit = archer;
                break;
                default:
                Debug.Log($"Unit Type: {type} could not be found");
                return null;
            }

            return unit.baseStats;
        }
        
        public UnitInformation GetTier1Info(string type)
        {
            UnitInformation unit;

            switch(type)
            {
                case "worker":
                unit = worker;
                break; 
                case "warrior":
                unit = warrior;
                break; 
                case "archer":
                unit = archer;
                break;
                default:
                Debug.Log($"Unit Type: {type} could not be found");
                return null;
            }

            return unit;
        }
        
        
    }
}

