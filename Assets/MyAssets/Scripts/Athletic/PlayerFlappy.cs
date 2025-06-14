using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerFlappy : MonoBehaviour
{
    [Header("左右移動設定")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("フラップ（スペース）設定")]
    [SerializeField] private float flapForce = 5f;
    [SerializeField] private float maxFallSpeed = -10f;

    [Header("ライフ設定")]
    [SerializeField] private int maxLives = 3;
    [SerializeField] private float invincibleDuration = 1f;

    private int currentLives;
    private float invincibleTimer;
    private bool isAlive = true;
    private bool doFlap;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // Rigidbody2D の Gravity Scale は Inspector で 1 以上にしておいてください
    }

    private void Start()
    {
        currentLives = maxLives;
        invincibleTimer = 0f;
    }

    private void Update()
    {
        // ─── ライフ０（ゲームオーバー）なら「Enter」でリロード待ち ───
        if (!isAlive)
        {
            if (Input.GetKeyDown(KeyCode.Return))
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            return;
        }

        // ─── 左右移動 ───
        float h = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(h * moveSpeed, rb.linearVelocity.y);

        // ─── フラップ予約 ───
        if (Input.GetKeyDown(KeyCode.Space))
            doFlap = true;

        // ─── 無敵タイマー経過 ───
        if (invincibleTimer > 0f)
            invincibleTimer -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (!isAlive) return;

        // ─── フラップ実行 ───
        if (doFlap)
        {
            // Y速度リセットしてからインパルス
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * flapForce, ForceMode2D.Impulse);
            doFlap = false;
        }

        // ─── 落下速度制限 ───
        if (rb.linearVelocity.y < maxFallSpeed)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, maxFallSpeed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isAlive) return;

        // ─── 無敵時間中は無視 ───
        if (invincibleTimer > 0f) return;

        // ─── ライフ減少 ───
        currentLives--;
        if (currentLives > 0)
        {
            // 無敵時間リセット
            invincibleTimer = invincibleDuration;
        }
        else
        {
            // ─── ライフ０：ゲームオーバー処理 ───
            isAlive = false;

            // コントロール停止
            rb.linearVelocity = Vector2.zero;
            enabled = false;  // このスクリプトを停止して Update/FixedUpdate を止める

            // 180° 回転してひっくり返す
            transform.Rotate(0f, 0f, 180f);

            // そのまま自然落下するよう Gravity Scale はそのままに
        }
    }
}
