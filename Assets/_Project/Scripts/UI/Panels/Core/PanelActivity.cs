using UnityEngine;
using UnityEngine.UI;


public class PanelActivity : MonoBehaviour
{
    public static PanelActivity Instance;
    
    private Panel[] _panels;
    private Panel currentSelectedPanel;
    
    private MainPanel[] _mainPanels;
    
    [SerializeField]
    private MainPanel currentSelectedMainPanel;
    
    [SerializeField]
    private ButtonPanelHandler[] _buttonsHandler;

    private void Awake()
    {
        Instance = this;
        _panels = GetComponentsInChildren<Panel>();
        _mainPanels = GetComponentsInChildren<MainPanel>();
        
    }

    private void OnEnable()
    {
        ButtonPanelHandler.OnButtonClick += MoveTo;
    }
    private void OnDisable()
    {
        ButtonPanelHandler.OnButtonClick -= MoveTo;
    }
    public async void MoveTo(Panels typePanel) {
        var newPanel = FindPanels(typePanel);
        if (!newPanel) return;
        if (currentSelectedPanel == newPanel) return;
        ChangeInteractionButtons(false);
        
        if(currentSelectedPanel) await currentSelectedPanel.DeselectionPanel();
        currentSelectedPanel = newPanel;
        await currentSelectedPanel.SelectionPanel();

        ChangeInteractionButtons(true);
    }
    public async void MoveTo(MainPanels mainTypePanel) {
        
        var newPanel = FindPanels(mainTypePanel);
        if (!newPanel) return;
        if (currentSelectedMainPanel == newPanel) return;

        if(currentSelectedMainPanel) await currentSelectedMainPanel.DeselectionPanel();
        currentSelectedMainPanel = newPanel;
        await currentSelectedMainPanel.SelectionPanel();
    }
    Panel FindPanels(Panels typePanel)
    {
        foreach (Panel panel in _panels)
            if (panel.myType == typePanel)
                return panel;

        return null;
    }
    MainPanel FindPanels(MainPanels typePanel)
    { 
        foreach (MainPanel panel in _mainPanels)
            if (panel.myType == typePanel)
                return panel;

        return null;
    }

    private void ChangeInteractionButtons(bool isActive) {
        foreach (var buttons in _buttonsHandler) 
            buttons.button.interactable = isActive;
    }
}
