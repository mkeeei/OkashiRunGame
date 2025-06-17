using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Cysharp.Threading.Tasks;  // UniTask を使う場合

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class PlayerFlappy : MonoBehaviour
{
    [Header("左右移動設定")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("フラップ設定")]
    [SerializeField] private float flapForce = 5f;
    [SerializeField] private float maxFallSpeed = -10f;

    [Header("ライフ設定")]
    [SerializeField] private int maxLives = 3;
    [SerializeField] private float invincibleDuration = 1f;
    [SerializeField] private float blinkInterval = 0.1f;

    [Header("落下時回転速度")] 
    [SerializeField] private float fallRotationSpeed = 360f;

    [Header("移動制限")]
    [SerializeField] private float minX = -8f;
    [SerializeField] private float maxX = 8f;
    [SerializeField] private float minY = -4f;
    [SerializeField] private float maxY = 4f;

    [Header("UI表示")]
    [SerializeField] private TextMeshProUGUI lifeText;  // Life表示用Text

    private int currentLives;
    private float invincibleTimer;
    private bool isAlive = true;
    private bool doFlap;
    private Rigidbody2D rb;
    private float defaultGravity;
    private SpriteRenderer spriteRend;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        defaultGravity = rb.gravityScale; // 起動時の値を保存
        spriteRend = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        currentLives = maxLives;
        invincibleTimer = 0f;
        UpdateLifeUI();
    }

    private void Update()
    {
        if (!isAlive) return;

        // 左右移動
        float h = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(h * moveSpeed, rb.linearVelocity.y);

        // フラップ予約
        if (Input.GetKeyDown(KeyCode.Space))
            doFlap = true;

        // 無敵タイマーを減算
        if (invincibleTimer > 0f)
            invincibleTimer -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (!isAlive) return;

        if (doFlap)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * flapForce, ForceMode2D.Impulse);
            doFlap = false;
        }

        if (rb.linearVelocity.y < maxFallSpeed)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, maxFallSpeed);
    }

    private void LateUpdate()
    {
        // 画面内に位置を制限
        Vector3 p = transform.position;
        p.x = Mathf.Clamp(p.x, minX, maxX);
        p.y = Mathf.Clamp(p.y, minY, maxY);
        transform.position = p;
    }

    /// <summary>
    /// 外部から強制的に一度だけフラップさせたいときに呼ぶ
    /// </summary>
    public void TriggerFlap()
    {
        // 生存チェックなど必要なら入れる
        if (!isAlive) return;
        doFlap = true;
    }

    // Ready/Play 状態切り替え用メソッド
    public void SetControlActive(bool active)
    {
        enabled = active;                     // Update/FixedUpdate の on/off
        rb.gravityScale = active
                          ? defaultGravity   // Play なら元に戻す
                          : 0f;              // Ready は重力なし
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // クリア／ゲームオーバー中は何もしない
        var mgr = FindFirstObjectByType<Athletic02Manager>();
        if (mgr.State != Athletic02Manager.GameState.Play)
            return;

        if (invincibleTimer > 0f) return;

        currentLives--;

        if(currentLives < 0)
        {
            currentLives = 0;
        }

        UpdateLifeUI();

        if (currentLives > 0)
        {
            // 無敵時間をリセット＆点滅開始
            invincibleTimer = invincibleDuration;
            BlinkDuringInvincible().Forget();
        }
        else
        {

            // ライフ０：ひっくり返って落下
            isAlive = false;

            // 速度リセット
            rb.linearVelocity = Vector2.zero;

            // FreezeRotation）を解除
            rb.constraints &= ~RigidbodyConstraints2D.FreezeRotation;

            // 回転速度を設定（deg/sec）
            rb.angularVelocity = fallRotationSpeed;

            // 操作スクリプトをオフ
            enabled = false;

            // 遅延してGameOver通知
            DelayedGameOverAsync().Forget();
        }
    }

    // UI テキスト更新
    private void UpdateLifeUI()
    {
        if (lifeText != null)
            lifeText.text = $"Life:{currentLives}";
    }

    // 無敵時間中だけ点滅する
    private async UniTaskVoid BlinkDuringInvincible()
    {
        var token = this.GetCancellationTokenOnDestroy();
        while (invincibleTimer > 0f && !token.IsCancellationRequested)
        {
            spriteRend.enabled = false;
            await UniTask.Delay(TimeSpan.FromSeconds(blinkInterval), cancellationToken: token);
            spriteRend.enabled = true;
            await UniTask.Delay(TimeSpan.FromSeconds(blinkInterval), cancellationToken: token);
        }
        spriteRend.enabled = true;
    }

    // ひっくり返ってからちょっと待って GameOver を通知
    private async UniTaskVoid DelayedGameOverAsync()
    {
        var token = this.GetCancellationTokenOnDestroy();
        await UniTask.Delay(TimeSpan.FromSeconds(1f), cancellationToken: token);
        var mgr = FindFirstObjectByType<Athletic02Manager>();
        mgr?.OnPlayerDead();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        // 範囲の中心とサイズを計算
        Vector3 center = new Vector3((minX + maxX) * 0.5f, (minY + maxY) * 0.5f, 0f);
        Vector3 size = new Vector3(maxX - minX, maxY - minY, 0f);
        Gizmos.DrawWireCube(center, size);
    }
}
