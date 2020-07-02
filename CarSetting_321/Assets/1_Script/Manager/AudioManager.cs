using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager:Singleton<AudioManager>
{

    /// <summary>
    /// audio_C_0
    /// 拆卸音频：Game10_1_C
    /// 安装音频：Game10_1
    /// </summary>
    /// <param name="audioName"></param>
    public void PlayAudio(string audioName)
    {
        if (string.IsNullOrEmpty(audioName)) { return; }

        AudioClip ac = Resources.Load("sounds/" + audioName) as AudioClip;

        AudioSource audioS = Camera.main.GetComponent<AudioSource>();

        audioS.clip = ac;

        audioS.Play();
    }
}
