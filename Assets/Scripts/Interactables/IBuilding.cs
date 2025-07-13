using RTS.UI.HUD;
using UnityEngine;

namespace RTS.Interactables
{
    public class IBuilding : Interactable
    {
        public PlayerActions actions;
        public GameObject rallyPoint;
        private Vector3 originalFlagPos;
        public GameObject unitSpawnPoint;
        public float maxPointDistance = 10f; //here incase rallypoint gets broken/unbalanced

        private void Start()
        {
            originalFlagPos = rallyPoint.transform.position;
        }
        
        public override void OnInteractEnter()
        {
            ActionFrame.instance.SetActionButtons(actions, unitSpawnPoint, rallyPoint);

            rallyPoint.SetActive(true);

            base.OnInteractEnter();
        }

        public override void OnInteractExit()
        {
            ActionFrame.instance.ClearAction();

            rallyPoint.SetActive(false);

            base.OnInteractExit();
        }

        public void SetRallyPoint(bool isItself)
        {
            if(isItself)
            {
                rallyPoint.transform.position = originalFlagPos;
                ActionFrame.instance.IsRallying(false);
            }
            else
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if(Physics.Raycast(ray, out hit))
                {
                    rallyPoint.transform.position = hit.point;
                    ActionFrame.instance.IsRallying(true);
                }
            }
            


        }
    }
}
