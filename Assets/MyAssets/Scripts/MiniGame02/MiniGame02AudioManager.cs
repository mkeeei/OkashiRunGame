using UnityEngine;

public class MiniGame02AudioManager : MonoBehaviour
{
    [Header("BGM Settings")]
    [SerializeField] AudioSource _bgmAudioSource; // BGM専用のAudioSource
    [SerializeField] AudioClip _bossBGM; // ボス戦のBGMクリップ

    [Header("SE Settings")]
    [SerializeField] AudioSource _seAudioSource; // SE専用のAudioSource
    [SerializeField] AudioClip _getItemSoundClip; // アイテム取得音のAudioClip
    [SerializeField] AudioClip _hitSoundClip; // ヒット音のAudioClip
    [SerializeField] AudioClip _jumpSE; // ジャンプ音のAudioClip

    void Awake()
    {
        // BGMはループ再生に設定
        _bgmAudioSource.loop = true;

        // SEはループさせない
        _seAudioSource.loop = false;
        // SEはBGMとは独立して再生されるように、PlayOnAwakeをfalseに設定
        _seAudioSource.playOnAwake = false;
    }

    /// <summary>
    /// ボス戦BGMの再生を開始します。
    /// </summary>
    public void PlayBossBGM()
    {
        if (_bossBGM != null)
        {
            _bgmAudioSource.clip = _bossBGM;
            _bgmAudioSource.Play();
        }
        else
        {
            Debug.LogWarning("Boss BGM AudioClip is not assigned.");
        }
    }

    /// <summary>
    /// BGMの再生を停止します。
    /// </summary>
    public void StopBGM()
    {
        _bgmAudioSource.Stop();
    }

    /// <summary>
    /// アイテム取得音を再生します。
    /// </summary>
    public void PlayGetItemSound()
    {
        if (_getItemSoundClip != null)
        {
            _seAudioSource.PlayOneShot(_getItemSoundClip);
        }
        else
        {
            Debug.LogWarning("Get Item Sound AudioClip is not assigned.");
        }
    }

    /// <summary>
    /// ヒット音を再生します。
    /// </summary>
    public void PlayHitSound()
    {
        if (_hitSoundClip != null)
        {
            _seAudioSource.PlayOneShot(_hitSoundClip);
        }
        else
        {
            Debug.LogWarning("Hit Sound AudioClip is not assigned.");
        }
    }

    /// <summary>
    /// ジャンプ音を再生します。
    /// 
    public void PlayJumpSound()
    {
        if (_jumpSE != null)
        {
            _seAudioSource.PlayOneShot(_jumpSE);
        }
        else
        {
            Debug.LogWarning("Jump Sound AudioClip is not assigned.");
        }
    }
}