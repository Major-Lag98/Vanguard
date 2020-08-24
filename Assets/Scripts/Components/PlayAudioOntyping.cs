using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioOntyping : MonoBehaviour
{
    public TextWriter Writer;
    public AudioSource Audio;

    bool playing;

    // Start is called before the first frame update
    void Start()
    {
        Writer.AddOnTextWriterStartDelegate(() => playing = true);
        Writer.AddOnTextWriterStopDelegate(() => playing = false);
    }

    private void LateUpdate()
    {
        das();
    }

    private void das()
    {
        if (playing && !Audio.isPlaying)
            PlaySound();
        else if (Audio.isPlaying && !playing)
            StopSound();
    }

    void PlaySound()
    {
        Audio.Play();
    }

    void StopSound()
    {
        Audio.Stop();
    }
}
