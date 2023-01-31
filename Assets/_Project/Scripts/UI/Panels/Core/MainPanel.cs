using System;
using DG.Tweening;
using UnityEngine;

public class MainPanel : MonoBehaviour
{
    public Action OnPanelSelection;
    public Action OnPanelDeselection;
    public MainPanels myType;
    
    private CanvasGroup _canvasGroup;

    [Header("Canvas Group Settings")] 
    [SerializeField]
    protected float alphaDuration = 0.8f;

    [HideInInspector]
    public bool isCurrentActive = false;

    protected virtual void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    protected virtual void Start()
    {
       
    }

    public void OnEnable()
    {
        OnPanelSelection += OnSelectionPanel;
        OnPanelDeselection += OnDeselectionPanel;
    }

    public void OnDisable()
    {
        OnPanelSelection -= OnSelectionPanel;
        OnPanelDeselection -= OnDeselectionPanel;
    }
    protected virtual void OnSelectionPanel()
    {
        SetInteractable(true);
        SetBlockRaycasts(true);
        SmoothSelectionPanel();

        isCurrentActive = true;
    }
    protected virtual void OnDeselectionPanel()
    {
        SetInteractable(false);
        SetBlockRaycasts(false);
        SmoothDeselectionPanel();

        isCurrentActive = false;
    }
    private void SmoothSelectionPanel()
    {
        _canvasGroup.DOFade(1, alphaDuration);
    }
    private async void SmoothDeselectionPanel()
    {
        _canvasGroup.DOFade(0, alphaDuration / 2);
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