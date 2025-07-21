using UnityEngine;

public class ShadowFollow : MonoBehaviour
{
    public Transform target; // Karakter utama
    public LayerMask groundLayer; // Layer untuk platform/ground
    public float rayDistance = 10f; // Jarak maksimum raycast ke bawah
    public float offsetY = 0.01f; // Supaya shadow tidak nempel banget ke tanah

    void Update()
    {
        Vector3 origin = target.position;

        // Raycast ke bawah
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, rayDistance, groundLayer);

        if (hit.collider != null)
        {
            Vector3 shadowPos = new Vector3(target.position.x, hit.point.y + offsetY, target.position.z);
            transform.position = shadowPos;
        }
    }

    // Debug line untuk raycast (opsional)
    void OnDrawGizmos()
    {
        if (target != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(target.position, target.position + Vector3.down * rayDistance);
        }
    }
}