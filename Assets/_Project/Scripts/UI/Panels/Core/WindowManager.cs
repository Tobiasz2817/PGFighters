using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
    void Start()
    {
        PanelActivity.Instance.MoveTo(Panels.MainPanel);
    }
}
public enum Panels
{
    NonePanel,
    MainPanel,
    CreatePanel,
    LobbiesPanel,
    WaitingPanel,   
    CustomizePanel,
    OptionsPanel,
}

public enum MainPanels
{
    AuthenticationPanel,
    MainMenuPanel
}