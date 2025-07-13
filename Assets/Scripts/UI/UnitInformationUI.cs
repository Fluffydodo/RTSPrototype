using RTS.Enemy;
using RTS.Interactables;
using RTS.Player;
using RTS.Unit;
using UnityEngine;
using UnityEngine.UI;

namespace RTS.UI.HUD
{
    public class UnitInformationUI : MonoBehaviour
    {
        private static UnitInformationUI _instance;
        [SerializeField]private Text currentHealthText, maxHealthText, attack, attackSpeed, armor;

        [SerializeField]private Image healthBarAmount; 
        [SerializeField]private Image portrait;

        private GameObject portraitPrefab, icon;

        private GameObject unit;
        private UnitStatTypes.Base baseStats;

        private void Awake()
        {
            _instance = this;
        }
        private void Update()
        {
            UpdateChangingStats();
        }

        public void ShowPlayerUnitStats(GameObject u, PlayerUnit pU)
        {
            if(icon)
            {
                Destroy(icon);
            }

            unit = u;
            baseStats = pU.baseStats;

            portrait.color = Color.blue; //player unit color

            portraitPrefab = pU.gameObject.GetComponent<IUnit>().unitCard.unitInformation.icon;
            icon = Instantiate(portraitPrefab, portrait.gameObject.transform);

            //displays stats on UI
            maxHealthText.text = baseStats.health.ToString();
            attack.text = baseStats.attack.ToString();
            attackSpeed.text = baseStats.attackSpeed.ToString();
            armor.text = baseStats.armor.ToString();

        }

        public void ShowEnemyUnitStats(GameObject e, EnemyUnit eU)
        {
            if(icon)
            {
                Destroy(icon);
            }

            unit = e;
            baseStats = eU.baseStats;

            portrait.color = Color.red; //enemy unit color

            portraitPrefab = eU.gameObject.GetComponent<IEnemy>().unitInformation.icon;
            icon = Instantiate(portraitPrefab, portrait.gameObject.transform);

            //displays stats on UI
            maxHealthText.text = baseStats.health.ToString();
            attack.text = baseStats.attack.ToString();
            attackSpeed.text = baseStats.attackSpeed.ToString();
            armor.text = baseStats.armor.ToString();
            
        }

        private void UpdateChangingStats()
        {
            if(unit)
            {
                UnitStatDisplay statDisplay = unit.GetComponentInChildren<UnitStatDisplay>();

                currentHealthText.text = statDisplay.currentHealth.ToString();
                healthBarAmount.fillAmount = statDisplay.currentHealth / baseStats.health;
            }
            else
            {
                gameObject.GetComponent<UIVisibility>().OnUIExit();
            }
        }

    }
}
