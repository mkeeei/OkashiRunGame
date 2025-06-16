using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Athletic02Manager : MonoBehaviour
{
    public enum GameState { Ready, Play, GameOver }
    public GameState State { get; private set; }

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI readyText;
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private TextMeshProUGUI lifeText;
    [SerializeField] private TextMeshProUGUI distanceText;

    [Header("参照")]
    [SerializeField] private PlayerFlappy playerFlappy;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform goalTransform;
    [SerializeField] private MonoBehaviour[] playerControllers;
    // プレイヤー移動スクリプト（PlayerFlappy など）をインスペクターでセット

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
                Time.timeScale = 0f;             
                readyText.gameObject.SetActive(true);
                gameOverText.gameObject.SetActive(false);
                lifeText.gameObject.SetActive(false);
                distanceText.gameObject.SetActive(false);
                SetPlayerEnabled(false);
                break;

            case GameState.Play:
                Time.timeScale = 1f;             
                readyText.gameObject.SetActive(false);
                gameOverText.gameObject.SetActive(false);
                lifeText.gameObject.SetActive(true);
                distanceText.gameObject.SetActive(true);
                SetPlayerEnabled(true);

                if (playerFlappy != null)
                    playerFlappy.TriggerFlap();
                break;

            case GameState.GameOver:
                Time.timeScale = 0f;
                readyText.gameObject.SetActive(false);
                gameOverText.gameObject.SetActive(true);
                lifeText.gameObject.SetActive(true);
                distanceText.gameObject.SetActive(true);
                SetPlayerEnabled(false);
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

    private void ReloadScene()
    {
        // 現在のシーンを再読み込み
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
