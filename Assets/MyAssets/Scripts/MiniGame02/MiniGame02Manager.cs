using Cysharp.Threading.Tasks;
using DG.Tweening;
using R3;
using Unity.Cinemachine;
using UnityEngine;

public class MiniGame02Manager : MonoBehaviour
{
    // カメラ参照
    [Header("Camera References")]
    [Tooltip("Cinemachineカメラコンポーネント")]
    [SerializeField] private CinemachineCamera _cinemaCamera; // CinemachineVirtualCameraが適切かご確認ください

    // オブジェクト参照
    [Header("Object References")]
    [Tooltip("主人公のTransform")]
    [SerializeField] private Transform _playerTransform;
    [Tooltip("プレイヤーの開始位置")]
    [SerializeField] private Transform _StartPoint;
    [Tooltip("プレイヤーが吹き飛ぶ位置")]
    [SerializeField] private Transform _playerButtobiPoint;

    // スクリプト参照
    [Header("Script References")]
    [Tooltip("プレイヤーコントローラーのスクリプト")]
    [SerializeField] private MiniGame02_PlayerController _pcController;
    [Tooltip("ボスエネミーの管理を行うスクリプト")]
    [SerializeField] private MiniGame02BossManager _bossManager;
    [Tooltip("デモの管理を行うスクリプト")]
    [SerializeField] private MiniGame02Demo _demo;

    // ゲームデータ参照
    [Header("Game Data")]
    [Tooltip("アイテムの配列")]
    [SerializeField] private MiniGame02Item[] _items;



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
        _demo.DemoInitialize().Forget();
    }


    // 初期化メソッド。
    public void Initialize()
    {
        SetFollowTarget(_playerTransform); // カメラ追従対象の初期化。
        _pcController.PlayerInitialize(); // プレイヤーコントローラーの初期化。
        UpdateItemPlacement(); // アイテムの初期配置を行う。
        ResetPlayerPosition(); // プレイヤーの開始位置を設定。
        ResetItemStatus(); // アイテムの取得状況をリセット。
    }

    // 全てのアイテムの取得状況をリセットするメソッド。
    public void ResetItemStatus()
    {
        foreach (var item in _items)
        {
            item.ToggleGetItem(false); // 各アイテムの取得状況をリセット。
        }
    }

    // プレイヤーを開始位置に戻すメソッド。
    public void ResetPlayerPosition()
    {
        if (_playerTransform != null && _StartPoint != null)
        {
            _playerTransform.position = _StartPoint.position; // プレイヤーの位置を開始位置に戻す。
        }
        else
        {
            Debug.LogWarning("PlayerTransform or StartPoint is not assigned.");
        }
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
            .transform.DOMove(_playerButtobiPoint.position, 0.5f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                // 主人公を初期位置に戻す。
                _playerTransform.position = _StartPoint.position;
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
