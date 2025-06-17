using System;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

[RequireComponent(typeof(Collider2D))]
public class WolfJumpAction : MonoBehaviour
{
    [Header("ジャンプ設定")]
    [SerializeField] private float minInterval = 1f;   // 待機時間の最小
    [SerializeField] private float maxInterval = 3f;   // 待機時間の最大
    [SerializeField] private float jumpPower = 2f;   // ジャンプ力
    [SerializeField] private int numJumps = 1;    // ジャンプ回数
    [SerializeField] private float jumpDuration = 0.6f; // ジャンプ所要時間

    [Header("カメラシェイク設定")]
    [SerializeField] private Transform cameraTransform;   // 揺らすカメラの Transform
    [SerializeField] private float shakeDuration = 0.5f;
    [SerializeField] private float shakeStrength = 0.3f;
    [SerializeField] private int shakeVibrato = 10;

    [Header("お菓子出現設定")]
    [Tooltip("複数のお菓子プレハブを登録できます")]
    [SerializeField] private GameObject[] okashiPrefabs;
    [Tooltip("お菓子の X 座標出現範囲：最小値")]
    [SerializeField] private float spawnXMin = -5f;
    [Tooltip("お菓子の X 座標出現範囲：最大値")]
    [SerializeField] private float spawnXMax = 5f;
    [Tooltip("お菓子の Y 座標固定値")]
    [SerializeField] private float spawnY = 6f;


    private void Start()
    {
        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;
    }

    // Manager から呼ばれる
    public void SetActiveJump(bool active)
    {
        enabled = active;
        if (active)
        {
            // Play 開始時点で初回 JumpLoop を走らせる
            JumpLoop().Forget();
        }
    }

    private async UniTask JumpLoop()
    {
        while (enabled && gameObject.activeInHierarchy)
        {
            // ランダム間隔で待機
            float waitSec = UnityEngine.Random.Range(minInterval, maxInterval);
            await UniTask.Delay(TimeSpan.FromSeconds(waitSec));

            // その場ジャンプ
            await transform
                .DOJump(transform.position, jumpPower, numJumps, jumpDuration)
                .AsyncWaitForCompletion();

            // 着地時の演出
            DoScreenShake();
            SpawnOkashi();
        }
    }

    private void DoScreenShake()
    {
        if (cameraTransform == null) return;
        cameraTransform
            .DOShakePosition(shakeDuration, shakeStrength, shakeVibrato);
    }

    private void SpawnOkashi()
    {
        if (okashiPrefabs == null || okashiPrefabs.Length == 0)
        {
            Debug.LogWarning("お菓子プレハブが設定されていません。");
            return;
        }

        // 登録されたプレハブからランダムにひとつを選択
        var prefab = okashiPrefabs[UnityEngine.Random.Range(0, okashiPrefabs.Length)];

        // 指定された範囲内でランダム X
        float x = UnityEngine.Random.Range(spawnXMin, spawnXMax);
        Vector3 pos = new Vector3(x, spawnY, 0f);

        Instantiate(prefab, pos, Quaternion.identity);
       
    }

    private void OnDrawGizmosSelected()
    {
        // お菓子の出現範囲を表示
        Gizmos.color = Color.yellow;
        // 左→右の範囲を簡易的にラインで可視化
        Vector3 a = new Vector3(spawnXMin, spawnY, 0f);
        Vector3 b = new Vector3(spawnXMax, spawnY, 0f);
        Gizmos.DrawLine(a, a + Vector3.up * 0.5f);
        Gizmos.DrawLine(b, b + Vector3.up * 0.5f);
        Gizmos.DrawLine(a, b);
    }
}
