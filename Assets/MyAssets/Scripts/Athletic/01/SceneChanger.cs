using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private string sceneName = "MiniGameScene01"; // 遷移先のシーン名
    [SerializeField] private TransitionManager transitionManager; // アニメーションPrefab

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Sheep"))
        {
            WaitAnim().Forget();
        }
    }

    async UniTask WaitAnim()
    {
        await transitionManager.SheepMaskOut();
        // アニメーションが完了したらシーン遷移
        SceneManager.LoadScene(sceneName);
    }
}
