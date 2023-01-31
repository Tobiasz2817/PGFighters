using System.Collections.Generic;  
using UnityEngine;  
using Debug = UnityEngine.Debug;  
[System.Serializable] 
public class SerializedDictionary<TKey, TValue>  
{  
    [SerializeField, HideInInspector] private List<TKey> keyData;  
    [SerializeField, HideInInspector] private List<TValue> valueData; 
    public TValue this[TKey key]  
    { 
        get  
        {  
            int index = FindIndex(key);  
            if (index >= 0)  
                return valueData[index];  
            throw new KeyNotFoundException(key.ToString());  
        }  
        set { Insert(key, value); }  
    } 
     
    public IEnumerator<TKey> Keys() 
    { 
        foreach (var key in keyData) 
        { 
            yield return key; 
        } 
    } 
    public SerializedDictionary() 
    { 
        keyData = new List<TKey>(); 
        valueData = new List<TValue>(); 
    } 
    public void Add(TKey key, TValue value) 
    { 
        keyData.Add(key); 
        valueData.Add(value); 
    } 
    public bool Remove(TKey key)  
    {  
        int index = FindIndex(key);  
        if (index < 0)  
            return false; 
        keyData.Remove(keyData[index]); 
        valueData.Remove(valueData[index]); 
         
        return true;  
    }  
    public bool ContainsKey(TKey key)  
    {  
        if (FindIndex(key) != -1)  
            return true;  
        return false;  
    }  
      
    public bool TryGetValue(TKey key, out TValue value)  
    {  
        value = default;  
          
        int index = FindIndex(key);  
        if (index < 0)  
            return false;  
        value = valueData[index];  
        return true;  
    }  
      
    public int Count { get => keyData.Count; } 
     
    public void Clear()  
    {  
        keyData.Clear(); 
        valueData.Clear(); 
    } 
     
    private void Insert(TKey key, TValue value)  
    {  
        int index = FindIndex(key);  
        if (index < 0)  
            throw new KeyNotFoundException(key.ToString());  
          
        valueData[index] = value;  
    }  
    private int FindIndex(TKey key)  
    { 
        for (int i = 0; i < keyData.Count; i++) 
            if (keyData[i].Equals(key)) 
                return i; 
         
        return -1;  
    } 
}
