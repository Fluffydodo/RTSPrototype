using RTS.Player;
using UnityEngine;

namespace RTS.Unit.Projectile
{
    public class Projecile : MonoBehaviour
    {
        public enum ProjectileFlyModes
    {
        PhysicalArc,
        Tracking,
    }
    private struct LaunchData
    {
        public readonly Vector3 initialVelocity;
        public readonly float timeToTarget;

        public LaunchData(Vector3 startVelocity, float time)
        {
            initialVelocity = startVelocity;
            timeToTarget = time;
        }
    }
    public enum ProjectileRotationModes
    {
        PointForwardAlongVelocity,
        SpinArroundX,
        None,
    }
    public ProjectileFlyModes projectileFlyMode;
    public float maxCurveHeight = 25f;
    public float trackSpeed = 10;
    public ProjectileRotationModes projectileRotationMode;
    public bool useFactionMaterial;

    private Transform targetObject;
    private Vector3 targetPosition;
    private float damage;
    private PlayerUnit owner;
    private bool hitSuccess;

    private Rigidbody rigid;
    private Renderer render;
    private float launchTime;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        if (projectileFlyMode == ProjectileFlyModes.PhysicalArc && rigid == null)
        {
            Debug.LogError("A kinematik projectile needs a Rigidbody", this);
            rigid = gameObject.AddComponent<Rigidbody>();
        }
        render = GetComponentInChildren<Renderer>();
    }

    void Update()
    {
        if (projectileFlyMode == ProjectileFlyModes.Tracking)
        {
            UpdateTrackStep();
        }
        AdjustAngleToRotationMode();
    }


    void OnTriggerEnter(Collider other)
    {
        if (projectileFlyMode == ProjectileFlyModes.Tracking && other.transform != targetObject)
        {
            return;
        }
        if (projectileFlyMode == ProjectileFlyModes.PhysicalArc && other.gameObject.layer == LayerMask.NameToLayer("Terrain"))
        {
            Destroy(gameObject);
            return;
        }

        hitSuccess = TryHitUnit(other.transform);

        if (hitSuccess)
        {
            Destroy(gameObject);
            return;
        }
        Debug.LogError("A projectile did not have an action", gameObject);
    }
    /*
    void OnDestroy()
    {
        if (terrainImpactEffect != null && !hitSuccess)
        {
            terrainImpactEffect.transform.SetParent(null);
            terrainImpactEffect.transform.rotation = Quaternion.Euler(Vector3.right * -90f);
            terrainImpactEffect.Play();
            Destroy(terrainImpactEffect.gameObject, terrainImpactEffect.main.duration);
        }
    }
    */

    void OnDrawGizmos()//Selected()
    {
        if (rigid == null)
        {
            return;
        }
        Gizmos.color = Color.Lerp(Color.yellow, Color.red, 0.8f);
        switch (projectileFlyMode)
        {
            case ProjectileFlyModes.PhysicalArc:
                LaunchData launchData = CalculateLaunchData();
                Vector3 previosPoint = transform.position;
                const int resolution = 30;
                for (int i = 1; i <= resolution; i++)
                {
                    float simulationTime = i / (float)resolution * (launchData.timeToTarget - (Time.time - launchTime));
                    Vector3 displacement = rigid.velocity * simulationTime + Physics.gravity * Mathf.Pow(simulationTime, 2f) / 2f;
                    Vector3 currentPoint = transform.position + displacement;
                    if (Vector3.Distance(previosPoint, targetPosition) < Vector3.Distance(previosPoint, currentPoint))
                    {
                        Gizmos.DrawLine(previosPoint, targetPosition);
                        break;
                    }
                    Gizmos.DrawLine(previosPoint, currentPoint);
                    previosPoint = currentPoint;
                }
                break;
            case ProjectileFlyModes.Tracking:
                Gizmos.DrawLine(transform.position, targetObject.position);
                break;
        }
    }

    public void LaunchAt(Transform target, float hitDamage, PlayerUnit firedBy)
    {
        targetObject = target;
        targetPosition = target.position;
        damage = hitDamage;
        owner = firedBy;
        launchTime = Time.time;

        /*
        if (useFactionMaterial)
        {
            UpdateMaterialTeamColor();
        }
        */

        switch (projectileFlyMode)
        {
            case ProjectileFlyModes.PhysicalArc:
                LaunchWithPhysics();
                break;
            case ProjectileFlyModes.Tracking:
                LaunchWithTracking();
                break;
        }
    }

    private void LaunchWithPhysics()
    {
        rigid.velocity = CalculateLaunchData().initialVelocity;
        rigid.isKinematic = false;
        if (projectileRotationMode == ProjectileRotationModes.SpinArroundX)
        {
            rigid.AddRelativeTorque(Vector3.right * 10000f);
        }
    }

    private LaunchData CalculateLaunchData()
    {
        float gravity = Physics.gravity.y;
        float displacementY = targetPosition.y - transform.position.y;
        Vector3 displacementXZ = (targetPosition - transform.position).ToWithY(0f);
        float time = Mathf.Sqrt(-2f * maxCurveHeight / gravity) + Mathf.Sqrt(2f * (displacementY - maxCurveHeight) / gravity);
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2f * gravity * maxCurveHeight);
        Vector3 velocityXZ = displacementXZ / time;
        return new LaunchData(velocityXZ + velocityY * -Mathf.Sign(gravity), time);
    }

    private void AdjustAngleToRotationMode()
    {
        switch (projectileRotationMode)
        {
            case ProjectileRotationModes.PointForwardAlongVelocity:
                switch (projectileFlyMode)
                {
                    case ProjectileFlyModes.PhysicalArc:
                        transform.rotation = Quaternion.LookRotation(rigid.velocity.normalized, Vector3.up);
                        break;
                    case ProjectileFlyModes.Tracking:
                        Vector3 distanceVector = (targetObject.position - transform.position);
                        if (distanceVector.normalized.magnitude > 0f)
                        {
                            transform.rotation = Quaternion.LookRotation(distanceVector.normalized, Vector3.forward);
                        }
                        break;
                }
                break;
            case ProjectileRotationModes.SpinArroundX:
                if (projectileFlyMode != ProjectileFlyModes.PhysicalArc)
                {
                    transform.Rotate(Vector3.right, Time.deltaTime * 100f);
                }
                break;
            case ProjectileRotationModes.None:
                break;
        }
    }

    /*
    protected void UpdateMaterialTeamColor()
    {
        Shader teamcolorShader = GameManager.Instance.tintShader;
        if (teamcolorShader == null)
        {
            throw new System.NullReferenceException();
        }
        UIManager uiManager = UIManager.Instance;

        for (int i = 0; i < render.sharedMaterials.Length; i++)
        {
            if (render.sharedMaterials[i] == null)
            {
                Debug.LogError(string.Concat("Material missing on ", render.gameObject.name), render);
                continue;
            }
            if (render.sharedMaterials[i].shader == teamcolorShader)
            {
                Color tmpColor = uiManager.GetFactionColorForColorMode(owner.faction, UIManager.ColorType.Shader);
                FactionTemplate.ChangeTeamcolorOnRenderer(render, tmpColor, teamcolorShader);
                break;
            }
        }
    }
    */

    private void LaunchWithTracking()
    {
        rigid.isKinematic = true;
    }

    private void UpdateTrackStep()
    {
        transform.position =
            Vector3.MoveTowards(transform.position, targetObject.position, Time.deltaTime * trackSpeed);
        if (!(Vector3.Distance(transform.position, targetObject.position) < 0.001f)) return;
        hitSuccess = TryHitUnit(targetObject);
        Destroy(gameObject);
    }

    private bool TryHitUnit(Transform target)
    {
        UnitStatDisplay statDisplay = target.GetComponentInChildren<UnitStatDisplay>();
        if (statDisplay != null && target != this)
        {
            statDisplay.TakeDamage(damage);
            return true;
        }
        return false;
    }
    }
}
