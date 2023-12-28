using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 音频播放
/// </summary>
public class PlaySound : MonoBehaviour
{
    private AudioSource audioSource;

    public AudioClip audioClip;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(audioClip,GameManager.Instance.volume);
    }
}
