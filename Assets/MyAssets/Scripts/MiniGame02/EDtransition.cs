using Cysharp.Threading.Tasks;
using UnityEngine;

public class EDtransition : MonoBehaviour
{
    [SerializeField] TransitionManager _transitionManager; // 画面遷移の管理を行うスクリプト。
    void Start()
    {
        _transitionManager.SheepMaskIn().Forget(); // ゲーム開始時に画面遷移を実行。
    }
}
