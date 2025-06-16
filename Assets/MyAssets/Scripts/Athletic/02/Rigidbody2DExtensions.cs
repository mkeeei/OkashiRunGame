using UnityEngine;

public static class Rigidbody2DExtensions
{
    public static void AddExplosionForce(this Rigidbody2D rb,
                                         float explosionForce,
                                         Vector2 explosionPosition,
                                         float explosionRadius,
                                         float upliftModifier = 0f)
    {
        Vector2 dir = rb.position - explosionPosition;
        float dist = dir.magnitude;
        if (dist > explosionRadius || dist <= 0f) return;

        float forceFactor = 1 - (dist / explosionRadius);
        dir.Normalize();
        Vector2 force = dir * explosionForce * forceFactor;

        if (upliftModifier != 0f)
            force += Vector2.up * explosionForce * upliftModifier * forceFactor;

        rb.AddForce(force, ForceMode2D.Impulse);
    }
}
