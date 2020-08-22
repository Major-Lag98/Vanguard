using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Apply directly onto a Text UI object to apply typewriter effect
/// </summary>
public class TextWriter : MonoBehaviour
{
    // Define delegates for when we start and stop
    public delegate void OnTextWriterEventDelegate();

    // These can be plugged into for events
    private OnTextWriterEventDelegate OnTextWriterStart;
    private OnTextWriterEventDelegate OnTextWriterStop;
    private OnTextWriterEventDelegate OnTextWriterRestart;

    [Tooltip("Assign to have the TextWriter operate on a specific object. If left null, the TextMeshPro will be pulled from the current object")]
    public TextMeshProUGUI TextMeshProTarget; // Our text mesh

    public float StartDelay;

    [Tooltip("Uses an invisible color to write in the rest of the characters, allowing the text to not shift around while writing")]
    public bool InvisibleCharacterPadding = true;

    public bool AutoStartWriting = true;

    private float timePerCharacter = 0.1f;

    [SerializeField]
    private float TotalWriteTime = 1f;


    private string originalText; // Our cached text
    private int currIndex;
    private bool working;

    private Coroutine storedRoutine;

    // Start is called before the first frame update
    void Start()
    {
        if(!TextMeshProTarget)
            TextMeshProTarget = GetComponentInChildren<TextMeshProUGUI>();

        if(originalText == null)
            originalText = TextMeshProTarget.text; // cache our text
        TextMeshProTarget.text = ""; // Clear the text content of the object

        if(AutoStartWriting)
            StartWriting();
    }

    public void Restart()
    {
        OnTextWriterRestart?.Invoke(); // Call our delegate event
        Skip(); // Skip anything we're still writing
        TextMeshProTarget.text = ""; // Clear the text content of the object
        currIndex = 0; // Reset the index

        if (AutoStartWriting)
            StartWriting();
    }

    /// <summary>
    /// Called to start our text effect object
    /// </summary>
    public void StartWriting()
    {
        storedRoutine = StartCoroutine(WriteText(StartDelay));
    }

    private int SkipTokens(string text, int start)
    {
        if (text[currIndex] == '<')
            return text.IndexOf('>', start);

        return start;
    }

    private IEnumerator WriteText(float startDelay)
    {
        working = true; // Change our working state to true;

        yield return new WaitForSecondsRealtime(startDelay);

        timePerCharacter = TotalWriteTime / originalText.Length;
        OnTextWriterStart?.Invoke(); // Call our start delegate

        var size = originalText.Length;
        while (currIndex < size)
        {
            currIndex = SkipTokens(originalText, currIndex); // Skip tokens like <color></color>
            var newText = originalText.Substring(0, currIndex+1); // Get the text we need to display
            var rest = originalText.Substring(currIndex + 1);
            if (InvisibleCharacterPadding)
            {
                var tokenlessText = RemoveAllTokens(rest); // Removes (temporarily) any future color tokens
                newText += "<color=#00000000>" + tokenlessText + "</color>"; // Hide the rest of the characters with an invisible character
            }
            TextMeshProTarget.text = newText; // Set it
            currIndex++; //Increment
            yield return new WaitForSecondsRealtime(timePerCharacter); // Wait for our desired time
        }

        OnTextWriterStop?.Invoke(); // Call our stop delegate
        working = false; // set our working state to false

        yield break;
    }

    /// <summary>
    /// Skips the text writing and fills the text immediately
    /// </summary>
    public void Skip()
    {
        if (working)
        {
            //currIndex = originalText.Length - 1;
            StopCoroutine(storedRoutine);
            OnTextWriterStop?.Invoke();
            TextMeshProTarget.text = originalText;
            working = false;
        }
    }

    /// <summary>
    /// Sets the text to write. Originally the text is pulled from the TextMeshPro instance.
    /// This will call Restart() on the writer to pause it and overwrite the cached text.
    /// </summary>
    /// <param name="text"></param>
    public void SetTextToWrite(string text)
    {
        Restart();
        originalText = text;
    }

    public bool IsWorking() => working;
    
    /// <summary>
    /// Removes tokens from the passed in text and returns text without tokens. Tokens are text between and including '<' and '>'.
    /// So <color=blue> is a token and is removed
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    private string RemoveAllTokens(string text)
    {
        var index = text.IndexOf('<');
        while(index >= 0)
        {
            var end = text.IndexOf('>') + 1;

            var first = text.Substring(0, index); // Then remove it by getting the substring from the text
            var second = text.Substring(end); // Then remove it by getting the substring from the text
            text = first + second; // Then combine the two strings back together without the token

            index = text.IndexOf('<');
        }

        return text;
    }

    public void AddOnTextWriterStartDelegate(OnTextWriterEventDelegate del)
        => OnTextWriterStart += del;

    public void AddOnTextWriterStopDelegate(OnTextWriterEventDelegate del)
        => OnTextWriterStop += del;

    public void RemoveOnTextWriterStartDelegate(OnTextWriterEventDelegate del)
        => OnTextWriterStart -= del;

    public void RemoveOnTextWriterStopDelegate(OnTextWriterEventDelegate del)
        => OnTextWriterStop -= del;

    public void AddOnTextWriterRestartDelegate(OnTextWriterEventDelegate del)
        => OnTextWriterRestart += del;

    public void RemoveOnTextWriterRestartDelegate(OnTextWriterEventDelegate del)
        => OnTextWriterRestart -= del;




}
