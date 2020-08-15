using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Apply directly onto a Text UI object to apply typewriter effect
/// </summary>
[DisallowMultipleComponent]
public class TextWriter : MonoBehaviour
{
    public float StartDelay;
    [Tooltip("Uses an invisible color to write in the rest of the characters, allowing the text to not shift around while writing")]
    public bool InvisibleCharacterPadding = true;

    [SerializeField] private float timePerCharacter = 0.1f;

    private TextMeshProUGUI textMesh; // Our text mesh

    private string text; // Our cached text
    private int currIndex;

    // Start is called before the first frame update
    void Start()
    {
        textMesh = GetComponentInChildren<TextMeshProUGUI>();

        text = textMesh.text; // cache our text
        textMesh.text = ""; // Clear the text content of the object

            StartWriting(StartDelay);
    }

    /// <summary>
    /// Called to start our text effect object
    /// </summary>
    public void StartWriting(float startDelay)
    {
        StartCoroutine(WriteText(startDelay));
    }

    private int SkipTokens(string text, int start)
    {
        if (text[currIndex] == '<')
            return text.IndexOf('>', start);

        return start;
    }

    private IEnumerator WriteText(float startDelay)
    {
        yield return new WaitForSeconds(startDelay);

        var size = text.Length;
        while (currIndex < size)
        {
            currIndex = SkipTokens(text, currIndex); // Skip tokens like <color></color>
            var newText = text.Substring(0, currIndex+1); // Get the text we need to display
            var rest = text.Substring(currIndex + 1);
            if (InvisibleCharacterPadding)
            {
                var tokenlessText = RemoveAllTokens(rest); // Removes (temporarily) any future color tokens
                newText += "<color=#00000000>" + tokenlessText + "</color>"; // Hide the rest of the characters with an invisible character
            }
            textMesh.text = newText; // Set it
            currIndex++; //Increment
            yield return new WaitForSeconds(timePerCharacter); // Wait for our desired time
        }
        yield break;
    }
    
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

}
