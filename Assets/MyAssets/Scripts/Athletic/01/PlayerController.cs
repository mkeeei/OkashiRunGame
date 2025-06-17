using Cysharp.Threading.Tasks;
using System.Collections;
using System.Transactions;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement; // SceneManagerを使用するために追加

public class PlayerController : MonoBehaviour
{
    [Header("設定")]
    public float runSpeed = 4f;
    public float stopForce = 4f;
    public float jumpForce = 5f;
    public float maxHeight = 7f;

    [Header("コンポーネント")]
    [SerializeField] private Rigidbody2D rb2d;

    private bool isJumping = false;


    void Update()
    {
        Run();
        Jump();

        if (transform.position.y <= -20f)
        {
            Destroy(gameObject);
            RestartGame(); // ゲームをリスタートするメソッドを呼び出し

        }
    }
    private void Run()
    {

        float moveInput = Input.GetAxisRaw("Horizontal");

        if (moveInput != 0)
        {
            Vector2 force = new Vector2(moveInput * runSpeed, 0);
            rb2d.AddForce(force);
        }
        else
        {
            Vector2 force = new Vector2(-Mathf.Sign(rb2d.linearVelocity.x) * stopForce, 0);
            rb2d.AddForce(force);
        }

    }
    private void Jump()
    {

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
    }

    private void RestartGame()
    {
        // 現在のシーンを再読み込み
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
