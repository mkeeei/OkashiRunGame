using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class WolfEyeController : MonoBehaviour
{
    [SerializeField] private float timer;
    public AudioClip voiceSE;
    AudioSource audioSource;
    void Start()
    {
        // スケールを0から100に変化させるアニメーションを開始
        transform.localScale = Vector3.zero;
        transform.DOScale(new Vector3(10f, 10f, 10f), 1f); // 1秒でスケール変更
        this.audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer > 3)
        {
            timer = 0;
            this.audioSource.PlayOneShot(this.voiceSE);
            // 1秒かけてスケールを0にし、イージングを追加
            transform.DOScale(Vector3.zero, 1f).SetEase(Ease.OutQuad).OnKill(() => Destroy(gameObject)); ;
            
        }

    }

}
