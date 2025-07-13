using RTS.Unit;
using UnityEngine;
using UnityEngine.UI;

//copy paste to unity editor + attach to unitcard gameobject

namespace RTS.UI.HUD
{
    public class UnitCard : MonoBehaviour
    {
    	private GameObject unit;

        [SerializeField] private GameObject portrait;
    	[SerializeField] private Image healthBar;
    	private float currentHealth, maxHealth;
    
    	private void Update()
    	{
    		UpdateChangingStats();
    	}
    
    	public void SetUnitCardStats(GameObject u, UnitInformation uInfo )
    	{
    		if(u) 
    		{	
    			unit = u;
    
    			maxHealth = uInfo.baseStats.health;	
                GameObject icon = Instantiate(uInfo.unitCard, portrait.transform);
    		}
    	}
    
    	private void UpdateChangingStats()
    	{
    		if(unit)
    		{
    			currentHealth = unit.transform.Find("UnitStatDisplay").GetComponent<UnitStatDisplay>().currentHealth;
    			healthBar.fillAmount = currentHealth/ maxHealth;
    
    			if( currentHealth <= 0)
    			{
    				Die();
    			}
    		}
    		else
    		{
    			Die();
    		}
    	}
    
    	private void Die()
    	{
    		if(gameObject)
    		{
    			Destroy(gameObject);
    		}
    		else
    		{
    			//just in case
    			Destroy(gameObject);
    		}
    	}
    
    	//next thing to do is to add a function if a unit card is clicked once, hover unit hp in list and if clicked again,
    	//remove every transform in selectedUnits except the double clicked unit
    }
}
