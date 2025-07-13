using RTS.Player;
using UnityEngine;

namespace RTS.Unit.Projectile
{
    public class Bow : MonoBehaviour
    {
        [SerializeField]
    private float damage, reloadTime, firePower = 100f;

    private Vector3 velocity;

    [SerializeField] private Rigidbody arrowPrefab;

    [SerializeField] private Transform spawnPoint;
    
    [SerializeField] private PlayerUnit owner;

    private Projecile currentArrow;

    private Transform target;

    private bool isReloading = false;

    private void Awake()
    {
        spawnPoint = transform.parent;
    }
    public void SetTargetToShoot(Transform t)
    {
        target = t;
        
        //velocity  = CalculateVelocity(target.position, transform.position, 3);

        //transform.rotation = Quaternion.LookRotation(velocity);
    }

    public void SetWeaponStats(float cd, float dmg)
    {
        reloadTime = cd;
        damage = dmg;
    }

    public void Fire()
    {
        
        Projecile arrow = Instantiate(arrowPrefab.gameObject, spawnPoint.position, spawnPoint.rotation).GetComponent<Projecile>();

        arrow.LaunchAt(target, damage, GetComponentInParent<PlayerUnit>());

    }

    public bool IsReady()
    {
        return (!isReloading && currentArrow != null);
    }

    Vector3 CalculateVelocity(Vector3 target, Vector3 origin, float time)
    {
        //define the distance x and y first
        Vector3 distance = target - origin;
        Vector3 distance_x_z = distance;
        distance_x_z.Normalize();
        distance_x_z.y = 0;

        //creating a float that represents our distance 
        float sy = distance.y;
        float sxz = distance.magnitude;


        //calculating initial x velocity
        //Vx = x / t
        float Vxz = sxz / time;

        ////calculating initial y velocity
        //Vy0 = y/t + 1/2 * g * t
        float Vy = sy / time + 0.5f * Mathf.Abs(Physics.gravity.y) * time;

        Vector3 result = distance_x_z * Vxz;
        result.y = Vy;



        return result;
    }   

    }
}
