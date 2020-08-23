using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionController : MonoBehaviour
{
    public GameObject SelectionRectPrefab;
    public GameObject UpgradeButton;

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
        {
            if (!IsOverUpgradeWindow())
                Select(null);
        }
    }

    bool IsOverUpgradeWindow()
    {
        var corners = new Vector3[4]; // Initialize array of 4
        ((RectTransform)UpgradeButton.transform).GetWorldCorners(corners); // Populate the array here
        Rect newRect = new Rect(corners[0], corners[2] - corners[0]); // Make a world rect

        var mouse = Input.mousePosition;
        var mouse3 = Camera.main.ScreenToWorldPoint(new Vector3(mouse.x, mouse.y, 10));

        return newRect.Contains(mouse3); // Check if the mouse is inside
    }

    public void Select(GameObject selected)
    {
        // If we're clicking upgrade, return early.
        if (IsOverUpgradeWindow())
            return;

        // If our previous selected is not null, deselect it
        if (this.selected != null)
            this.selectable.Deselect();

        // If we're placing, don't select but do let the above deselect happen
        if (PlacingController.Instance.IsPlacing())
            return;

        // If incoming object is null, clear everything
        if (selected == null)
        {
            this.selected = null; // Clear our selected
            selectionRect.transform.parent = null; // Clear the rect's parent
            selectionRect.SetActive(false); // Disable the rect
            UpgradeButton.SetActive(false); // Disable the upgrade button
            UpgradeButton.transform.position = new Vector3(1000, 1000);
            return;
        }

        // Then hook us up with the current stuff
        this.selected = selected;
        this.selectable = selected.GetComponent<ISelectable>();
        selectionRect.SetActive(true);
        selectionRect.transform.parent = selected.transform;
        selectionRect.transform.localPosition = new Vector3(0, 0, -5);

        // If the selected thing has the ability to upgrade
        if(selected.GetComponent<IUpgradeable>() is IUpgradeable upgradeable)
        {
            // Clear listeners
            UpgradeButton.GetComponentInChildren<Button>().onClick.RemoveAllListeners();
            // Add a listener that tries to upgrade
            UpgradeButton.GetComponentInChildren<Button>().onClick.AddListener(() => TryToUpgrade(upgradeable));

            UpgradeButton.transform.GetChild(0).Find("Price").GetComponent<TMPro.TextMeshProUGUI>().text = upgradeable.GetUpgradeCost().ToString();

            //Move and display the upgrade button next to the object
            UpgradeButton.SetActive(true);
            UpgradeButton.transform.position = selected.transform.position + new Vector3(0,-1,0);

        }
    }

    void TryToUpgrade(IUpgradeable upgradeable)
    {
        // If we have enough money
        if (upgradeable.CanUpgrade() && PlayerData.Instance.CanAfford(upgradeable.GetUpgradeCost()))
        {
            //Spend the money
            PlayerData.Instance.Spend(upgradeable.GetUpgradeCost());

            //Upgrade
            upgradeable.Upgrade();

            var cost = upgradeable.GetUpgradeCost();
            var text = cost >= 0 ? cost.ToString() : "-";
            UpgradeButton.transform.GetChild(0).Find("Price").GetComponent<TMPro.TextMeshProUGUI>().text = text;

        }
    }

   
}
