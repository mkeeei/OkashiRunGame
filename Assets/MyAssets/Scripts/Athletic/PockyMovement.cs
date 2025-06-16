using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PockyMovement : MonoBehaviour
{
    [Header("移動速度")]
    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody2D rb;
    private bool hasTouched = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // 最初は重力無効
        rb.gravityScale = 0f;
    }

    private void Start()
    {
        // 左向きに水平に動かし続ける
        rb.linearVelocity = new Vector2(-moveSpeed, 0f);
    }

    // 通常の物理コライダーに触れたとき
    private void OnCollisionEnter2D(Collision2D collision)
    {
        EnableGravity();
    }

    // 一度だけ呼んで gravityScale を 1 に戻す
    private void EnableGravity()
    {
        if (hasTouched) return;
        hasTouched = true;
        rb.gravityScale = 1f;
        Destroy(gameObject, 1f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        EnableGravity();
    }
}
