using System;
using System.Collections;
using System.Collections.Generic;
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

        foreach (var child in compo)
            foreach (var contentKey in CustomizeCharacterEquipmentData.Instance.GetHeaders())
                if (child.name == contentKey || child.name == "+ R Hand" || child.name == "+ L Hand") 
                    if(!tmp.ContainsKey(child.name))
                        tmp.Add(child.name, child);
        return tmp;
    }

    public Transform GetContent(string key) {
        return contents[key];
    }
}
