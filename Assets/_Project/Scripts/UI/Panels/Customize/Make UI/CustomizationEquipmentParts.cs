using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizationEquipmentParts : MonoBehaviour
{
    [SerializeField] 
    private CustomizeData _customizeData;

    [SerializeField]
    private GameObject _customizePrefab;
    public static event Action OnInterfaceCustomizationCreated;
    void Start()
    {
        InstancesChilds();
    }

    private void InstancesChilds() {
        CustomizeCharacterEquipmentData.Instance.Clear();
        
        int x = 0;
        foreach (var mainPart in _customizeData.GetCustomizeList())
            foreach (var secoundPart in mainPart.secoundPartList)
            {
                var tmp = Instantiate(_customizePrefab, transform);
                var customizePrefab = tmp.GetComponent<CustomizePrefab>();
                customizePrefab.SetupPrefab(secoundPart.nameFirstPart);

                if (mainPart.nameMainPart == "+ Color" && CustomizationLoader.BasicCustomizationColor == 0)
                    CustomizationLoader.BasicCustomizationColor = x;
                
                foreach (var element in secoundPart.partList)
                {
                    customizePrefab.SetupChild(new CustomizeSelection(mainPart.nameMainPart,secoundPart.nameFirstPart,x), element.imagePrefab,element.prefab);
                    CustomizeCharacterEquipmentData.Instance.SetElementsCustomizedCharacter(x,element.prefab);
                    x++;
                }
            }
        
        OnInterfaceCustomizationCreated?.Invoke();
    }
}
