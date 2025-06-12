// アイテムのスプライトを管理するスクリプト。

using UnityEngine;

public class MinGame02ItemSpriteSelector : MonoBehaviour
{
    [SerializeField] private Sprite[] _itemSprites; // アイテムのスプライトを格納する配列。
    [SerializeField] private SpriteRenderer _spriteRenderer; // スプライトを表示するSpriteRendererコンポーネント。

    private void Start()
    {
        // スプライトをランダムに設定。
        if (_itemSprites.Length > 0)
        {
            int randomIndex = Random.Range(0, _itemSprites.Length);
            SetItemSprite(randomIndex);
        }
        else
        {
            Debug.LogWarning("No item sprites assigned.");
        }
    }

    // アイテムのスプライトを設定するメソッド。
    public void SetItemSprite(int index)
    {
        // インデックスが範囲内であることを確認
        if (index < 0 || index >= _itemSprites.Length)
        {
            Debug.LogError("Invalid sprite index: " + index);
            return;
        }
        // スプライトを設定
        _spriteRenderer.sprite = _itemSprites[index];
    }
}