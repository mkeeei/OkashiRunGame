using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb2d;
    public float runVelocity = 5f;
    public float jumpForce = 5f;
    private bool isDead = false;
    private bool isGrounded = false;
    private bool isJumping = false; // ジャンプ中かどうかを判定するフラグ

    void Update()
    {
        if (isDead) return;

        // 接地判定
        isGrounded = IsGrounded();

        // ジャンプ処理
        if (isGrounded && Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            Jump();
        }

        // 走行処理
        if (Input.GetKey(KeyCode.RightArrow))
        {
            Run();
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            Behind();
        }
        else
        {
            StopRunning();
        }
    }

    private bool IsGrounded()
    {
        // プレイヤーの位置から下方向にRayを飛ばして接地判定
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f);
        return hit.collider != null;
    }

    private void Jump()
    {
        // Y軸の速度をリセットしてジャンプ
        //rb2d.linearVelocity = new Vector2(rb2d.linearVelocity.x, 0);
        rb2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        isJumping = true; // ジャンプ中フラグを立てる
    }

    private void Run()
    {
        if (animator != null)
        {
            animator.SetBool("isRunning", isRunning);
            animator.SetBool("isJumping", rb2d.linearVelocity.y > 0.1f);
            animator.SetBool("isDead", isDead);
        }
        // X軸方向に移動
        float moveInput = Input.GetAxis("Horizontal"); // 入力を取得
        Vector2 force = new Vector2(moveInput * runVelocity, 0); // X軸方向の力を計算
        rb2d.AddForce(force); // 力を加える
    }

    private void Behind()
    {
        // -X方向に後ずさり
        float moveInput = Input.GetAxis("Horizontal"); // 入力を取得
        Vector2 force = new Vector2(moveInput * -runVelocity, 0); // X軸方向の力を計算
        rb2d.AddForce(force); // 力を加える
    }

    private void StopRunning()
    {
        // X軸方向の速度をゼロにして停止
        //rb2d.linearVelocity = new Vector2(0, rb2d.linearVelocity.y);
        float moveInput = Input.GetAxis("Horizontal"); // 入力を取得
        Vector2 force = new Vector2(0, moveInput * runVelocity); // X軸方向の力を計算
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isJumping = false; // 地面と接触したらジャンプ中フラグをリセット
        }
    }

    public void Die()
    {
        isDead = true;
        rb2d.linearVelocity = Vector2.zero; // 速度をゼロにして停止
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }
    }

    public void Revive()
    {
        isDead = false;
    }
}
