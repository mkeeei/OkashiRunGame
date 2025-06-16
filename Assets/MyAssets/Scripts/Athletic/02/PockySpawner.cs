using System;
using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;

public class PockySpawner : MonoBehaviour
{
    [Header("敵プレハブ")]
    [SerializeField] private GameObject enemyPrefab;

    [Header("スポーン開始の遅延")]
    [SerializeField] private float startDelay = 15f;

    [Header("スポーン間隔 (秒)")]
    [SerializeField] private float intervalMin = 2f;
    [SerializeField] private float intervalMax = 4f;

    [Header("スポーン位置 X (画面外右端)")]
    [SerializeField] private float spawnX = 10f;

    [Header("スポーン Y 範囲")]
    [SerializeField] private float spawnYMin = -3f;
    [SerializeField] private float spawnYMax = 3f;

    [Header("Warning UI")]
    [SerializeField] private Canvas uiCanvas;            // Screen Space Overlay 推奨
    [SerializeField] private TextMeshProUGUI warningPrefab;       // Inspector で Prefab 化しておく
    [SerializeField] private float warningBlinkInterval = 0.2f;
    [SerializeField] private float warningDuration = 1f;
    [SerializeField] private float warningOffsetX = -50f; // 画面端から内側へのオフセット

    private bool isSpawning = false;

    /// <summary>
    /// Athletic02Manager から Ready/Play/GameOver に応じて呼び出す
    /// </summary>
    public void SetActiveSpawner(bool active)
    {
        isSpawning = active;
        if (active)
        {
            // Play に入った瞬間だけ StartLoop を呼ぶ
            SpawnLoop().Forget();
        }
    }

    // メインのループ
    private async UniTaskVoid SpawnLoop()
    {
        // 1) 初回遅延
        await UniTask.Delay(TimeSpan.FromSeconds(startDelay));

        // 2) ループ
        while (isSpawning && gameObject.activeInHierarchy)
        {
            // 次のスポーン Y を決定
            float y = UnityEngine.Random.Range(spawnYMin, spawnYMax);

            // Warning 表示＆完了待ち
            await ShowWarningAtWorldY(y);

            // 敵を生成
            Vector3 spawnPos = new Vector3(spawnX, y, 0f);
            Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

            // 次の間隔まで待機
            float wait = UnityEngine.Random.Range(intervalMin, intervalMax);
            await UniTask.Delay(TimeSpan.FromSeconds(wait));
        }
    }

    /// <summary>
    /// 敵の spawnY と同じ高さ worldY で画面右端に Warning を表示
    /// </summary>
    private async UniTask ShowWarningAtWorldY(float worldY)
    {
        // スクリーン座標に変換
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(new Vector3(0, worldY, 0));
        screenPoint.x = Screen.width + warningOffsetX;

        // Canvas ローカル座標に変換
        RectTransform canvasRt = uiCanvas.GetComponent<RectTransform>();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRt, screenPoint, uiCanvas.worldCamera, out Vector2 localPos);

        // テキストのインスタンスを生成して位置セット
        var warning = Instantiate(warningPrefab, uiCanvas.transform);
        warning.rectTransform.anchoredPosition = localPos;
        warning.gameObject.SetActive(true);

        // 点滅ループ
        float timer = 0f;
        bool visible = true;
        while (timer < warningDuration)
        {
            visible = !visible;
            warning.enabled = visible;
            await UniTask.Delay(TimeSpan.FromSeconds(warningBlinkInterval));
            timer += warningBlinkInterval;
        }

        // 終わったら消す
        Destroy(warning.gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 a = new Vector3(spawnX, spawnYMax, 0f);
        Vector3 b = new Vector3(spawnX, spawnYMin, 0f);
        Gizmos.DrawLine(a, a + Vector3.up * 0.5f);
        Gizmos.DrawLine(b, b + Vector3.up * 0.5f);
        Gizmos.DrawLine(a, b);
    }
}
