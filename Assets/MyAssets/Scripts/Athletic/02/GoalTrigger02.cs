using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class GoalTrigger : MonoBehaviour
{
    [Header("次にロードするシーン名")]
    [SerializeField] private string nextSceneName;

    [SerializeField] TransitionManager transition;

    private bool hasCleared = false;

    private void Awake()
    {
        // 必ず Is Trigger にしておく
        var col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasCleared) return;

        if (other.TryGetComponent<PlayerFlappy>(out _))
        {
            hasCleared = true;

            // 1) State を Clear に切り替え
            var mgr = FindFirstObjectByType<Athletic02Manager>();
            mgr?.OnPlayerClear();

            // 2) 非同期でトランジション→シーン切り替え
            DoMaskOut().Forget();
        }
    }

    private async UniTaskVoid DoMaskOut()
    {
        await transition.SheepMaskOut();

        SceneManager.LoadScene(nextSceneName);
    }
}
