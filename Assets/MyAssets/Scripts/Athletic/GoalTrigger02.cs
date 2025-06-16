using System.Transactions;
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
        // 一度だけ発火
        if (hasCleared) return;

        // PlayerFlappy コンポーネントを持つものをプレイヤーと見なす
        if (other.TryGetComponent<PlayerFlappy>(out _))
        {
            hasCleared = true;
            // もし演出が必要ならここで BGM 止めたり、アニメーションを流してから遷移してもOK

            transition.SheepMaskOut();

            Debug.Log("ステージクリア！");
            Time.timeScale = 0;
            
            // 直接次のシーンをロード
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
