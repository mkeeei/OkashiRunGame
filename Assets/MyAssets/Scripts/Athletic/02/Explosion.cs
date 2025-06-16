using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Explosion : MonoBehaviour
{
    [Header("爆発パラメータ")]
    [SerializeField] private float explosionForce = 10f;
    [SerializeField] private float explosionRadius = 2f;
    [SerializeField] private float upliftModifier = 0.5f;

    private Collider2D triggerCollider;

    private void Awake()
    {
        // Collider2D をキャッシュ
        triggerCollider = GetComponent<Collider2D>();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        // あたったオブジェクトが Explosionable タグなら発火
        if (!other.CompareTag("Explosionable")) return;

        // Collider.bounds の center.x／min.y を使って足元中心を計算
        Bounds b = triggerCollider.bounds;
        Vector2 center = new Vector2(b.center.x, b.min.y);

        // 半径内の Collider2D を列挙
        Collider2D[] hits = Physics2D.OverlapCircleAll(center, explosionRadius);
        foreach (var col in hits)
        {
            if (!col.CompareTag("Explosionable")) continue;

            Rigidbody2D rb = col.attachedRigidbody;
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                // 拡張メソッドで爆風力を加える
                rb.AddExplosionForce(explosionForce, center, explosionRadius, upliftModifier);
            }
        }
    }

    // Sceneビューで爆発半径が見えるように
    private void OnDrawGizmosSelected()
    {
        if (triggerCollider == null) triggerCollider = GetComponent<Collider2D>();
        Bounds b = triggerCollider.bounds;
        Vector2 center = new Vector2(b.center.x, b.min.y);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(center, explosionRadius);
    }
}
