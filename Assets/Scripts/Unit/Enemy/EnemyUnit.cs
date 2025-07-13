using RTS.Unit;
using UnityEngine;
using UnityEngine.AI;

namespace RTS.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    
    public class EnemyUnit : MonoBehaviour
    {
        public UnitStatTypes.Base baseStats;
        private NavMeshAgent navAgent;
        private Collider[] rangeColliders;
        [SerializeField] private Transform aggroTarget;
        [SerializeField] private UnitStatDisplay aggroUnit;
        private bool isAggro;
        private float distance;
        private float atkCooldown;

        
        private void Awake()
        {
           navAgent = gameObject.GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            atkCooldown -= Time.deltaTime;
            if(!isAggro)
            {
                CheckForTargets();
            }
            else
            {
                MoveToAggroTarget();
                Attack(); 
            }
        }

        private void CheckForTargets()
        {
            Collider[] rangeColliders = Physics.OverlapSphere(transform.position, baseStats.aggroRange, UnitHandler.instance.pUnitLayer);

            for(int i=0; i < rangeColliders.Length;)
            {
                
                aggroTarget = rangeColliders[i].gameObject.transform;
                aggroUnit = aggroTarget.gameObject.GetComponentInChildren<UnitStatDisplay>();
                isAggro = true;
                break;
            }
        }

        private void MoveToAggroTarget()
        {
            
            if(aggroTarget == null)
            {
                navAgent.SetDestination(transform.position);
                isAggro = false;
                aggroTarget = null;
            }
            else
            {
                distance = Vector3.Distance(aggroTarget.position, transform.position);
                //to be adjusted based on models
                navAgent.stoppingDistance = (baseStats.range + 1);

                if(distance <= baseStats.aggroRange && aggroTarget)
                {
                    navAgent.SetDestination(aggroTarget.position);
                }
                else if(distance > baseStats.aggroRange && aggroTarget)
                {
                    navAgent.SetDestination(transform.position);
                    isAggro = false;
                    aggroTarget = null;
                }
            }
        }

        private void Attack()
        {
            if(atkCooldown <= 0 && distance <= baseStats.range + 1)
            {
                aggroUnit.TakeDamage(baseStats.attack);
                atkCooldown = baseStats.attackSpeed;
            }
            else if(aggroTarget && aggroTarget.gameObject.layer == 10) //harvest resource
            {
                if(atkCooldown <= 0 && distance <= baseStats.range + 1)
                {
                    //aggroTarget.GetComponent<Resources.ResourceObject>().GiveResource(baseStats.attack);
                    atkCooldown = baseStats.attackSpeed;
                }
            }
        }

       
    }
}