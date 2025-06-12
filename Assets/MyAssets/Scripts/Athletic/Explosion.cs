using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Explosion : MonoBehaviour
{
    [Header("爆発パラメータ")]
    [SerializeField] private float explosionForce = 10f;
    [SerializeField] private float explosionRadius = 2f;
    [SerializeField] private float upliftModifier = 0.5f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // あたったオブジェクトが Explosionable タグなら発火
        if (!other.CompareTag("Explosionable")) return;

        Vector2 center = (Vector2)transform.position;

        // 半径内の Collider2D を列挙
        Collider2D[] hits = Physics2D.OverlapCircleAll(center, explosionRadius);
        foreach (var col in hits)
        {
            if (!col.CompareTag("Explosionable")) continue;

            Rigidbody2D rb = col.attachedRigidbody;
            if (rb != null)
            {
                // 拡張メソッドで爆風力を加える
                rb.AddExplosionForce(explosionForce, center, explosionRadius, upliftModifier);
            }
        }
    }

    // Sceneビューで爆発半径が見えるように
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
