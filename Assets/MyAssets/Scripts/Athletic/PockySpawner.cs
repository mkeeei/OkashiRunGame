using System;
using UnityEngine;
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

    [Header("スポーン位置 X (画面右端)")]
    [Tooltip("カメラの右端＋オフセットなど")]
    [SerializeField] private float spawnX = 10f;

    [Header("スポーン Y 範囲")]
    [SerializeField] private float spawnYMin = -3f;
    [SerializeField] private float spawnYMax = 3f;

    private async void Start()
    {
        // 15秒遅延：キャンセル用トークンを渡さない
        await UniTask.Delay(TimeSpan.FromSeconds(startDelay));

        while (gameObject.activeInHierarchy)
        {
            SpawnEnemy();
            float wait = UnityEngine.Random.Range(intervalMin, intervalMax);
            await UniTask.Delay(TimeSpan.FromSeconds(wait));
        }
    }

    private void SpawnEnemy()
    {
        // スポーン位置を計算
        float y = UnityEngine.Random.Range(spawnYMin, spawnYMax);
        Vector3 pos = new Vector3(spawnX, y, 0f);

        // 敵を生成
        Instantiate(enemyPrefab, pos, Quaternion.identity);
    }

    private void OnDrawGizmosSelected()
    {
        // お菓子の出現範囲を表示
        Gizmos.color = Color.yellow;
        // 左→右の範囲を簡易的にラインで可視化
        Vector3 a = new Vector3(spawnX, spawnYMax, 0f);
        Vector3 b = new Vector3(spawnX, spawnYMin, 0f);
        Gizmos.DrawLine(a, a + Vector3.up * 0.5f);
        Gizmos.DrawLine(b, b + Vector3.up * 0.5f);
        Gizmos.DrawLine(a, b);
    }
}
