using System;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;


public abstract class Panel : MonoBehaviour
{
    public Panels myType;
    
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
        await SmoothSelectionPanel();

        OnSelectionPanel();
    }
    public async Task DeselectionPanel()
    {
        OnDeselectionPanel();
        
        SetInteractable(false);
        SetBlockRaycasts(false);
        await SmoothDeselectionPanel();
    }

    protected abstract void OnSelectionPanel();
    protected abstract void OnDeselectionPanel();
    
    private async Task SmoothSelectionPanel()
    {
        _canvasGroup.DOFade(1, alphaDuration);
        await Task.Delay((int)(alphaDuration * 1000) - 100);
    }
    private async Task SmoothDeselectionPanel()
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
