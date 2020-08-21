using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayAfter : MonoBehaviour
{
    [Tooltip("The TextWriter to wait for")]
    public TextWriter Writer;

    [Tooltip("The TextWriter to start after the main writer is done. If left null, pulls from the current object")]
    public TextWriter WriterToStart;

    // Start is called before the first frame update
    void Start()
    {
        // Get our writer if we don't have one from the inspector
        if(!WriterToStart)
            WriterToStart = GetComponent<TextWriter>();

        // Create this as a step for later
        TextWriter.OnTextWriterEventDelegate startWriting = () => { };
        TextWriter.OnTextWriterEventDelegate hideMyself = () => { };

        // Define the delegate
        startWriting = () =>
        {
            WriterToStart.StartWriting(); // We start writing our writer
            //Writer.RemoveOnTextWriterStopDelegate(startWriting); // Remove us from the writer
        };

        // Define the delegate
        hideMyself = () =>
        {
            WriterToStart.Restart();
            //Writer.RemoveOnTextWriterStartDelegate(hideMyself); // Remove us from the writer
        };

        Writer.AddOnTextWriterStopDelegate(startWriting);
        Writer.AddOnTextWriterRestartDelegate(hideMyself);

    }





}
