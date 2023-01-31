using System;
using TMPro;
using UnityEngine;
using Maz.String;

// TODO: Add Polling prefabs
public class CustomizePrefab : MonoBehaviour
{
    [SerializeField] private TMP_Text namePartText;
    [SerializeField] private Transform bodyContent;
    [SerializeField] private GameObject buttonHandler;
    
    private GameObject currentPrefab;

    public void SetupPrefab(string namePart)
    {
        namePartText.text = namePart;
    }
    public void SetupChild(CustomizeSelection customizeSelection,Sprite sprite,GameObject prefab)
    {
        var child = Instantiate(buttonHandler, bodyContent);
        var selectionHandler = child.GetComponent<CustomizeSelectionPrefab>();
        selectionHandler.Setup(customizeSelection, sprite,prefab);
    }
}
