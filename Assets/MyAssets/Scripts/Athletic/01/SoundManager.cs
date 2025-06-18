using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("設定")]
    [SerializeField] private AudioSource audioSource; // AudioSource コンポーネント
    [SerializeField] private AudioClip bgmClip;       // BGM 用の AudioClip
    [SerializeField] private AudioClip jumpSound;     // ジャンプ音の AudioClip
    [SerializeField] private AudioClip gameOverSound; // ゲームオーバー音の AudioClip

    void Start()
    {
        // BGM のループ再生設定
        if (audioSource != null && bgmClip != null)
        {
            audioSource.clip = bgmClip;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    // ジャンプ時の効果音を再生
    public void PlayJumpSound()
    {
        if (audioSource != null && jumpSound != null)
        {
            audioSource.PlayOneShot(jumpSound);
        }
    }

    // ゲームオーバー時の効果音を再生
    public void PlayGameOverSound()
    {
        if (audioSource != null && gameOverSound != null)
        {
            audioSource.PlayOneShot(gameOverSound);
        }
    }
}
