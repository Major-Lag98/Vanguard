using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpeedControlGroup : MonoBehaviour
{

    Button[] buttons;
    // Start is called before the first frame update
    void Start()
    {
        buttons = GetComponentsInChildren<Button>();
        foreach (var button in buttons)
        {
            button.onClick.AddListener(() => { ClearButtons(); SetSelectedButton(button); });
        }
        
    }

    void ClearButtons()
    {
        foreach (var button in buttons)
        {
            button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
        }
    }

    void SetSelectedButton(Button button)
    {
        button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.yellow;
    }
}
