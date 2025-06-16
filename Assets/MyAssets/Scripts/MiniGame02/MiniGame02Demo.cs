// ミニゲームシーン02のデモを管理するスクリプト。

using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class MiniGame02Demo : MonoBehaviour
{
    [SerializeField] GameObject _pcObject; // プレイヤーキャラクターのオブジェクト。
    [SerializeField] SpriteRenderer _pcSpriteRenderer; // プレイヤーキャラクターのスプライトレンダラー。
    [SerializeField] MiniGame02BossManager _bossManager; // ボスエネミーの管理を行うスクリプト。
    [SerializeField] GameObject _blackLine; // 黒線のオブジェクト。

    [Header("デモの設定")]
    [SerializeField] GameObject _bamiPCStart; // PCがスタートする位置。
    [SerializeField] GameObject _bamiBossStart; // ボスがスタートする位置。
    [SerializeField] Transform _bamiPCButtobi; // PCが吹き飛ぶ位置。


    // デモの初期化メソッド。
    public void DemoInitialize()
    {
        // 画面上下の黒枠を表示。
        _blackLine.SetActive(true);

        // 開始メソッドを呼び出す。
        DemoStart().Forget();
    }

    // デモの開始メソッド。
    public async UniTask DemoStart()
    {
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

        // PCは吹き飛ばされる。
        await _pcObject.transform.DOLocalMove(_bamiPCButtobi.position, 0.3f)
            .SetEase(Ease.Linear)
           .AsyncWaitForCompletion();

        // PCは初期位置へ移動。

        // ボスが身震いし、動けなくなったことを表現。
        await _bossManager.gameObject.transform.DOPunchPosition(new Vector3(0.2f, 0, 0), 0.4f, 20, 1f)
            .AsyncWaitForCompletion();
        await UniTask.Delay(TimeSpan.FromSeconds(0.4f)); // 少し待機。
        await _bossManager.gameObject.transform.DOPunchPosition(new Vector3(0.2f, 0, 0), 0.4f, 20, 1f)
            .AsyncWaitForCompletion();

        // カメラをPCに切り替え

        // 画面上下の黒枠を非表示。
        _blackLine.SetActive(false);
    }

}
