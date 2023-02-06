using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class CustomizePanel : Panel
{
    [SerializeField] private CustomizeCharacterEquipment customizeCharacterEquipment;
    [SerializeField] private Button button;
    public static event Action OnSaveChanges;
    public static event Action<string> OnEnableCustomize;

    protected override void Awake() {
        base.Awake();
        SetCurrentSelectedKey();
    }

    protected override void Start() {    
        base.Start();
        button.onClick.AddListener(() => OnSaveChanges?.Invoke());
    }
    protected override void OnSelectionPanel() {
        customizeCharacterEquipment.EnableDisplayCharacter();
        
        OnEnableCustomize?.Invoke(CustomizeCharacterEquipmentData.Instance.currentKeySelectedCustomization);
    }
    protected override void OnDeselectionPanel() {
        customizeCharacterEquipment.DisableDisplayCharacter();
    }
    
    private void SetCurrentSelectedKey() {
        var currentKeySelected = SaveManager.LoadDates<string>(CustomizationLoader.CUSTOMIZE_CURRENT_SELECTED,CustomizationLoader.CUSTOMIZE_CURRENT_SELECTED_FILE);
        if (currentKeySelected.Count > 0) {
            CustomizeCharacterEquipmentData.Instance.currentKeySelectedCustomization = currentKeySelected[0];
        }
        else {
            CustomizeCharacterEquipmentData.Instance.currentKeySelectedCustomization = CustomizationLoader.CUSTOMIZE_KEY + 1;
        }
    }
}
