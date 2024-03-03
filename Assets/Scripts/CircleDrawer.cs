using UnityEngine;

public class CircleDrawer : MonoBehaviour
{
    TankController tank;
    public float radius = 1f;

    private void Start()
    {
        tank = GetComponent<TankController>();
        radius = tank.Info.attackRange;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
