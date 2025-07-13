using System;
using RTS.Building.Player;
using RTS.Enemy;
using RTS.InputManager;
using RTS.Player;
using UnityEngine;
using UnityEngine.UI;

namespace RTS.Unit
{
    public class UnitStatDisplay : MonoBehaviour
    {

        public float maxHealth, armor, currentHealth;
        [SerializeField]private Image healthBarAmount;
        private bool isPlayerUnit;

        private void Awake()
        {
            
        }

        private void Start()
        {
            try
            {
                maxHealth = gameObject.GetComponentInParent<PlayerUnit>().baseStats.health;
                armor = gameObject.GetComponentInParent<PlayerUnit>().baseStats.armor;
                isPlayerUnit = true;
            }
            catch (Exception)
            {
                try
                {
                    maxHealth = gameObject.GetComponentInParent<EnemyUnit>().baseStats.health;
                    armor = gameObject.GetComponentInParent<EnemyUnit>().baseStats.armor;
                    isPlayerUnit = false;
                }
                catch (Exception)
                {
                    maxHealth = gameObject.GetComponentInParent<PlayerBuilding>().baseStats.health;
                    armor = gameObject.GetComponentInParent<PlayerBuilding>().baseStats.armor;
                    isPlayerUnit = false;
                }
            }

            currentHealth = maxHealth;
        }

        private void Update()
        {
            HandleHealth();
        }

        private void HandleHealth()
        {
            Camera camera = Camera.main;
            gameObject.transform.LookAt(gameObject.transform.position + camera.transform.rotation * Vector3.forward, camera.transform.rotation * Vector3.up);
            healthBarAmount.fillAmount = currentHealth / maxHealth;

            if(currentHealth <= 0)
            {
                Die();
            }
        }

         public void TakeDamage(float damage)
        {
            float totalDamage = damage - armor;
            if(totalDamage <= 0)
            {
                totalDamage = 1;
            }
            currentHealth -= totalDamage;
        }
        
        private void Die()
        {
            if(isPlayerUnit)
            {
                //other stuff
                InputHandler.instance.selectedUnits.Remove(gameObject.transform.parent);
                Destroy(gameObject.transform.parent.gameObject);

            }
            else{
                Destroy(gameObject.transform.parent.gameObject);
            }
           
        }
    }

}
