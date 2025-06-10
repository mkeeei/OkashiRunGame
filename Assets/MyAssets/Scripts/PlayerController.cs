using UnityEngine;
using UnityEngine.SceneManagement; // SceneManagerを使用するために追加

public class PlayerController : MonoBehaviour
{
    [Header("設定")]
    public float runSpeed = 4f;
    public float stopForce = 4f;
    public float jumpForce = 5f;
    public float maxHeight = 7f;
    public bool isDead;

    [Header("コンポーネント")]
    [SerializeField] private Rigidbody2D rb2d;
    [SerializeField] private Animator animator;

    private bool isJumping = false;
    private bool isRunning = false;

    void Update()
    {
        Debug.Log(isDead);

        if (isDead) return;

        Run();
        Jump();
        UpdateAnimator();
    }

    private void Run()
    {
        if (isDead) return;

        float moveInput = Input.GetAxisRaw("Horizontal");

        if (moveInput != 0)
        {
            isRunning = true;
            Vector2 force = new Vector2(moveInput * runSpeed, 0);
            rb2d.AddForce(force);
        }
        else
        {
            isRunning = false;
            Vector2 force = new Vector2(-Mathf.Sign(rb2d.linearVelocity.x) * stopForce, 0);
            rb2d.AddForce(force);
        }

        AlignWithSlope();
    }

    private void Jump()
    {
        if (isDead) return;

        if (Input.GetKeyDown(KeyCode.Space) && transform.position.y < maxHeight)
        {
            Vector2 force = new Vector2(0, jumpForce);
            rb2d.AddForce(force, ForceMode2D.Impulse);
        }
    }

    private void UpdateAnimator()
    {
        if (animator != null)
        {
            animator.SetBool("isRunning", isRunning);
            animator.SetBool("isJumping", rb2d.linearVelocity.y > 0.1f);
            animator.SetBool("isDead", isDead);
        }
    }

    private void AlignWithSlope()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1f);
        if (hit.collider != null && hit.collider.CompareTag("Ground"))
        {
            Vector2 slopeNormal = hit.normal;
            float angle = Vector2.SignedAngle(Vector2.up, slopeNormal);
            rb2d.rotation = angle;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isJumping = false;
        }

        if (collision.collider.CompareTag("Sweets"))
        {
            rb2d.linearVelocity = Vector2.zero;
            animator.SetTrigger("Die");
            isDead = true;

            RestartGame(); // ゲームをリスタートするメソッドを呼び出し

        }
    }

    private void RestartGame()
    {
        // 現在のシーンを再読み込み
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Revive()
    {
        isDead = false;
        rb2d.linearVelocity = Vector2.zero;
        transform.rotation = Quaternion.identity;
        if (animator != null)
        {
            animator.SetTrigger("Revive");
        }
    }
}
