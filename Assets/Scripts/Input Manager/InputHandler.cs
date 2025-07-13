using System.Collections.Generic;
using System.Linq;
using RTS.Interactables;
using RTS.Player;
using RTS.UI.HUD;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RTS.InputManager
{
    public class InputHandler : MonoBehaviour
    {
        public static InputHandler instance;

        [SerializeField] private LayerMask ignoreRayCastLayer;
        private RaycastHit hit;

        public List<Transform> selectedUnits = new List<Transform>();

        private Transform selectedBuilding, selectedEnemy;
        private string selectedInteractable;

        public LayerMask interactableLayer;
        
        private bool isDragging;

        private Vector3 mousePos;

        private float lastClicktime;

        private void Awake()
        {
            instance = this;
        }

        private void OnGUI()
        {
            if (!isDragging) return;
            var rect = MultiSelect.GetScreenRectangle(mousePos, Input.mousePosition);
            MultiSelect.DrawScreenRectangle(rect, new Color(0f, 0f, 0f, 0.25f) );
            MultiSelect.DrawScreenRectangleBorder(rect, 3, Color.green);

            //hover
            foreach(Transform child in PlayerManager.instance.playerUnits)
            {
                foreach(Transform unit in child)
                {
                    unit.Find("Hover").gameObject.SetActive(IsWithinSelectionBounds(unit));
                }
            }
        }

        public void HandleUnitMovement()
        {
            if(EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }


            if(Input.GetMouseButtonDown(0))
            {
                mousePos = Input.mousePosition;

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                //wack af fix eventually
                if(Physics.Raycast(ray, out hit, 100, ~ignoreRayCastLayer) && hit.transform.gameObject.CompareTag("Clickable"))
                {
                    if(AddedUnit(hit.transform, Input.GetKey(KeyCode.LeftShift), IsDoubleClicking()))
                    {
                    }
                    if(AddedBuilding(hit.transform))
                    {
                    }
                    if(AddedEnemy(hit.transform))
                    {
                    }
                }
                else
                {
                    isDragging = true;
                    DeselectUnit();
                }

            }

            if(Input.GetMouseButtonUp(0))
            {
                foreach(Transform child in PlayerManager.instance.playerUnits)
                {
                    foreach(Transform unit in child)
                    {
                        if(IsWithinSelectionBounds(unit))
                        {
                            AddedUnit(unit, true);
                        }
                    }
                }
                isDragging = false;
            }

            if(Input.GetMouseButtonDown(1) && HaveSelectedUnits()) //rightclick
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    

                if (Physics.Raycast(ray, out hit, 1000f, ~ignoreRayCastLayer))
                {
                    LayerMask layerHit = hit.transform.gameObject.layer;  

                    switch(layerHit.value)
                    {
                        
                        case 7: //enemy unit
                            foreach(Transform unit in selectedUnits)
                            {
                                if(unit)
                                {
                                    GameObject enemyTarget = hit.transform.gameObject;
                                    unit.GetComponent<PlayerUnit>().PlayerUnitSetTargets(enemyTarget);
                                }
                            }
                            break;

                        case 8:
                            foreach(Transform unit in selectedUnits)
                            {
                                if(unit)
                                {
                                    GameObject friendlyTarget = hit.transform.gameObject;
                                    unit.GetComponent<PlayerUnit>().PlayerUnitSetTargets(friendlyTarget);
                                }
                            }
                            break;

                        case 10:
                            foreach(Transform unit in selectedUnits)
                            {
                                if(unit)
                                {
                                    GameObject resource = hit.transform.gameObject;
                                    unit.GetComponent<PlayerUnit>().PlayerUnitSetTargets(resource);
                                }
                            }
                            break;

                        default:
                            foreach(Transform unit in selectedUnits)
                            {
                                if(unit)
                                {
                                    PlayerUnit pU = unit.gameObject.GetComponent<PlayerUnit>();
                                    pU.MoveUnit(hit.point);
                                }
                            }
                            break;
                    }
                }
            }
            else if((Input.GetMouseButtonDown(1) && selectedBuilding != null))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    selectedBuilding.gameObject.GetComponent<IBuilding>()
                        .SetRallyPoint(false);
                }
                
            }
        }
        
        private void DeselectUnit()
        {
            if(selectedBuilding)
            {
                
                selectedBuilding.gameObject.GetComponent<IBuilding>().OnInteractExit();
                UIHandler.instance.recruitmentUI.GetComponent<UIVisibility>().OnUIExit();
                selectedBuilding = null;
            }
            else if(selectedEnemy)
            {
                selectedEnemy.gameObject.GetComponent<IEnemy>().OnInteractExit();
                UIHandler.instance.unitInformationUI.GetComponent<UIVisibility>().OnUIExit();
                selectedEnemy = null;
            }
                
            

            foreach (var t in selectedUnits.Where(t => t != null))
            {
                t.gameObject.GetComponent<IUnit>().OnInteractExit();
            }
            selectedUnits.Clear();
        }

        private bool IsWithinSelectionBounds(Transform tf)
        {
            if(!isDragging)
            {
                return false;
            }

            Camera cam = Camera.main;
            Bounds vpBounds = MultiSelect.GetVPBounds(cam, mousePos, Input.mousePosition);
            return vpBounds.Contains(cam.WorldToViewportPoint(tf.position));
        }

        private bool IsDoubleClicking()
        {
            if(Input.GetMouseButtonDown(0))
            {
                float timeSinceLastClick = Time.time - lastClicktime;
                if(timeSinceLastClick <= .2f)
                {
                    return true;
                }

                lastClicktime = Time.time;
                return false;
            }
            return false;
        }

        private bool HaveSelectedUnits()
        {
            if(selectedUnits.Count > 0)
            {
                return true;
            }

            return false;
        }

        private IUnit AddedUnit(Transform tf, bool canMultiSelect = false, bool isDoubleClicking = false)
        {
            IUnit iUnit = tf.GetComponent<IUnit>();
            if(iUnit)
            {
                if(!canMultiSelect)  
                {
                    DeselectUnit();
                }
                

                if (isDoubleClicking) 
                {
                    SelectUnitofSameType(tf);
                    return iUnit;   
                }

                if(selectedUnits.Contains(iUnit.gameObject.transform) && canMultiSelect && !isDragging) 
                {
                    GameObject o;
                    (o = iUnit.gameObject).transform.Find("Hover").gameObject.SetActive(false);
                    selectedUnits.Remove(o.transform);
                    iUnit.OnInteractExit();
                    return null;
                }

                if(selectedUnits.Contains(iUnit.gameObject.transform) == false)
                {
                    GameObject o;
                    (o = iUnit.gameObject).transform.Find("Hover").gameObject.SetActive(false);
                    selectedUnits.Add(o.transform);
                }
                
                SendUIUnitData(tf);

                iUnit.OnInteractEnter();
                return iUnit;
            }

            return null;
        }

        private IBuilding AddedBuilding(Transform tf)
        {
            IBuilding iBuilding = tf.GetComponent<IBuilding>();
            if(iBuilding)
            {
                DeselectUnit();

                selectedBuilding = iBuilding.gameObject.transform;
                
                gameObject.GetComponent<UIHandler>().SetSelectedBuildingForUI(); //(selectedBuilding);
                iBuilding.OnInteractEnter();

                return iBuilding;
            }

            return null;
        }

        private IEnemy AddedEnemy(Transform tf)
        {
            IEnemy iEnemy = tf.GetComponent<IEnemy>();
            if(iEnemy)
            {
                DeselectUnit();

                selectedEnemy = iEnemy.gameObject.transform;

                gameObject.GetComponent<UIHandler>().SetUnitForUI(selectedEnemy);
                iEnemy.OnInteractEnter();

                return iEnemy;
            }

            return null;
        }

        private void SendUIUnitData(Transform tf)
        {
            switch (selectedUnits.Count)
            {
                case 1:
                    gameObject.GetComponent<UIHandler>().SetUnitForUI(tf);
                    break;
                case > 1:
                    gameObject.GetComponent<UIHandler>().SetMultipleUnitsForUI();
                    break;
            }
        }

        private IUnit SelectUnitofSameType(Transform tf)
        {
            Transform unitParent = tf.parent.gameObject.transform;
            foreach (Transform unit in unitParent)
            {
                if(selectedUnits.Contains(unit) == false)
                {
                    IUnit iCurrentUnit = unit.GetComponent<IUnit>();
                    selectedUnits.Add(iCurrentUnit.gameObject.transform);
                    iCurrentUnit.OnInteractEnter();
                    gameObject.GetComponent<UIHandler>().SetMultipleUnitsForUI();
                }
                else
                {
                    return null;
                }
            }
            return null;
        }
        
    }
}

