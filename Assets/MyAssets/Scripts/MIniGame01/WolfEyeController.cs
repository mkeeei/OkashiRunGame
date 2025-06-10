using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class WolfEyeController : MonoBehaviour
{
    [SerializeField] private float timer;
    void Start()
    {
        // スケールを0から100に変化させるアニメーションを開始
        transform.localScale = Vector3.zero;
        transform.DOScale(new Vector3(100f, 100f, 100f), 2f); // 2秒でスケール変更
    }

    private void Update()
    {
        
    }

}
