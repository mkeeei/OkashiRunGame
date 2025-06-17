using Cysharp.Threading.Tasks;
using DG.Tweening;
using R3;
using UnityEngine;
using UnityEngine.InputSystem;

public class MiniGame02_PlayerController : MonoBehaviour
{
    // 移動設定
    [Header("Movement Settings")]
    [Tooltip("プレイヤーの移動速度")]
    [SerializeField] private const float MOVE_SPEED = 5f;
    [Tooltip("ジャンプの力")]
    [SerializeField] private const float JUMP_FORCE = 10f;

    // プレイヤーの状態
    [Header("Player State")]
    [Tooltip("プレイヤーが移動可能かどうか")]
    public SerializableReactiveProperty<bool> _canMove = new(true);
    [Tooltip("ジャンプ中かどうか")]
    public SerializableReactiveProperty<bool> _isJumping = new();
    [Tooltip("現在の移動レベル")]
    public float _moveLevel = 1f;

    // コンポーネント参照
    [Header("Component References")]
    [Tooltip("プレイヤーのスプライトレンダラー")]
    [SerializeField] private SpriteRenderer _pcSpriteRenderer;
    [Tooltip("Rigidbody2Dコンポーネント")]
    [SerializeField] private Rigidbody2D _rb;

    // マネージャー参照
    [Header("Manager References")]
    [Tooltip("画面遷移を管理するTransitionManager")]
    [SerializeField] private TransitionManager _transitionManager;
    [Tooltip("ミニゲームを管理するMiniGame02Manager")]
    [SerializeField] private MiniGame02Manager _miniGameManager;

    private ReactiveProperty<Vector2> _moveInput = new();

    private void Start()
    {
        PlayerInitialize(); // プレイヤーの初期化メソッドを呼び出す。
        _moveInput.Subscribe(move => OnMoveInputChanged(move)); // _moveInputを購読して、値が変化したときにOnMoveInputChangedメソッドを呼び出す。
    }

    // プレイヤーの初期化メソッド。
    public void PlayerInitialize()
    {
        _canMove.Value = true; // プレイヤーが移動可能に設定。
        ResetMoveLevel(); // 移動レベルをリセットする。
        _isJumping.Value = false; // ジャンプ状態を初期化する。
        _rb.gravityScale = 2f; // Rigidbody2Dの重力スケールを設定する。
    }

    // プレイヤーの移動の可否を切り替えるメソッド。
    public void ToggleCanMove(bool canMove) => _canMove.Value = canMove;

    // _moveLevel変更メソッド。
    public void AddMoveLevel(float level)
    {
        _moveLevel = Mathf.Clamp(_moveLevel + level, 1.0f, 5.0f); // 移動レベルを加算し、1.0fから5.0fの範囲に制限する。
    }

    // _moveLevelをリセットするメソッド。
    public void ResetMoveLevel() => _moveLevel = 1f; // 移動レベルを初期値にリセットする。

    // 速度をセットしなおすメソッド。加減速時に速度変化を適用するために呼び出す。
    public void SetVelocity() => OnMoveInputChanged(_moveInput.Value);

    // 移動入力が変化したときに呼び出されるメソッド。
    private void OnMoveInputChanged(Vector2 move)
    {
        _rb.linearVelocity = new Vector2(move.x * MOVE_SPEED * _moveLevel, _rb.linearVelocity.y); // x方向の速度を計算し、y方向の速度は保持する。
    }

    // プレイヤーのノックアウト処理メソッド。
    public async UniTask PCKnockOut()
    {
        _canMove.Value = false; // 移動可能状態をfalseに設定する。
        if (TryGetComponent(out CapsuleCollider2D capsuleCollider))
        {
            capsuleCollider.enabled = false; // 地面すり抜けのためにコライダーを一時的に無効化する。
        }

        // 地面を無視して落下。
        Sequence sequence = DOTween.Sequence(); // Sequenceを作成

        // 落下アニメ。
        await sequence.Append(transform.DOLocalMoveX(2f, 0.8f).SetEase(Ease.Linear)) // X方向に移動。
            .Join(_pcSpriteRenderer.transform.DORotate(new Vector3(0f, 0f, 2880f), 0.8f)) // 720度回転。
            .Join(transform.DOLocalMoveY(1f, 0.4f).SetEase(Ease.OutCubic)// いったん上昇。
            .OnComplete(() => transform.DOLocalMoveY(-4f, 0.4f).SetEase(Ease.InCubic)))  // 落下。
            .AsyncWaitForCompletion();

        if (capsuleCollider != null)
        {
            capsuleCollider.enabled = true; // コライダーを再度有効化する。
        }

        // PCの位置でInTransitionを呼び出す。
        await _transitionManager.SheepMaskOut(); // 画面遷移のアウトメソッドを呼び出す。
        _miniGameManager.Initialize(); // 初期化。
        await _transitionManager.SheepMaskIn(); // 画面遷移のインメソッドを呼び出す。
    }

    // 移動キー入力受付メソッド。
    public void MoveInput(InputAction.CallbackContext context)
    {
        if (!_canMove.Value) return; // 移動可能状態でない場合は何もしない。
        Vector2 input = context.ReadValue<Vector2>().normalized; // 入力値を正規化して取得する。
        _moveInput.Value = input; // _moveInputに設定する。

        // スプライトの向きを更新する。
        if (input.x > 0)
        {
            _pcSpriteRenderer.flipX = true; // 右方向に移動する場合、スプライトを正面向きにする。
        }
        else if (input.x < 0)
        {
            _pcSpriteRenderer.flipX = false; // 左方向に移動する場合、スプライトを反転させる。
        }
    }

    // ジャンプキー入力受付メソッド。
    public void JumpInput(InputAction.CallbackContext context)
    {
        // ジャンプキー入力直後、かつ現在ジャンプ中でない場合にジャンプを実行する。
        if (context.started && !_isJumping.Value)
        {
            _rb.AddForce(Vector2.up * JUMP_FORCE, ForceMode2D.Impulse);
        }
    }
}