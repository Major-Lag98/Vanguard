using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayAfter : MonoBehaviour
{
    public TextWriter Writer;

    private TextWriter myWriter;

    // Start is called before the first frame update
    void Start()
    {
        // Get our writer
        myWriter = GetComponent<TextWriter>();

        // Create this as a step for later
        TextWriter.OnTextWriterEventDelegate startWriting = () => { };
        TextWriter.OnTextWriterEventDelegate hideMyself = () => { };

        // Define the delegate
        startWriting = () =>
        {
            myWriter.StartWriting(); // We start writing our writer
            //Writer.RemoveOnTextWriterStopDelegate(startWriting); // Remove us from the writer
        };

        // Define the delegate
        hideMyself = () =>
        {
            myWriter.Restart();
            //Writer.RemoveOnTextWriterStartDelegate(hideMyself); // Remove us from the writer
        };

        Writer.AddOnTextWriterStopDelegate(startWriting);
        Writer.AddOnTextWriterRestartDelegate(hideMyself);

    }





}
