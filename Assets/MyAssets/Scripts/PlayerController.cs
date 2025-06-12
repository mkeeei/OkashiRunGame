using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement; // SceneManagerを使用するために追加

public class PlayerController : MonoBehaviour
{
    [Header("設定")]
    public float runSpeed = 4f;
    public float stopForce = 4f;
    public float jumpForce = 5f;
    public float maxHeight = 7f;
    GameObject player;

    [Header("コンポーネント")]
    [SerializeField] private Rigidbody2D rb2d;
    [SerializeField] private Animator animator;

    private bool isJumping = false;
    private bool isRunning = false;
    private bool isDead = false;

    private void Start()
    {
        this.player = GameObject.Find("Sheep01_0");
    }

    void Update()
    {
        //if (isDead) return;

        Run();
        Jump();

        if (transform.position.y <= -5f)
        {
            Destroy(gameObject);
            RestartGame(); // ゲームをリスタートするメソッドを呼び出し

        }

    }

    private void Run()
    {
        //if (isDead) return;

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

    }

    private void Jump()
    {
        //if (isDead) return;

        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            Vector2 force = new Vector2(0, jumpForce);
            rb2d.AddForce(force, ForceMode2D.Impulse);

            isJumping = true;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isJumping = false;
        }

        //if (collision.collider.CompareTag("Sweets"))
        //{
        //    rb2d.linearVelocity = Vector2.zero;
        //    //animator.SetTrigger("Die");
        //    isDead = true;

        //    RestartGame(); // ゲームをリスタートするメソッドを呼び出し

        //}
    }
    //public void Die()
    //{
    //    Debug.Log("死にました");
    //    isDead = true;
    //    rb2d.linearVelocity = Vector2.zero; // 速度をゼロにして停止
    //    //if (animator != null)
    //    //{
    //    //    animator.SetTrigger("Die");
    //    //}
    //}

    private void RestartGame()
    {
        // 現在のシーンを再読み込み
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //public void Revive()
    //{
    //    isDead = false;
    //    rb2d.linearVelocity = Vector2.zero;
    //    transform.rotation = Quaternion.identity;
    //    if (animator != null)
    //    {
    //        animator.SetTrigger("Revive");
    //    }
    //}
}
