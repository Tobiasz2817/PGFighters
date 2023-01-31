using UnityEngine;



public class PanelActivity : MonoBehaviour
{
    public static PanelActivity Instance;
    
    private Panel[] _panels;
    private Panel currentSelectedPanel;
    
    private MainPanel[] _mainPanels;
    private MainPanel currentSelectedMainPanel;
    
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
    public void MoveTo(Panels typePanel)
    {
        var newPanel = FindPanels(typePanel);
        if (!newPanel) return;
        if (currentSelectedPanel == newPanel) return;
        
        if(currentSelectedPanel) currentSelectedPanel.OnPanelDeselection?.Invoke();
        newPanel.OnPanelSelection?.Invoke();
        
        currentSelectedPanel = newPanel;
    }
    public void MoveTo(MainPanels mainTypePanel)
    {
        var newPanel = FindPanels(mainTypePanel);
        if (!newPanel) return;
        if (currentSelectedMainPanel == newPanel) return;
        
        Debug.Log("Invoke Button handler");
        
        if(currentSelectedMainPanel) currentSelectedMainPanel.OnPanelDeselection?.Invoke();
        newPanel.OnPanelSelection?.Invoke();
        
        currentSelectedMainPanel = newPanel;
        
        
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
}
