// ミニゲームシーン02のデモを管理するスクリプト。

using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class MiniGame02Demo : MonoBehaviour
{
    [SerializeField] MiniGame02Manager _miniGameManager; // ミニゲームの管理を行うスクリプト。
    [SerializeField] GameObject _pcObject; // プレイヤーキャラクターのオブジェクト。
    [SerializeField] SpriteRenderer _pcSpriteRenderer; // プレイヤーキャラクターのスプライトレンダラー。
    [SerializeField] MiniGame02BossManager _bossManager; // ボスエネミーの管理を行うスクリプト。
    [SerializeField] GameObject _blackLine; // 黒線のオブジェクト。
    [SerializeField] MiniGame02_PlayerController _pcController; // プレイヤーコントローラーのスクリプト。
    [SerializeField] TransitionManager _transitionManager; // 画面遷移の管理を行うスクリプト。

    [Header("デモの設定")]
    [SerializeField] Transform _bamiPCStart; // PCがスタートする位置。
    [SerializeField] Transform _bamiBossStart; // ボスがスタートする位置。
    [SerializeField] Transform _bamiPCButtobi; // PCが吹き飛ぶ位置。


    // デモの初期化メソッド。
    public async UniTask DemoInitialize()
    {
        // 画面上下の黒枠を表示。
        _blackLine.SetActive(true);

        // トランジションの開始。
        await _transitionManager.SheepMaskIn();

        // 開始メソッドを呼び出す。
        DemoStart().Forget();
    }

    // デモの開始メソッド。
    public async UniTask DemoStart()
    {
        // PCの移動を無効化。
        _pcController.ToggleCanMove(false);

        // PCを規定の位置に移動。
        await _pcObject.transform.DOMoveX(_bamiPCStart.transform.position.x, 1f)
            .SetEase(Ease.OutSine)
            .AsyncWaitForCompletion();

        // 0.4秒ごとに_pcSpriteRendererのXを交互に切り替える
        for (int i = 0; i < 4; i++)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(0.4f)); // 少し待機。
            _pcSpriteRenderer.flipX = !_pcSpriteRenderer.flipX; // PCの向きを反転。
        }

        // ボスが壁を突き破って登場。
        await _bossManager.gameObject.transform.DOMoveX(_bamiBossStart.transform.position.x, 0.2f)
            .SetEase(Ease.Linear)
            .AsyncWaitForCompletion();

        // PCは回転しながら吹き飛ばされる。
        // 回転タスクをUniTaskに変換
        var rotateTask = _pcObject.transform.DORotate(new Vector3(0f, 1080f, 0f), 0.3f)
                          .AsyncWaitForCompletion().AsUniTask();

        // 移動タスクをUniTaskに変換
        var moveTask = _pcObject.transform.DOLocalMove(_bamiPCButtobi.position, 0.3f)
                          .SetEase(Ease.Linear)
                          .AsyncWaitForCompletion().AsUniTask();

        // 両方のUniTaskを同時実行
        await UniTask.WhenAll(rotateTask, moveTask);



        // PCは初期位置へ移動。
        _miniGameManager.ResetPlayerPosition();

        // ボスが身震いし、動けなくなったことを表現。
        await _bossManager.gameObject.transform.DOPunchPosition(new Vector3(0.2f, 0, 0), 0.4f, 20, 1f)
            .AsyncWaitForCompletion();
        await UniTask.Delay(TimeSpan.FromSeconds(0.4f)); // 少し待機。
        await _bossManager.gameObject.transform.DOPunchPosition(new Vector3(0.2f, 0, 0), 0.4f, 20, 1f)
            .AsyncWaitForCompletion();
        await UniTask.Delay(TimeSpan.FromSeconds(0.4f)); // 少し待機。

        // カメラをPCに切り替え
        _miniGameManager.SetFollowTarget(_pcObject.transform); // PCのTransformをカメラの追従対象に設定。

        // 画面上下の黒枠を非表示。
        _blackLine.SetActive(false);

        // PCの移動を有効化。
        _pcController.ToggleCanMove(true);

        // 初期化処理。
        _miniGameManager.Initialize(); // ミニゲームの初期化を行う。
    }

}
