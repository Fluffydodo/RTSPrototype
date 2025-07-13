using RTS.Player;
using UnityEngine;
using UnityEngine.UI;

namespace RTS.UI.HUD
{
    public class SpawnUnit : MonoBehaviour
    {
        [SerializeField] Button button;
        [SerializeField] GameObject unit;
        [SerializeField] Transform unitType;
        Transform recruitmentPoint;
        private void Start()
        {
            button.onClick.AddListener(TaskOnClick);
        }

        public void SelectedBuildingForUI(Transform sB)
        {
            recruitmentPoint = sB.Find("SpawnUnitPoint");
        }

        private void TaskOnClick()
        {
            Transform parentTransform = unitType.transform;

            GameObject childGameObject = Instantiate(unit, recruitmentPoint.position, Quaternion.identity, unitType);
            PlayerManager.instance.SetTier1StatsSingular(childGameObject, unitType.gameObject, 1);
            childGameObject.name = unit.name;
         
        }
            
    }
}