using RTS.Building;
using RTS.Building.Player;
using RTS.Enemy;
using RTS.InputManager;
using RTS.Unit;
using UnityEngine;

namespace RTS.Player
{
    public class PlayerManager : MonoBehaviour
    {
        public static PlayerManager instance;
        
        public Transform playerUnits;
        public Transform enemyUnits;

        public Transform playerBuildings;
        public Transform enemyBuildings;

        private void Awake()
        {
            instance = this;
            SetTier1Stats(playerUnits);
            SetTier1Stats(playerBuildings);
            SetTier1Stats(enemyUnits);
        }

        private void Update()
        {
            InputHandler.instance.HandleUnitMovement();
        }

        public void SetTier1StatsSingular(GameObject playerUnit, GameObject parent, int type)
        {
            string name = parent.name.Substring(0, parent.name.Length - 1).ToLower();

            switch(type)
            { 
                case var value when value == 1:
                PlayerUnit pU = playerUnit.GetComponent<PlayerUnit>();
                pU.baseStats = UnitHandler.instance.GetTier1Stats(name);
                break;
                //case var value when value == enemyUnits:
                //Enemy.EnemyUnit eU = tf.GetComponent<Enemy.EnemyUnit>();
                //eU.baseStats = Unit.UnitHandler.instance.GetTier1Stats(name);
                //break;
                //case var value when value == playerBuildings:
                //Building.Player.PlayerBuilding pB = tf.GetComponent<Building.Player.PlayerBuilding>();
                //pB.baseStats = Building.BuildingHandler.instance.GetTier1Stats(name);
                //break;
                default:
                Debug.Log("transform(units,buildings,etc.) not found");
                break;
            } 
        }

        public void SetTier1Stats(Transform type)
        {
            foreach(Transform child in type)
            {
                foreach(Transform tf in child)
                {
                    string name = child.name.Substring(0, child.name.Length - 1).ToLower();
                    //var stats = Unit.UnitHandler.instance.GetTier1Stats(unitName);

                    switch(type)
                    {
                        case var value when value == playerUnits:
                        PlayerUnit pU = tf.GetComponent<PlayerUnit>();
                        pU.baseStats = UnitHandler.instance.GetTier1Stats(name);
                        break;
                        case var value when value == enemyUnits:
                        EnemyUnit eU = tf.GetComponent<EnemyUnit>();
                        eU.baseStats = UnitHandler.instance.GetTier1Stats(name);
                        break;
                        case var value when value == playerBuildings:
                        PlayerBuilding pB = tf.GetComponent<PlayerBuilding>();
                        pB.baseStats = BuildingHandler.instance.GetTier1Stats(name);
                        break;
                        default:
                        Debug.Log("transform(units,buildings,etc.) not found");
                        break;
                    } 

                    //if upgraded/buffed, add them here
                    //add upgrades to unit stats

                }
            }
        }
    }
}

