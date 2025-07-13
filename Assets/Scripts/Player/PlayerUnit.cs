using RTS.Enemy;
using RTS.Interactables;
using RTS.Resources;
using RTS.Unit;
using RTS.Unit.Projectile;
using UnityEngine;
using UnityEngine.AI;

namespace RTS.Player
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class PlayerUnit : MonoBehaviour
    {
        private NavMeshAgent navAgent;
        public UnitStatTypes.Base baseStats;
        private Collider[] rangeColliders;
        [SerializeField] private Transform aggroTarget;
        private UnitStatDisplay aggroUnit;
        private UnitInformation unitInfo;
        private Bow rWeapon;
        private bool isAggro;
        public bool isMarching, isTargetSetManually;
        private float distance;
        private float atkCooldown;
        
        public void OnEnable()
        {
            //reference
            navAgent = GetComponent<NavMeshAgent>();
        }

        private void Awake()
        {
            unitInfo = gameObject.GetComponent<IUnit>().unitCard.unitInformation;

            if(isMelee() == false)
            {
                rWeapon = GetComponentInChildren<Bow>();
            }
        }

        private void Update()
        {
            atkCooldown -= Time.deltaTime;
            if(!isAggro)
            {
                CheckForTargets();
            }
            else if(isAggro && !isMarching)
            {
                MoveToAggroTarget();
                Attack(); 
            }
            
        }

        private void FixedUpdate()
        {
            if(navAgent.velocity.magnitude <= 0.01f && navAgent.remainingDistance <= 2f) //0.15f is safe magnitude
            {
                CancelMarch();
            }
        }

        public void MoveUnit(Vector3 _destination)
        {
            navAgent.stoppingDistance = 0f;
            navAgent.SetDestination(_destination);

            isMarching = true;
            isTargetSetManually = false;

            if(aggroTarget)
            {
                if(aggroTarget.gameObject.layer == 8 || aggroTarget.gameObject.layer == 10) //playerunits and resources
                {
                    aggroTarget = null;
                }
            }
        }

        private void CancelMarch()
        {
            isMarching = false;
        }

        private void CheckForTargets()
        {
            Collider[] rangeColliders = Physics.OverlapSphere(transform.position, baseStats.aggroRange, UnitHandler.instance.eUnitLayer);

            for(int i=0; i < rangeColliders.Length; i++)
            {
                //temporary solution if this breaks or is not enough: layer == LayerMask.NameToLayer("Interactables")
                if(transform.parent.gameObject.name != "Workers")
                {
                    aggroTarget = rangeColliders[i].gameObject.transform;
                    aggroUnit = aggroTarget.gameObject.GetComponentInChildren<UnitStatDisplay>();
                    isAggro = true;

                    SetRangedTarget();

                    break;
                }
            }
        }
        
        public void PlayerUnitSetTargets(GameObject target)
        {   
            if(target != gameObject && target.layer != 10)
            {
                
                aggroTarget = target.transform;
                if(target.GetComponent<EnemyUnit>())
                {
                    aggroUnit = target.GetComponentInChildren<UnitStatDisplay>();
                    SetRangedTarget();
                    
                }
                isTargetSetManually = true;
                isAggro = true;
                
                CancelMarch();
                navAgent.SetDestination(aggroTarget.position);
            }
            else if(target.layer == 10) //resource layer
            {
                SetResource(target);
            }
            
        }

        private void MoveToAggroTarget()
        {
            if(aggroTarget == null)
            {
                navAgent.SetDestination(transform.position);
                isAggro = false;
                aggroTarget = null;
                isTargetSetManually = false;
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
                else if(distance > baseStats.aggroRange && aggroTarget && isTargetSetManually == false)
                {
                    navAgent.SetDestination(transform.position);
                    isAggro = false;
                    aggroTarget = null;
                }
            }
        }

        private void Attack()
        {
            if(aggroUnit)
            {
                if(atkCooldown <= 0 && distance <= baseStats.range + 1)
                {
                    if(isMelee())
                    {
                        aggroUnit.TakeDamage(baseStats.attack);
                        atkCooldown = baseStats.attackSpeed;
                    }
                    else
                    {
                        rWeapon.Fire();
    
                        atkCooldown = baseStats.attackSpeed;
                    }
                    
                }
            }
            else if(aggroTarget && aggroTarget.gameObject.layer == 10) //harvest resource
            {
                if(atkCooldown <= 0 && distance <= baseStats.range + 1)
                {
                    aggroTarget.GetComponent<ResourceObject>().GiveResource(baseStats.attack);
                    atkCooldown = baseStats.attackSpeed;
                }
            }
        }

        private void SetResource(GameObject resourceTarget)
        {
            UnitInformation unitInfo = gameObject.GetComponent<IUnit>().unitCard.unitInformation;

            if(unitInfo.type == UnitInformation.unitType.Worker)
            {
                aggroTarget = resourceTarget.transform;
                if(resourceTarget.GetComponent<ResourceObject>())
                {
                    aggroUnit = null;
                }
                isTargetSetManually = true;
                isAggro = true;
                //might break
                CancelMarch();
                navAgent.SetDestination(aggroTarget.position);
            }
        }

        private bool isMelee()
        {
            if(unitInfo.type == UnitInformation.unitType.Archer)
            {
                return false;
            }

            return true;
        }

        private void SetRangedTarget()
        {
            if(isMelee() == false)
            {
                rWeapon.SetTargetToShoot(aggroTarget);
                rWeapon.SetWeaponStats(baseStats.attackSpeed, baseStats.attack);
            }
        }

    }
}

