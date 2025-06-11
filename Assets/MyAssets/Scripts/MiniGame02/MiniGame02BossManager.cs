// ミニゲームシーン02におけるボスエネミーのクラス。

using DG.Tweening;
using R3;
using UnityEngine;

public class MiniGame02BossManager : MonoBehaviour
{
    [Header("参照コンポーネント")]
    [SerializeField] MiniGame02_PlayerController _player; // プレイヤーの参照を設定する変数。
    [SerializeField] SpriteRenderer _bossSprite; // ボスのスプライトレンダラーを設定する変数。

    [SerializeField] SerializableReactiveProperty<int> _bossHealth = new(3); // ボスの体力を管理するReactiveProperty。
    [SerializeField] SerializableReactiveProperty<float> _koMoveLevel = new(3); // ボスにダメージを与えられる移動レベル。


    private void Start()
    {
        // ボスの初期化を行う。
        BossInitialize();
    }

    // 初期化メソッド。
    public void BossInitialize()
    {
        _bossHealth.Value = 3;// ボスの初期体力を3に設定。
        _koMoveLevel.Value = 3.0f; // ボスにダメージを与えるための移動レベルを1に設定。
    }

    // ボスの被弾処理。
    public void OnBossHit()
    {
        // ボスの体力を1減らす。
        _bossHealth.Value--;

        // 点滅させる。
        _bossSprite.DOFade(0f, 0.15f).SetLoops(8, LoopType.Yoyo);

        // ボスの体力が0以下になった場合、ボスを倒す。
        if (_bossHealth.Value <= 0)
        {
            Debug.Log("Boss Defeated!");
            // ボスの消去処理を呼び出す。
        }
    }

    // ボスと衝突した際に呼び出されるメソッド。
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // プレイヤータグを確認。
        if (collision.gameObject.CompareTag("Player"))
        {
            // プレイヤーの速度が十分であれば、ボスにダメージを与える。
            if (_player._moveLevel >= _koMoveLevel.Value) 
            {
                Debug.Log("Boss Hit! Player was fast enough.");
                // ボスの被弾処理を呼び出す。
                OnBossHit();
            }
            else
            {
            }

            // プレイヤーを上方へ吹き飛ばす。
        }
    }
}