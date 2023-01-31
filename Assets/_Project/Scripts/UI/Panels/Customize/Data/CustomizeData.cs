using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizeData : MonoBehaviour
{
    #region Input Data
    [Serializable]
    public class MainPart
    {
        public string nameMainPart; 
        public List<SecoundPart> secoundPartList;
    }
    [Serializable]
    public class SecoundPart
    {
        public string nameFirstPart;
        public List<Element> partList;
    }

    [Serializable]
    public class Element
    {
        public GameObject prefab;
        public Sprite imagePrefab;
    }

    [SerializeField]
    private List<MainPart> customizeLists = new List<MainPart>();
    #endregion

    public List<MainPart> GetCustomizeList()
    {
        return customizeLists;
    }

    public IEnumerable<string> StringContents()
    {
        foreach (var contentName in customizeLists)
        {
            yield return contentName.nameMainPart;
        }
    }
}
