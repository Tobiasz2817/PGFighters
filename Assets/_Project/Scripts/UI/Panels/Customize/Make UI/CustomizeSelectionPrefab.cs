using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class CustomizeSelection
{
    public string contentName;
    public string secoundPart;
    public int index;
    
    public CustomizeSelection(string content, string secoundPart, int index)
    {
        this.contentName = content;
        this.secoundPart = secoundPart;
        this.index = index;
    }
}
public class CustomizeSelectionPrefab : MonoBehaviour
{
    private CustomizeSelection customizeSelection;

    public GameObject customizePrefab;
    public static event Action<CustomizeSelection,GameObject> OnCustomizeSelectionPart;
    public static event Action<CustomizeSelection,GameObject> OnCustomizeSelectionColor;
    
    [SerializeField] private Button buttonHandler;
    [SerializeField] private Image partImage;
    
    public void Setup(CustomizeSelection customizeSelection,Sprite sprite, GameObject customizePrefab)
    {
        SetCustomizeSelection(customizeSelection);
        SetImageSprite(sprite);
        SetPrefab(customizePrefab);
        AddButtonListner();
    }
    private void AddButtonListner()
    {
        if (customizeSelection.secoundPart == "Color")
            buttonHandler.onClick.AddListener(() => { OnCustomizeSelectionColor?.Invoke(customizeSelection,customizePrefab);});
        else
            buttonHandler.onClick.AddListener(() => { OnCustomizeSelectionPart?.Invoke(customizeSelection,customizePrefab);});
    }

    private void SetCustomizeSelection(CustomizeSelection customizeSelection) => this.customizeSelection = customizeSelection;
    private void SetImageSprite(Sprite spirte) => partImage.sprite = spirte;
    private void SetPrefab(GameObject prefab) => this.customizePrefab = prefab;
}
