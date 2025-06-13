// ミニゲームシーン02におけるプレイヤーコントローラーのスクリプト。

using R3;
using UnityEngine;
using UnityEngine.InputSystem;

public class MiniGame02_PlayerController : MonoBehaviour
{
    const float MOVE_SPEED = 5f; // プレイヤーの移動速度定数。
    const float JUMP_FORCE = 10f; // ジャンプの力定数。
    public SerializableReactiveProperty<bool> _canMove = new(true); // プレイヤーが移動可能かどうかを管理するReactiveProperty。

    [SerializeField] Rigidbody2D _rb; // Rigidbody2Dコンポーネントを格納する変数。
    ReactiveProperty<Vector2> _moveInput = new(); // 移動入力を格納するReactiveProperty。
    public float _moveLevel = 1f; // 移動レベルを格納する変数。
    public SerializableReactiveProperty<bool> _isJumping = new(); // ジャンプ状態を格納するReactiveProperty。


    private void Start()
    {
        PlayerInitialize(); // プレイヤーの初期化メソッドを呼び出す。

        // _moveInputを購読して、値が変化したときにOnMoveInputChangedメソッドを呼び出す。
        _moveInput.Subscribe(move =>
        {
            OnMoveInputChanged(move);
        });
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
    public void ToggleCanMove(bool _move)
    {
        _canMove.Value = _move; // プレイヤーの移動可能状態を設定する。
    }

    // _moveLevel変更メソッド。
    public void AddMoveLevel(float level)
    {
        // 移動レベルを加算する。
        _moveLevel += level;

        // 最大値を5.0fに抑える。
        _moveLevel = Mathf.Clamp(_moveLevel, 1.0f, 5.0f);
    }

    // _moveLevelをリセットするメソッド。
    public void ResetMoveLevel()
    {
        _moveLevel = 1f; // 移動レベルを初期値にリセットする。
    }

    // 速度をセットしなおすメソッド。加減速時に速度変化を適用するために呼び出す。
    public void SetVelocity()
    {
        OnMoveInputChanged(_moveInput.Value);
    }


    // 移動入力が変化したときに呼び出されるメソッド。
    private void OnMoveInputChanged(Vector2 move)
    {
        // 現在の速度を一時的な変数にコピーする
        Vector2 velocity = _rb.linearVelocity;

        // x成分のみ更新する
        velocity.x = move.x * MOVE_SPEED * _moveLevel;

        // 新しい速度をRigidbody2Dに適用する
        _rb.linearVelocity = velocity;
    }

    // 移動キー入力受付メソッド。
    public void MoveInput(InputAction.CallbackContext context)
    {
        // 移動可能状態でない場合は何もしない。
        if (!_canMove.Value) return;
        _moveInput.Value = context.ReadValue<Vector2>().normalized;
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