using Unity.Cinemachine;
using UnityEngine;

public class MiniGame02Manager : MonoBehaviour
{
    [SerializeField] CinemachineCamera _cinemaCamera; // Cinemachineカメラコンポーネント。
    [SerializeField] Transform _playerTransform; // 主人公のTransform。
    [SerializeField] MiniGame02_PlayerController _pcController; // プレイヤーコントローラーのスクリプト。

    void Start()
    {
        // 初期化処理を呼び出す。
        Initialize();
    }


    // 初期化メソッド。
    public void Initialize()
    {
        SetFollowTarget(_playerTransform); // カメラ位置の初期化。
        _pcController.PCInitialize(); // プレイヤーコントローラーの初期化。
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
}
