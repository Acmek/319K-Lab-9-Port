using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    public AudioSource source;
    public AudioClip[] sounds;

    public void Sound_Play(int i) {
        source.Stop();
        source.clip = sounds[i];
        source.Play();
    }
}
