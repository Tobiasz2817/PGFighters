using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;

public class CustomizeCharacterEquipment : MonoBehaviour
{
    private readonly List<CustomizeSelectionTmp> currentEquipment = new List<CustomizeSelectionTmp>();
    private readonly Dictionary<string,Transform> contents = new Dictionary<string, Transform>();

    [SerializeField] 
    private CustomizeData _customizeData;
    [SerializeField] 
    private SkinnedMeshRenderer meshRenderer;

    private void Awake()
    {
        FindContents(transform);
    }
    
    private void OnEnable()
    {
        CustomizeSelectionPrefab.OnCustomizeSelectionPart += EquipPart;
        CustomizePanel.OnSaveChanges += SaveChanges;
        CustomizationLoader.OnCustomizeSelected += EquipLoad;
        CustomizationLoader.OnCustomizeSelected += CustomizeCharacterEquipmentData.Instance.SaveCurrentLoad;
        CustomizeSelectionPrefab.OnCustomizeSelectionColor += EquipColor;
        CustomizationLoader.OnLoadedCustomizations += DisableDisplayCharacter;
    }

    private void OnDisable()
    {
        CustomizeSelectionPrefab.OnCustomizeSelectionPart -= EquipPart;
        CustomizePanel.OnSaveChanges -= SaveChanges;
        CustomizationLoader.OnCustomizeSelected -= EquipLoad;
        CustomizationLoader.OnCustomizeSelected -= CustomizeCharacterEquipmentData.Instance.SaveCurrentLoad;
        CustomizeSelectionPrefab.OnCustomizeSelectionColor -= EquipColor;
        CustomizationLoader.OnLoadedCustomizations -= DisableDisplayCharacter;
    }

    private void FindContents(Transform reserachPrefab)   
    {
        if (!reserachPrefab) throw new Exception("Customize Prefab are null");
        
        var compo = reserachPrefab.transform.GetComponentsInChildren<Transform>(false);

        foreach (var child in compo)
            foreach (var contentKey in _customizeData.GetCustomizeList())
                if (child.name == contentKey.nameMainPart)
                {
                    contents.Add(contentKey.nameMainPart,child);
                    foreach (var secoundPart in contentKey.secoundPartList)
                        currentEquipment.Add(new CustomizeSelectionTmp(new CustomizeSelection(contentKey.nameMainPart,secoundPart.nameFirstPart,-1),null));
                }
    }

    private void EquipPart(CustomizeSelection customizeSelection, GameObject prefab){
        for (int i = 0; i < currentEquipment.Count; i++) {
            if (currentEquipment[i].customizeSelection.secoundPart != customizeSelection.secoundPart) continue;
            if (currentEquipment[i].prefab != null)
                Destroy(currentEquipment[i].prefab);

            GameObject newObject = Instantiate(prefab,contents[currentEquipment[i].customizeSelection.contentName]);
            currentEquipment[i] = new CustomizeSelectionTmp(customizeSelection,newObject);
        }
    }
    private void EquipColor(CustomizeSelection customizeSelection, GameObject prefab) {
        for (int i = 0; i < currentEquipment.Count; i++) {
            if (currentEquipment[i].customizeSelection.secoundPart != customizeSelection.secoundPart) continue;
            var material = Instantiate(prefab);
            meshRenderer.material = material.GetComponent<SkinnedMeshRenderer>().material;
            Destroy(material);
            currentEquipment[i] = new CustomizeSelectionTmp(customizeSelection,null);
        }
    }
    private void SaveChanges() {
        List<CustomizeSelection> customizeSelections = new List<CustomizeSelection>();

        foreach (var current in currentEquipment) 
            customizeSelections.Add(current.customizeSelection);

        SaveCurrentLoad(customizeSelections);
        SaveManager.SaveDates(CustomizeCharacterEquipmentData.Instance.currentKeySelectedCustomization,customizeSelections,CustomizationLoader.CUSTOMIZE_FILE,ModificationType.Replaced);
    }

    public void ClearSaves() {
        foreach (var equipment in currentEquipment) {
            if(equipment.prefab != null)
                Destroy(equipment.prefab);
            equipment.customizeSelection.index = -1;
            equipment.prefab = null;
        }
    }
    public void EquipLoad(List<CustomizeSelection> customizationEquipmetns) {
        ClearSaves();

        foreach (var current in customizationEquipmetns) {
            if(current.index == -1 && current.contentName == "+ Color")
                EquipColor(new CustomizeSelection(current.contentName,"Color",CustomizationLoader.BasicCustomizationColor),CustomizeCharacterEquipmentData.Instance.GetCustomizePrefab(CustomizationLoader.BasicCustomizationColor));
            if(current.index == -1) continue;

            if (current.contentName != "+ Color") 
                EquipPart(current);
            else 
                EquipColor(current);
        }
    }

    private void EquipColor(CustomizeSelection current) {
        var newObject =
            Instantiate(
                CustomizeCharacterEquipmentData.Instance.GetCustomizePrefab(current.index));

        var customizeSelectionTmp = GetCustomizeElement(current.secoundPart);
        if (customizeSelectionTmp == null) return;
        meshRenderer.material = newObject.GetComponent<SkinnedMeshRenderer>().material;
        Destroy(newObject);
        customizeSelectionTmp.customizeSelection.index = current.index;
    }

    private void EquipPart(CustomizeSelection current) {
        var newObject =
            Instantiate(
                CustomizeCharacterEquipmentData.Instance.GetCustomizePrefab(current.index),
                contents[current.contentName]);

        var customizeSelectionTmp = GetCustomizeElement(current.secoundPart);
        if (customizeSelectionTmp == null) return;
        customizeSelectionTmp.prefab = newObject;
        customizeSelectionTmp.customizeSelection.index = current.index;
    }

    private void SaveCurrentLoad(List<CustomizeSelection> obj) {
        CustomizeCharacterEquipmentData.Instance.ClearCurrentEquipment();
        foreach (var equipment in obj) 
            CustomizeCharacterEquipmentData.Instance.SetCurrentEquipment(equipment);
    }
    private CustomizeSelectionTmp GetCustomizeElement(string secoundPart) {
        foreach (var element in currentEquipment) 
            if (element.customizeSelection.secoundPart == secoundPart)
                return element;
        
        return null;
    }

    public void DisableDisplayCharacter() {
        this.meshRenderer.enabled = false;
        
        foreach (var equipment in currentEquipment) 
            if(equipment.prefab != null)
                equipment.prefab.SetActive(false);
    }

    public void EnableDisplayCharacter() {
        this.meshRenderer.enabled = true;
        
        foreach (var equipment in currentEquipment) 
            if(equipment.prefab != null)
                equipment.prefab.SetActive(true);
    }
}

public class CustomizeSelectionTmp {
    public CustomizeSelection customizeSelection;
    public GameObject prefab;

    public CustomizeSelectionTmp(CustomizeSelection customizeSelection, GameObject prefab) {
        this.customizeSelection = customizeSelection;
        this.prefab = prefab;
    }
}