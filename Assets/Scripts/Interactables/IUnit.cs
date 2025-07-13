using RTS.InputManager;
using RTS.UI.HUD;
using UnityEngine;

namespace RTS.Interactables
{
    public class IUnit : Interactable
    {
        public UnitListCard unitCard;

        private GameObject unitCardPrefab;

        public override void OnInteractEnter()
        {
            AddToUnitList();

            base.OnInteractEnter();
        }

        public override void OnInteractExit()
        {
            base.OnInteractExit();

            //for now
            Destroy(unitCardPrefab);
        }

        private void AddToUnitList()
        {
            if(InputHandler.instance.selectedUnits.Count > 1)
            {
                unitCardPrefab = Instantiate(unitCard.prefab, UnitList.instance.gameObject.transform.Find("GridGroups"));

                unitCardPrefab.GetComponent<UnitCard>().SetUnitCardStats(gameObject, unitCard.unitInformation);

                //this is to fix bug where the first unit card is not instantiated
                if(InputHandler.instance.selectedUnits.IndexOf(gameObject.transform) == 1)
                {
                    InputHandler.instance.selectedUnits[0].GetComponent<IUnit>().OnInteractEnter();
                }


                //Add a script to unitCard that tracks hp, portrait and stuff
            }
        }
    }
}
