using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Athletic02Manager : MonoBehaviour
{
    public enum GameState { Ready, Play, GameOver, Clear }
    public GameState State { get; private set; }

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI readyText;
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private TextMeshProUGUI lifeText;
    [SerializeField] private TextMeshProUGUI distanceText;

    [Header("Ground スクロール")]
    [SerializeField] private ScrollGround scrollGround;

    [Header("Pocky スポナー")]
    [SerializeField] private PockySpawner pockySpawner;

    [Header("参照")]
    [SerializeField] private PlayerFlappy playerFlappy;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform goalTransform;
    [SerializeField] private MonoBehaviour[] playerControllers;
    [SerializeField] private WolfJumpAction wolfJumpAction;

    [Header("Wolf 侵入演出")]
    [SerializeField] private Transform wolfTransform;        // Scene上のWolfオブジェクト
    [SerializeField] private float wolfStartOffsetX = -2f;// 左端からどれだけ外に置くか
    [SerializeField] private float wolfEndOffsetX = 1f;// 左端からステージ内どこまで来るか
    [SerializeField] private float wolfEntranceDuration = 1.5f; // 秒    

    private void Start()
    {
        SetState(GameState.Ready);
    }

    private void Update()
    {
        switch (State)
        {
            case GameState.Ready:
                if (Input.GetKeyDown(KeyCode.Space))
                    SetState(GameState.Play);

                break;

            case GameState.Play:
                // 残り距離を毎フレーム更新
                if (playerTransform != null && goalTransform != null)
                {
                    float dist = goalTransform.position.x - playerTransform.position.x;
                    dist = Mathf.Max(0f, dist);
                    distanceText.text = $"Distance: {dist:0.0}m";
                }
                break;

            case GameState.GameOver:
                if (Input.GetKeyDown(KeyCode.Return))
                    ReloadScene();
                break;
        }
    }

    public void SetState(GameState newState)
    {
        State = newState;
        switch (newState)
        {
            case GameState.Ready:
                // ゲーム全体は動かす
                Time.timeScale = 1f;

                // プレイヤー操作停止＆重力オフ
                playerFlappy.SetControlActive(false);
                // Wolf のジャンプ停止
                wolfJumpAction.SetActiveJump(false);
                // Groundのスクロール停止
                scrollGround.StopScroll();
                // Pockyの生成停止
                pockySpawner.SetActiveSpawner(false);

                // UI表示切り替え
                readyText.gameObject.SetActive(true);
                gameOverText.gameObject.SetActive(false);
                lifeText.gameObject.SetActive(false);
                distanceText.gameObject.SetActive(false);

                SetPlayerEnabled(false);
                break;

            case GameState.Play:
                Time.timeScale = 1f;

                // プレイヤー操作再開＆重力オン
                playerFlappy.SetControlActive(true);
                // Groundのスクロール開始
                scrollGround.StartScroll();
                // Pockyの生成開始
                pockySpawner.SetActiveSpawner(true);

                // UI 表示切り替え
                readyText.gameObject.SetActive(false);
                gameOverText.gameObject.SetActive(false);
                lifeText.gameObject.SetActive(true);
                distanceText.gameObject.SetActive(true);

                // 初回フラップ
                playerFlappy?.TriggerFlap();
                WolfSequence().Forget();
                break;

            case GameState.GameOver:
                // 画面を止める
                Time.timeScale = 0f;

                // UI 表示切り替え
                readyText.gameObject.SetActive(false);
                gameOverText.gameObject.SetActive(true);
                lifeText.gameObject.SetActive(true);
                distanceText.gameObject.SetActive(true);

                SetPlayerEnabled(false);
                break;

            case GameState.Clear:

                // プレイヤーやその他ギミックを止める
                playerFlappy.SetControlActive(false);
                wolfJumpAction.SetActiveJump(false);
                scrollGround.StopScroll();
                pockySpawner.SetActiveSpawner(false);

                // UI 切り替え
                readyText.gameObject.SetActive(false);
                gameOverText.gameObject.SetActive(false);
                lifeText.gameObject.SetActive(false);
                distanceText.gameObject.SetActive(false);
                break;
        }
    }

    private void SetPlayerEnabled(bool enabled)
    {
        foreach (var ctrl in playerControllers)
            ctrl.enabled = enabled;
    }

    public void OnPlayerDead()
    {
        if (State == GameState.Play)
            SetState(GameState.GameOver);
    }

    public void OnPlayerClear()
    {
        if (State == GameState.Play)
            SetState(GameState.Clear);
    }

    // 侵入アニメが終わるまで待ってから WolfJumpAction を起動
    private async UniTaskVoid WolfSequence()
    {
        // 1) 侵入アニメ
        await WolfEntrance();

        // 2) ジャンプスクリプト起動
        wolfJumpAction.SetActiveJump(true);
    }

    private void ReloadScene()
    {
        // 現在のシーンを再読み込み
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private async UniTask WolfEntrance()
    {
        if (wolfTransform == null) return;

        // 画面左端のワールド X 座標を取得（メインカメラ）
        float leftWorldX = Camera.main.ViewportToWorldPoint(Vector3.zero).x;

        // スタート／エンド位置を計算
        Vector3 startPos = wolfTransform.position;
        startPos.x = leftWorldX + wolfStartOffsetX;
        Vector3 endPos = wolfTransform.position;
        endPos.x = leftWorldX + wolfEndOffsetX;

        wolfTransform.position = startPos;

        float t = 0f;
        while (t < wolfEntranceDuration)
        {
            t += Time.deltaTime;
            float rate = Mathf.Clamp01(t / wolfEntranceDuration);
            wolfTransform.position = Vector3.Lerp(startPos, endPos, rate);
            await UniTask.Yield();
        }
        // 最終位置を保証
        wolfTransform.position = endPos;
    }
}

