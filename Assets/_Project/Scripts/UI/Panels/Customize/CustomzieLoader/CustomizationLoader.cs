using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.UI;

public class CustomizationLoader : MonoBehaviour
{
    public const string CUSTOMIZE_FILE = "CUSTOMIZE_FILE";
    public const string CUSTOMIZE_KEY = "CUSTOMIZE_PART_";

    public const string CUSTOMIZE_CURRENT_SELECTED = "CUSTOMIZE_CURRENT_SELECTED";
    public const string CUSTOMIZE_CURRENT_SELECTED_FILE = "CUSTOMIZE_CURRENT_SELECTED_FILE";

    public static int BasicCustomizationColor;
    
    private string lastCustomizeKey = "";
    
    private Button[] customizeButtons;
    
    public static event Action<List<CustomizeSelection>> OnCustomizeSelected;
    public static event Action OnLoadedCustomizations;

    [SerializeField] 
    private CustomizeData _customizeData;
    private void Awake() {
        customizeButtons = GetComponentsInChildren<Button>();
        
        customizeButtons[0].onClick.AddListener(() => ButtonSelected(CUSTOMIZE_KEY + 1));
        customizeButtons[1].onClick.AddListener(() => ButtonSelected(CUSTOMIZE_KEY + 2));
        customizeButtons[2].onClick.AddListener(() => ButtonSelected(CUSTOMIZE_KEY + 3));
    }
    
    private void LoadCustomizationElements() {
        SelectionKeyCheck();
        CustomizationCheck();
        OnLoadedCustomizations?.Invoke();
    }

    private void OnEnable() {
        CustomizePanel.OnEnableCustomize += ButtonSelected;
        CustomizationEquipmentParts.OnInterfaceCustomizationCreated += LoadCustomizationElements;
    }

    private void OnDisable() {
        CustomizePanel.OnEnableCustomize -= ButtonSelected;
        CustomizationEquipmentParts.OnInterfaceCustomizationCreated -= LoadCustomizationElements;
    }

    #region Customization
    private void CreateCustomizationLoaders() {
        CreateCustomization(CUSTOMIZE_KEY + 1);
        CreateCustomization(CUSTOMIZE_KEY + 2);
        CreateCustomization(CUSTOMIZE_KEY + 3);
    }
    
    private void CreateCustomization(string key) {
        List<CustomizeSelection> customizeSelections = new List<CustomizeSelection>();
        foreach (var contentKey in _customizeData.GetCustomizeList())
        foreach (var secoundPart in contentKey.secoundPartList) 
            customizeSelections.Add(new CustomizeSelection(contentKey.nameMainPart,secoundPart.nameFirstPart,-1));
        
        SaveManager.SaveDates(key,customizeSelections,CUSTOMIZE_FILE,ModificationType.Replaced);
    }

    private void CustomizationCheck() {
        if (!SaveManager.SaveExsist(CUSTOMIZE_FILE)) 
            CreateCustomizationLoaders();

        var customization = SaveManager.LoadDates<CustomizeSelection>(CustomizeCharacterEquipmentData.Instance.currentKeySelectedCustomization,CUSTOMIZE_FILE);
        OnCustomizeSelected?.Invoke(customization);
    }

    #endregion
    

    #region SelectionKey

    private void SelectionKeyCheck() {

        string selectionKey = IsSelectionKey();
        if (string.IsNullOrEmpty(selectionKey))
            CreateSelectionKey(CUSTOMIZE_KEY + 1);
        else
            CreateSelectionKey(selectionKey);
    }
    private void CreateSelectionKey(string key) {
        lastCustomizeKey = key;
        CustomizeCharacterEquipmentData.Instance.currentKeySelectedCustomization = key;
        SaveManager.SaveDates(CUSTOMIZE_CURRENT_SELECTED,new List<string>() { key },CUSTOMIZE_CURRENT_SELECTED_FILE,ModificationType.Replaced);
    }
    private string IsSelectionKey() {
        var key = SaveManager.LoadDates<string>(CUSTOMIZE_CURRENT_SELECTED,CUSTOMIZE_CURRENT_SELECTED_FILE);

        return key.Count <= 0 ? "" : key[0];
    }
    #endregion
    
    private void ButtonSelected(string customizeKey) {
        if (customizeKey == lastCustomizeKey) return;

        lastCustomizeKey = customizeKey;
        CustomizeCharacterEquipmentData.Instance.currentKeySelectedCustomization = customizeKey;
        SaveManager.SaveDates(CUSTOMIZE_CURRENT_SELECTED,new List<string>() { customizeKey },CUSTOMIZE_CURRENT_SELECTED_FILE,ModificationType.Replaced);
        
        var customization = SaveManager.LoadDates<CustomizeSelection>(customizeKey,CUSTOMIZE_FILE);
        
        OnCustomizeSelected?.Invoke(customization);
    }
}
