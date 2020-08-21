using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionController : MonoBehaviour
{
    public GameObject SelectionRectPrefab;

    private GameObject selected;
    private ISelectable selectable;
    private GameObject selectionRect;

    public static SelectionController Instance;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
            Instance = this;

        selectionRect = Instantiate(SelectionRectPrefab);
        selectionRect.SetActive(false); // Disable immediately
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            Select(null);
    }

    public void Select(GameObject selected)
    {
        // If our previous selected is not null, deselect it
        if (this.selected != null)
            this.selectable.Deselect();

        // If incoming object is null, clear everything
        if (selected == null)
        {
            this.selected = null; // Clear our selected
            selectionRect.transform.parent = null; // Clear the rect's parent
            selectionRect.SetActive(false); // Disable the rect
            return;
        }

        // Then hook us up with the current stuff
        this.selected = selected;
        this.selectable = selected.GetComponent<ISelectable>();
        selectionRect.SetActive(true);
        selectionRect.transform.parent = selected.transform;
        selectionRect.transform.localPosition = new Vector3(0, 0, -5);
    }

   
}
