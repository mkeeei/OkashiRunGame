// 設置判定スクリプト。

using UnityEngine;

public class MiniGame02_GroundCheck : MonoBehaviour
{
    [SerializeField] MiniGame02_PlayerController _playerController; // プレイヤーコントローラーの参照を格納する変数。

    private void OnTriggerExit2D(Collider2D collision)
    {
        // プレイヤーが地面から離れたとき、ジャンプ状態をtrueに設定する。
        if (collision.CompareTag("Ground"))
        {
            _playerController._isJumping.Value = true; // ジャンプ状態をtrueに設定。
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // プレイヤーが地面に接触したとき、ジャンプ状態をfalseに設定する。
        if (collision.CompareTag("Ground"))
        {
            _playerController._isJumping.Value = false; // ジャンプ状態をfalseに設定。
        }
    }
}