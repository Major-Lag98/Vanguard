using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class TextContentManager : MonoBehaviour
{
    [Tooltip("The text file to use for the TextWriter")]
    public List<TextAsset> TextFiles;
    [Tooltip("The target TextMeshPro to provide lines for")]
    public TextMeshProUGUI textMeshPro;
    [Tooltip("The target TextWriter to feed lines to")]
    public TextWriter writer;

    public GameObject DisableOnComplete;
    public List<GameObject> EnableOnComplete;

    private string[] text;
    private int textIter;

    public delegate void ChangedState();

    private ChangedState OnStart;
    private ChangedState OnEnd;

    bool active;

    public static TextContentManager Instance;

    private void Awake()
    {
        //text = TextFiles[0].text.Split('\n');
        //textMeshPro.text = text[0]; // Assign the first line to begin with.

        if(Instance == null)
            Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //TimeTicker.Instance.SetTimeScale(0); // Pause the game while we are doing this

        DisableOnComplete.GetComponent<HideAndShowUIElement>().AddOnHideDelegate(() => Time.timeScale = 1);
        DisableOnComplete.GetComponent<HideAndShowUIElement>().AddOnShowDelegate(StartWriter);

        Begin(0);
    }

    /// <summary>
    /// Beings a new dialogue
    /// </summary>
    /// <param name="index">The index of the file to load</param>
    public void Begin(int index)
    {
        active = true;
        textIter = 0;
        DisableOnComplete.GetComponent<IHideableUI>()?.Show(); // Show us
        TimeTicker.Instance.SetTimeScale(0); // Pause the game while we are doing this
        text = TextFiles[index].text.Split('\n');
        writer.SetTextToWrite(text[0]); // Set the initial text
    }

    void StartWriter()
    {
        
        writer.StartWriting(); // Start writing
    }

    // Update is called once per frame
    void Update()
    {
        // If we press space
        if (Input.GetKeyDown(KeyCode.Space) && active)
        {
            if (writer.IsWorking()) // If working, skip the working text
                writer.Skip();
            else                    // Otherwise we go to the next text chunk
            {
                textIter++;
                if (textIter < text.Length) // If we're within our bounds still, then write!
                {
                    writer.SetTextToWrite(text[textIter]); // Set the next line to write
                    if (!writer.AutoStartWriting) // Start writing if the writer is set to auto
                        writer.StartWriting();
                }
                else                        // Otherwise we should close this UI
                {
                    TimeTicker.Instance.SetTimeScale(1); // Set to regular time scale
                    EnableOnComplete.ForEach(obj => obj.GetComponent<IHideableUI>()?.Show());
                    DisableOnComplete.GetComponent<IHideableUI>()?.Hide();
                    OnEnd?.Invoke();
                    active = false;

                    if(GameStateManager.Instance.DayCycleNumber < 1)
                        GameStateManager.Instance.SetNight(); //TODO I don't like this here... too coupled
                }
            }
        }
    }

    public void AddOnStartDelegate(ChangedState del)
        => OnStart += del;

    public void AddOnEndDelegate(ChangedState del)
        => OnEnd+= del;

    public void RemoveOnStartDelegate(ChangedState del)
        => OnStart -= del;

    public void RemoveOnEndDelegate(ChangedState del)
        => OnEnd -= del;
}
