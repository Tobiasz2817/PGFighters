using System;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public abstract class MainPanel : MonoBehaviour
{
    public Action OnPanelSelection;
    public Action OnPanelDeselection;
    public MainPanels myType;
    
    private CanvasGroup _canvasGroup;

    [Header("Canvas Group Settings")] 
    [SerializeField]
    protected float alphaDuration = 0.8f;
    

    protected virtual void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    protected virtual void Start()
    {
       
    }
    public async Task SelectionPanel()
    {
        SetInteractable(true);
        SetBlockRaycasts(true);
        
        await SmoothSelectionPanel(alphaDuration);
        
        OnSelectionPanel();
    }
    public async Task DeselectionPanel()
    {
        await SmoothDeselectionPanel(alphaDuration);
        
        OnDeselectionPanel();
        
        SetInteractable(false);
        SetBlockRaycasts(false);
    }

    protected abstract void OnSelectionPanel();
    protected abstract void OnDeselectionPanel();
    
    private async Task SmoothSelectionPanel(float alphaDuration)
    {
        _canvasGroup.DOFade(1, alphaDuration);
        await Task.Delay((int)(alphaDuration * 1000));
    }
    private async Task SmoothDeselectionPanel(float alphaDuration)
    {
        var deselectionAlphaDuration = alphaDuration / 2;
        _canvasGroup.DOFade(0, deselectionAlphaDuration);
        await Task.Delay((int)(deselectionAlphaDuration * 1000));
    }
    private void SetBlockRaycasts(bool isRaycasts)
    {
        _canvasGroup.blocksRaycasts = isRaycasts;
    }
    private void SetInteractable(bool isInteractable)
    {
        _canvasGroup.interactable = isInteractable;
    }
}

