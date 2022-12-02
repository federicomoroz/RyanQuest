using UnityEngine;

public static class ExtensionMethod
{
    public static void Knockback(this Rigidbody rb, Vector3 other, float impactForce = 100f)
    {
        rb.velocity = Vector3.zero;
        Vector3 dir = (rb.position - other).normalized;        
        rb.AddForce(dir * impactForce, ForceMode.Impulse);
        rb.AddForce(Vector3.up * impactForce * 0.33f, ForceMode.Impulse);      
    }

    public static void VelocityFreezeY(this Rigidbody rb)
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
    }
    public static void Warp(this Transform transform, Vector3 position)
    {
        transform.position = position;        
    }
    public static void Warp(this Transform transform, Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;
    }

    public static void Warp(this Rigidbody rb, Vector3 position)
    {
        rb.position = position;        
    }

    public static void Warp(this Rigidbody rb, Vector3 position, Quaternion rotation)
    {
        rb.position = position;
        rb.rotation = rotation;
    }
}
