// ミニゲームシーン02におけるボスエネミーのクラス。

using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using R3;
using Unity.Cinemachine;
using UnityEngine;

public class MiniGame02BossManager : MonoBehaviour
{
    [Header("参照コンポーネント")]
    [SerializeField] MiniGame02_PlayerController _player; // プレイヤーの参照を設定する変数。
    [SerializeField] SpriteRenderer _bossSprite; // ボスのスプライトレンダラーを設定する変数。
    [SerializeField] MiniGame02Manager _miniGameManager; // ミニゲームマネージャーの参照を設定する変数。
    [SerializeField] Transform[] _bossKnockBackTransforms; // ボスのTransformを設定する変数。
    [SerializeField] CinemachineCamera _bossCamera; // ボスのカメラを設定する変数。

    [SerializeField] public SerializableReactiveProperty<int> _bossHealth = new(3); // ボスの体力を管理するReactiveProperty。
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
    public async UniTask OnBossHit()
    {
        // ボスの体力を1減らす。
        _bossHealth.Value--;

        // ボスの体力が0以下になった場合、ボスを倒す。
        if (_bossHealth.Value <= 0)
        {
            Debug.Log("Boss Defeated!");
            // ボスの消去処理を呼び出す。
            return;
        }

        // ボスの体力がまだ残っている場合、演出に移行するためにボスカメラの優先度を上昇。
        _bossCamera.Priority = 11;

        // 点滅させる。
        _bossSprite.DOFade(0f, 0.15f).SetLoops(8, LoopType.Yoyo)
            .OnComplete(() =>
            {
                // 点滅終了後に透明度を元に戻す。
                _bossSprite.color = new Color(_bossSprite.color.r, _bossSprite.color.g, _bossSprite.color.b, 1f);
            });

        // 残り体力に応じてノックバックさせ、位置まで移動させる。
        switch (_bossHealth.Value)
        {
            case 2:
                // ボスの体力が2のときのノックバック処理。
                await this.transform.DOMoveX(_bossKnockBackTransforms[0].position.x, 1f)
                    .SetEase(Ease.OutQuad)
                    .AsyncWaitForCompletion();
                break;
            case 1:
                // ボスの体力が1のときのノックバック処理。
                await this.transform.DOMoveX(_bossKnockBackTransforms[1].position.x, 1f)
                   .SetEase(Ease.OutQuad)
                   .AsyncWaitForCompletion();
                break;
        }

        // ノックバックが完了したら数秒待機
        await UniTask.Delay(TimeSpan.FromSeconds(1));

        // カメラの優先度を下げて演出終了。
        _bossCamera.Priority = 0;

        // プレイヤーを移動可能状態に戻す。
        _player.PlayerInitialize();
    }

    // ボスと衝突した際に呼び出されるメソッド。
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // プレイヤータグを確認。
        if (collision.gameObject.CompareTag("Player"))
        {
            // プレイヤーを弾き飛ばす処理を呼び出す。
            _miniGameManager.KnockBackPlayer();

            // プレイヤーの速度が十分であれば、ボスにダメージを与える。
            if (_player._moveLevel >= _koMoveLevel.Value)
            {
                // ボスの被弾処理を呼び出す。
                OnBossHit().Forget();
            }
        }
    }
}