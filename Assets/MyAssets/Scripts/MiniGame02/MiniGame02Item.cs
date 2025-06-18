// ミニゲームシーン02におけるアイテムのクラス。

using R3;
using UnityEngine;

public class MiniGame02Item : MonoBehaviour
{
    [SerializeField] private ItemType itemType = ItemType.SpeedUp; // アイテムの種類を設定する変数。
    [SerializeField] MiniGame02_PlayerController _player; // プレイヤーの参照を設定する変数。
    [SerializeField] SerializableReactiveProperty<bool> _isGetItem = new(false); // アイテム取得状態を管理するReactiveProperty。
    [SerializeField] SpriteRenderer _itemSprite; // アイテムのスプライトレンダラーを設定する変数。
    [SerializeField] MiniGame02AudioManager _audioManager; // オーディオマネージャーの参照を設定する変数。

    // アイテムの種類を定義する列挙型。
    public enum ItemType
    {
        Heart,      // ハート
        SpeedUp     // パワーアップ
    }

    private void Start()
    {
        // _isGetItemを監視し、アイテムの取得状態が変化したときにDestroyItemメソッドを呼び出す。
        _isGetItem.Subscribe(isGet => 
        {
            if (isGet)
            {
                DestroyItem(); // アイテムが取得された場合、DestroyItemメソッドを呼び出す。
            }
            else
            {
                RespawnItem(); // アイテムが未取得の場合、RespawnItemメソッドを呼び出す。
            }
        });
    }

    // プレイヤーがアイテムに触れたときに呼び出されるメソッド。
    public void GetItem()
    {
        // アイテムが取得済みの場合は何もしない。
        if (_isGetItem.Value)
        {
            return;
        }

        // SEを再生する。
        _audioManager.PlayGetItemSound();

        // アイテムの種類がSpeedUpの場合、プレイヤーの速度を上げる処理を実行。
        if (itemType == ItemType.SpeedUp)
        {
            Debug.Log("Speed Up Item Acquired!");
            _player.AddMoveLevel(0.5f);

            // 速度変化を適用するためにSetVelocityを呼び出す。
            _player.SetVelocity();
        }
    }

    // アイテムの取得状態を変更するメソッド。
    public void ToggleGetItem(bool isGet)
    {
        _isGetItem.Value = isGet;
    }

    // 消去処理。
    public void DestroyItem()
    {
        // アイテムを半透明にして取得済みにする。
        _itemSprite.color = new Color(1f, 1f, 1f, 0.5f);
    }

    // 復活処理。
    public void RespawnItem()
    {
        // アイテムを再表示し、取得状態をリセット。
        _itemSprite.color = new Color(1f, 1f, 1f, 1f);
    }

    // 取得処理。
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // プレイヤーがアイテムを取得した場合。
        if (collision.CompareTag("Player"))
        {
            // アイテム取得音を再生。
            _audioManager.PlayGetItemSound();

            // アイテムの取得処理を実行。
            GetItem();

            // アイテム取得状態を更新。
            ToggleGetItem(true);
        }
    }
}