using System.Collections.Generic;
using RTS.Building;
using RTS.Interactables;
using RTS.Player;
using RTS.Resources;
using RTS.Unit;
using UnityEngine;
using UnityEngine.UI;

namespace RTS.UI.HUD
{
    public class ActionFrame : MonoBehaviour
    {
        public static ActionFrame instance;

        [SerializeField] private Button actionButton;
        [SerializeField] private Transform layoutGroup;

        private List<Button> buttonList = new List<Button>();
        private PlayerActions actionsList;

        public List<float> spawnQueue = new List<float>();
        public List<GameObject> spawnOrder = new List<GameObject>();

        [SerializeField]private List<Transform> unitType = new List<Transform>();
        [SerializeField]private List<Transform> buildingType = new List<Transform>();

        private List<Transform> setParent = new List<Transform>();

        public GameObject unitSpawnPoint;
        public GameObject rallyPoint;
        private bool isRallying;

        private void Awake()
        {
            instance = this;
        }

        public void SetActionButtons(PlayerActions actions, GameObject uSpawnP, GameObject rallyP)
        {
            actionsList = actions;
            rallyPoint = rallyP;
            unitSpawnPoint = uSpawnP;

            if(actions.unitInformation.Count > 0)
            {
                foreach(UnitInformation unit in actions.unitInformation)
                {
                    Button btn = Instantiate(actionButton, layoutGroup);
                    GameObject icon = Instantiate(unit.icon, btn.transform);
                    btn.name = unit.name;
                    //add costs text
                    buttonList.Add(btn);

                }
            }

            if(actions.buildingInformation.Count > 0)
            {
                foreach(Tier1Building building in actions.buildingInformation)
                {
                    Button btn = Instantiate(actionButton, layoutGroup);
                    GameObject icon = Instantiate(building.icon, btn.transform);
                    btn.name = building.name;
                    
                    //add costs text
                    buttonList.Add(btn);
                }
            }
        }

        public void ClearAction()
        {
            
            foreach(Button btn in buttonList)
            {
                Destroy(btn.gameObject);
            }
            buttonList.Clear();
        }

        public void StartSpawnTimer(string objectToSpawn)
        {
            if(IsUnit(objectToSpawn))
            {
                UnitInformation unit = IsUnit(objectToSpawn);

                string type = unit.type + "s";

                if( ResourceHandler.instance.goldStockpile > unit.baseStats.cost)
                {
                    ResourceHandler.instance.goldStockpile -= unit.baseStats.cost;

                    SetType(type,1);

                    spawnQueue.Add(unit.spawnTime);
                    spawnOrder.Add(unit.unitPrefab);
                }
                
            }
            else if(IsBuilding(objectToSpawn))
            {
                Tier1Building building = IsBuilding(objectToSpawn);

                string type = building.type + "s";

                if( ResourceHandler.instance.goldStockpile > building.baseStats.cost)
                {
                    ResourceHandler.instance.goldStockpile -= building.baseStats.cost;

                    SetType(type,2);

                    spawnQueue.Add(building.spawnTime);
                    spawnOrder.Add(building.buildingPrefab);
                }
            }


            if(spawnQueue.Count == 1)
            {
                ActionTimer.instance.StartCoroutine(ActionTimer.instance.SpawnQueueTimer());
            }
            else if(spawnQueue.Count == 0)
            {
                ActionTimer.instance.StopAllCoroutines();
            }
        }

        private UnitInformation IsUnit(string name)
        {
            if(actionsList.unitInformation.Count > 0 )
            {
                foreach(UnitInformation unit in actionsList.unitInformation)
                {
                    if(unit.name == name)
                    {
                        return unit;
                    }
                }
            }
            return null;
        }

        private Tier1Building IsBuilding(string name)
        {
            if(actionsList.buildingInformation.Count > 0 )
            {
                foreach(Tier1Building building in actionsList.buildingInformation)
                {
                    if(building.name == name)
                    {
                        return building;
                    }
                }
            }
            return null;
        }

        public void IsRallying(bool isRally)
        {
            if(isRally)
            {
                isRallying = true;
            }
            else
            {
                isRallying = false;
            }
            
        }

        public void SpawnObject()
        {
            int spawnAttempts = 0;
            bool validPosition = false;

            Vector3 position = new Vector3(unitSpawnPoint.transform.position.x,unitSpawnPoint.transform.position.y, unitSpawnPoint.transform.position.z);

            while(!validPosition && spawnAttempts < 20)
            {
                spawnAttempts++;

                validPosition = true;

                Collider[] colliders = Physics.OverlapSphere(position, 0.1f);


                foreach(Collider col in colliders)
                {
                    if(col.GetComponent<IUnit>())
                    {
                        validPosition = false;
                        position = new Vector3(unitSpawnPoint.transform.position.x,unitSpawnPoint.transform.position.y, unitSpawnPoint.transform.position.z) + new Vector3 (Random.Range(-2,2f), 0, Random.Range(-2,2f));
                    }
                }

            }

            if(validPosition)
            {
                GameObject spawnedObject = Instantiate(spawnOrder[0], position, Quaternion.identity, setParent[0]);

                PlayerManager.instance.SetTier1StatsSingular(spawnedObject, setParent[0].gameObject, 1);
                spawnedObject.name = spawnOrder[0].name;

                if(isRallying)
                {
                    spawnedObject.GetComponent<PlayerUnit>().MoveUnit(rallyPoint.transform.position);
                }

                spawnOrder.Remove(spawnOrder[0]);
                setParent.Remove(setParent[0]);
            }  

        }

        private void SetType(string type, int typeID)
        {
            switch(typeID)
            {
                case 1: //units
                    for (int i=0; i< unitType.Count; i++) 
                    {
                        if (unitType[i].name == type) 
                        {
                            setParent.Add(unitType[i]);
                            break; 
                        }
                    }
                break;
                
                case 2: //buildings 
                    for (int i=0; i< buildingType.Count; i++) 
                    {
                        if (buildingType[i].name == type) 
                        {
                            setParent.Add(buildingType[i]);
                            break; 
                        }
                     }
                break;

            }
            
        }
    }
}
