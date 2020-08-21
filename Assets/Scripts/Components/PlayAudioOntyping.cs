using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioOntyping : MonoBehaviour
{
    public TextWriter Writer;
    public AudioSource Audio;

    // Start is called before the first frame update
    void Start()
    {
        Writer.AddOnTextWriterStartDelegate(PlaySound);  
        Writer.AddOnTextWriterStopDelegate(StopSound);
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
