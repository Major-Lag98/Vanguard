using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionController : MonoBehaviour
{
    public GameObject SelectionRect;

    private GameObject selected;
    private ISelectable selectable;

    public static SelectionController Instance;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
            Instance = this;
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
            this.selected = null;
            SelectionRect.SetActive(false);
            return;
        }
        

        // Then hook us up with the current stuff
        this.selected = selected;
        this.selectable = selected.GetComponent<ISelectable>();
        SelectionRect.SetActive(true);
        SelectionRect.transform.parent = selected.transform;
        SelectionRect.transform.localPosition = new Vector3(0, 0, -5);
    }

    private void OnDisable()
    {
        transform.parent = null;
    }
}
