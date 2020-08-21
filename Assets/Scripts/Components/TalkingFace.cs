using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkingFace : MonoBehaviour
{
    public float BounceSpeed = 1;
    public float BounceHeight = 0.5f;
    public TextWriter Writer;

    private float counter;
    private bool working;
    private Vector3 original;
    private bool down;

    // Start is called before the first frame update
    void Start()
    {
        original = transform.position; // Cache our original position
        Writer.AddOnTextWriterStartDelegate(StartWorking); // Plug into the text writer start delegate
        Writer.AddOnTextWriterStopDelegate(StopWorking); // Plug into the text writer stop delegate
    }

    // Update is called once per frame
    void Update()
    {
        if (working)
        {
            counter += Time.unscaledDeltaTime;
            if(counter >= BounceSpeed) // If we need to bounce
            {
                counter -= BounceSpeed;
                if (down)
                    transform.position = original; // Snap to our original
                else
                    transform.position = original + new Vector3(0, BounceHeight); // Snap upwards

                down = !down; // Toggle our direction
            }
        }
    }

    void StartWorking()
    {
        original = transform.position;
        working = true;
    }

    void StopWorking()
    {
        working = false;
        transform.position = original; // Use our original when we stop
    }
}
