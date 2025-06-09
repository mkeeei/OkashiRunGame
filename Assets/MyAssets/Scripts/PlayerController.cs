//using Unity.VisualScripting;
//using UnityEngine;

//public class PlayerController : MonoBehaviour
//{
//    bool isDead;
//    float angle;

//    public float maxHeight;
//    public float runVelocity;
//    public float jumpVelocity;
//    public float relativeVelocityX;
//    [SerializeField] Rigidbody2D rb2d;

//    void Start()
//    {

//    }

//    public bool IsDead()
//    {
//        return isDead;
//    }

//    void Update()
//    {
//        // 最高高度に達していない場合、タップの入力を受け付ける
//        if (Input.GetKeyDown(KeyCode.Space) && transform.position.y < maxHeight)
//        {
//            Jump();
//        }
//        else if (Input.GetKeyDown(KeyCode.LeftArrow))
//        {
//            Run();
//        }
//        // 角度を反映
//        ApplyAngle();

//        // angleが水平以上だったら、アニメーターのJumpフラグをtrueにする
//        //Animator.SetBool("jump", angle >= 0.0f && !isDead);

//    }

//    void ApplyAngle()
//    {
//        // 現在の速度、相対速度から進んでいる角度を求める
//        float targetAngle = Mathf.Atan2(rb2d.velocity.y, relativeVelocityX) * Mathf.Rad2Deg;

//        // 死亡したら常にヒックリ返る
//        if (isDead)
//        {
//            targetAngle = 180.0f;
//        }
//        else
//        {
//            targetAngle =
//                Mathf.Atan2(rb2d.linearVelocity.y, relativeVelocityX) * Mathf.Rad2Deg;
//        }
//    }
//        public void Run()
//    {
//        // 死んだら走れない
//        if (!isDead) return;

//        // Velocityを直接書き換えてX方向に加速
//        rb2d.velocity = new Vector2(runVelocity, rb2d.velocity.y);
//    }
//        public void Jump()
//        {
//            // 死んだらジャンプできない
//            if (isDead) return;

//            // Velocity
//            rb2d.linearVelocity = new Vector2(0, jumpVelocity);
//        }

//}

using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("設定")]
    public float runSpeed = 5f;          // 走る速度
    public float jumpForce = 10f;        // ジャンプの力
    public float maxHeight = 10f;        // 最大高さ
    public bool isDead = false;          // 死亡フラグ

    [Header("コンポーネント")]
    [SerializeField] private Rigidbody2D rb2d;  // Rigidbody2Dコンポーネント
    [SerializeField] private Animator animator; // Animatorコンポーネント

    private bool isRunning = false;      // 走っているかどうか

    void Update()
    {
        if (isDead) return; // 死亡している場合、移動処理を行わない

        Run();
        Jump();
        UpdateAnimator();
    }

    private void Run()
    {
        float moveInput = Input.GetAxisRaw("Horizontal"); // 左右の入力

        if (moveInput != 0)
        {
            isRunning = true;
            rb2d.linearVelocity = new Vector2(moveInput * runSpeed, rb2d.linearVelocity.y); // 水平移動
            FlipCharacter(moveInput); // キャラクターの向きを反転
        }
        else
        {
            isRunning = false;
            rb2d.linearVelocity = new Vector2(0, rb2d.linearVelocity.y); // 停止
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && transform.position.y < maxHeight)
        {
            rb2d.linearVelocity = new Vector2(rb2d.linearVelocity.x, jumpForce); // ジャンプ
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

    private void FlipCharacter(float moveInput)
    {
        if (moveInput > 0)
            transform.localScale = new Vector3(1, 1, 1); // 右向き
        else if (moveInput < 0)
            transform.localScale = new Vector3(-1, 1, 1); // 左向き
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
