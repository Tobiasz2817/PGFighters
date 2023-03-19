using System;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPanelHandler : MonoBehaviour
{
    public Panels _panel;
    public Button button;
    public static Action<Panels> OnButtonClick;

    private void Awake() {
        button = GetComponent<Button>();
        button.onClick.AddListener(SelectionPanel);
    }
    private void SelectionPanel()
    {
        OnButtonClick?.Invoke(_panel);
    }
}
