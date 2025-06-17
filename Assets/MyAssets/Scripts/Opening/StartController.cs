using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;


public class StartController : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private TransitionManager transitionManager; // アニメーションPrefab

    private void Start()
    {
        SetEvent();
    }
    void SetEvent()
    {
        startButton.onClick.AddListener(() => StartGame());
    }

    public void StartGame()
    {
        Debug.Log("ボタン押されたよ");
        WaitAnim().Forget();
    }
    async UniTask WaitAnim()
    {
        await transitionManager.SheepMaskOut();
        // アニメーションが完了したらシーン遷移
        SceneManager.LoadScene("AthleticScene01");

    }

}
