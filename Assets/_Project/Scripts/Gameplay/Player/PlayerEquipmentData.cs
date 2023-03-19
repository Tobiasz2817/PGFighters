using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.Netcode;
using UnityEngine;

public class PlayerEquipmentData : NetworkBehaviour
{
    private Dictionary<string, Transform> contents = new Dictionary<string, Transform>();

    [SerializeField] private Transform body;
    
    public void Awake() {
        Debug.Log("Finding conetnts");
        contents = FindContents();
    }
    
    private Dictionary<string, Transform> FindContents() {
        Dictionary<string, Transform> tmp = new Dictionary<string, Transform>();
        var compo = body.transform.GetComponentsInChildren<Transform>();

        if (CustomizeCharacterEquipmentData.Instance == null) return null;
        
        foreach (var child in compo)
            if (child.name == "+ R Hand" || child.name == "+ L Hand" || CustomizeCharacterEquipmentData.Instance != null && CustomizeCharacterEquipmentData.Instance.GetHeaders().Contains(child.name)) 
                if(!tmp.ContainsKey(child.name))
                    tmp.Add(child.name, child);

        return tmp;
    }

    public Transform GetContent(string key) {
        return contents[key];
    }
}
