using DG.Tweening;
using R3;
using Unity.Cinemachine;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class MiniGame02Manager : MonoBehaviour
{
    [SerializeField] CinemachineCamera _cinemaCamera; // Cinemachineカメラコンポーネント。
    [SerializeField] Transform _playerTransform; // 主人公のTransform。
    [SerializeField] MiniGame02_PlayerController _pcController; // プレイヤーコントローラーのスクリプト。
    [SerializeField] Transform _StartPoint; // プレイヤーの開始位置を格納するTransform。
    [SerializeField] MiniGame02BossManager _bossManager; // ボスエネミーの管理を行うスクリプト。
    [SerializeField] MiniGame02Demo _demo; // デモの管理を行うスクリプト。

    [Header("アイテム配置")]
    [SerializeField] GameObject[] _itemSpawnPoint; // アイテムのスポーンポイントを格納する変数。

    void Start()
    {
        //// 初期化処理を呼び出す。
        //Initialize();

        // ボスのHPを購読する。
        _bossManager._bossHealth.Subscribe(health =>
        {
            // ボスのHPが変動するたびに、アイテムの配置を更新する。
            UpdateItemPlacement();
        });

        // デモ演出の開始。
        _demo.DemoInitialize();
    }


    // 初期化メソッド。
    public void Initialize()
    {
        SetFollowTarget(_playerTransform); // カメラ位置の初期化。
        _pcController.PlayerInitialize(); // プレイヤーコントローラーの初期化。
        UpdateItemPlacement(); // アイテムの初期配置を行う。
        _playerTransform.position = _StartPoint.position; // プレイヤーの開始位置を設定。
    }

    // 主人公にカメラを追従させるメソッド。。
    public void SetFollowTarget(Transform target)
    {
        if (_cinemaCamera != null)
        {
            _cinemaCamera.Follow = target;
        }
        else
        {
            Debug.LogWarning("CinemachineCamera is not assigned.");
        }
    }

    // 主人公をノックバックさせる処理。
    public void KnockBackPlayer()
    {
        // プレイヤーを移動不能に。
        _pcController.ToggleCanMove(false);

        // プレイヤーを上方へ吹き飛ばす。
        _pcController.GetComponent<Rigidbody2D>()
            .transform.DOMove(new Vector2(-1, 1) * 15f, 1.5f)
            .SetEase(Ease.OutQuad)
            .SetRelative(true)
            .OnComplete(()=>
            {
                // 主人公を初期位置に戻す。
                _playerTransform.position = _StartPoint.position; 

                // 主人公の重力をいったん無効化して待機。
                _pcController.GetComponent<Rigidbody2D>().gravityScale = 0f;
            });
    }

    

    // ボスの残り体力に応じてアイテムの再配置を行うメソッド。
    public void UpdateItemPlacement()
    {
        // アイテムスポーンポイントを全て無効化する。
        foreach (var spawnPoint in _itemSpawnPoint)
        {
            spawnPoint.SetActive(false);
        }

        // ボスの残り体力に応じてアイテムの配置を行う。
        switch (_bossManager._bossHealth.Value)
        {
            case 3:
                // ボスのHPが3のときのアイテム配置処理。
                Debug.Log("Boss Health is 3, placing items accordingly.");
                _itemSpawnPoint[0].SetActive(true); // アイテムスポーンポイントを有効化。
                break;
            case 2:
                // ボスのHPが2のときのアイテム配置処理。
                Debug.Log("Boss Health is 2, placing items accordingly.");
                _itemSpawnPoint[1].SetActive(true); // アイテムスポーンポイントを有効化。
                break;
            case 1:
                // ボスのHPが1のときのアイテム配置処理。
                Debug.Log("Boss Health is 1, placing items accordingly.");
                _itemSpawnPoint[2].SetActive(true); // アイテムスポーンポイントを有効化
                break;
            default:
                Debug.Log("Boss Health is 0 or less, no items to place.");
                break;
        }
    }
}
