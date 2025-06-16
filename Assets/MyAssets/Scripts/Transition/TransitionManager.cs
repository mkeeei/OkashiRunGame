// 画面遷移演出を管理するスクリプト。

using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

public class TransitionManager : MonoBehaviour
{
    [Header("画面遷移設定")]
    [SerializeField] float _duration = 0.5f; // 遷移アニメーションの持続時間

    [Header("羊マスク設定")]
    [SerializeField] private SpriteRenderer _blackScreen; // 羊マスクの黒背景
    [SerializeField] private SpriteMask _sheepSpriteMask; // 羊マスクのスプライトマスク
    private Vector3 _originalScaleSheep = new(3f, 3f, 1f); // 元のスケール


    // 羊マスクのアウトメソッド（画面を黒くする方）。
    public async UniTask SheepMaskOut()
    {
        // スプライトマスクの縮小アニメを開始。
        await _sheepSpriteMask.transform
            .DOScale(Vector3.zero, _duration)
            .SetEase(Ease.OutQuad)
            .AsyncWaitForCompletion();
    }

    // 羊マスクのインメソッド（画面を元に戻す方）。
    public async UniTask SheepMaskIn()
    {
        // _sheepSpriteMaskのスケールをzeroに設定。
        _sheepSpriteMask.transform.localScale = Vector3.zero;

        // スプライトマスクの拡大アニメを開始。
        await _sheepSpriteMask.transform
           .DOScale(_originalScaleSheep, _duration)
           .SetEase(Ease.InQuad)
           .AsyncWaitForCompletion();
    }
}