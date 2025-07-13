using System;
using RTS.Enemy;
using RTS.Player;
using UnityEngine;
using UnityEngine.Serialization;

namespace RTS.UI.HUD
{
    public class UIHandler : MonoBehaviour
    {
        public static UIHandler instance;

        private Transform selectedUnit;

        [FormerlySerializedAs("RecruitmentUI")] [SerializeField] public GameObject recruitmentUI;

        [FormerlySerializedAs("UnitInformationUI")] [SerializeField] public GameObject unitInformationUI;

        [FormerlySerializedAs("UnitListUI")] [SerializeField] public GameObject unitListUI;

        private float currentHealth;

        private void Awake()
        {
            instance = this;

        }

        public void HideUnitUI()
        {
            recruitmentUI.GetComponent<UIVisibility>().OnUIExit();
            unitInformationUI.GetComponent<UIVisibility>().OnUIExit();
            unitListUI.GetComponent<UIVisibility>().OnUIExit();
        }

        public void SetSelectedBuildingForUI()//Transform sB)scuffed fix pls
        {
            recruitmentUI.GetComponent<UIVisibility>().OnUIEnter();
            unitInformationUI.GetComponent<UIVisibility>().OnUIExit();
            unitListUI.gameObject.GetComponent<UIVisibility>().OnUIExit();

            //foreach(Transform button in RecruitmentUI.transform)
            //{
            //    button.gameObject.GetComponent<SpawnUnit>().SelectedBuildingForUI(sB);
            //}
           
        }

        public void SetUnitForUI(Transform tf)
        {
            recruitmentUI.GetComponent<UIVisibility>().OnUIExit();
            unitInformationUI.GetComponent<UIVisibility>().OnUIEnter();
            unitListUI.GetComponent<UIVisibility>().OnUIExit();

            selectedUnit = tf; 
            SetUIUnitInformation();
        }

        public void SetMultipleUnitsForUI()
        {
            recruitmentUI.GetComponent<UIVisibility>().OnUIExit();
            unitInformationUI.GetComponent<UIVisibility>().OnUIExit();
            unitListUI.GetComponent<UIVisibility>().OnUIEnter();
        }


        private void SetUIUnitInformation()
        {
            try
            {
                GameObject o;
                PlayerUnit pU = (o = selectedUnit.gameObject).GetComponentInParent<PlayerUnit>();
                
                unitInformationUI.GetComponent<UnitInformationUI>().ShowPlayerUnitStats(o, pU);
            }
            catch (Exception)
            {
                try
                {
                    GameObject o;
                    EnemyUnit eU = (o = selectedUnit.gameObject).GetComponentInParent<EnemyUnit>();

                    unitInformationUI.GetComponent<UnitInformationUI>().ShowEnemyUnitStats(o, eU);
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

    }
}

