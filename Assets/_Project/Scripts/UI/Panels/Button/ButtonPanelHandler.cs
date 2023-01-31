using System;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPanelHandler : MonoBehaviour
{
    public Panels _panel;
    public static Action<Panels> OnButtonClick;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(SelectionPanel);
    }
    private void SelectionPanel()
    {
        OnButtonClick?.Invoke(_panel);
    }
    
}
