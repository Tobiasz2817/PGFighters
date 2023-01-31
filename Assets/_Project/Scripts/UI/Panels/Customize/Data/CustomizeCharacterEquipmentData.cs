using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CustomizeCharacterEquipmentData : MonoBehaviour, IDataInstances
{
    public static CustomizeCharacterEquipmentData Instance;
    private void CreateInstances()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(this);
    }

    #region Customize Data Operations

    private readonly Dictionary<int, GameObject> customizeCharacterDict = new Dictionary<int, GameObject>();
    public void SetElementsCustomizedCharacter(int index, GameObject gameObject)
    {
        customizeCharacterDict.Add(index,gameObject);
    }
    public GameObject TryGetCustomizePrefab(int index)
    {
        GameObject pref = null;
        if (customizeCharacterDict.TryGetValue(index, out pref))
            return pref;
        
        return pref;
    }
    public GameObject GetCustomizePrefab(int index)
    {
        return index < customizeCharacterDict.Count && index > 0 ? customizeCharacterDict[index] : null;
    }
    #endregion

    #region Equipment Data Operations

    private readonly List<CustomizeSelection> currentEquipment = new List<CustomizeSelection>();
    public string currentKeySelectedCustomization;
    
    public void SetCurrentEquipment(CustomizeSelection customizeEquipmentPart)
    {
        currentEquipment.Add(customizeEquipmentPart);
    }
    public void SaveCurrentLoad(List<CustomizeSelection> obj) {
       ClearCurrentEquipment();
        foreach (var equipment in obj) 
            SetCurrentEquipment(equipment);
    }
    public void ClearCurrentEquipment()
    {
        currentEquipment.Clear();
    }
    public IEnumerable<string> GetHeaders()
    {
        foreach (var customizeEquipmentPart in currentEquipment)
            yield return customizeEquipmentPart.contentName;
    }
    public IEnumerable<CustomizeSelection> GetCustomizeSelections()
    {
        foreach (var customizeEquipmentPart in currentEquipment)
            yield return customizeEquipmentPart;
    }
    
    #endregion
    
    public Task IsDone()
    {
        CreateInstances();
        return Task.Delay(1);
    }
}