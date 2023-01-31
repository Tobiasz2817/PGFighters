using System; 
using System.Collections.Generic; 
using System.IO; 
using System.Linq; 
using System.Runtime.Serialization; 
using Unity.VisualScripting; 
using UnityEngine; 
public class SaveManager 
{ 
    [Serializable] 
    private class Wrapper<T> 
    { 
        public SerializedDictionary<string, Item<T>> Items = new SerializedDictionary<string, Item<T>>(); 
    } 
     
    [Serializable] 
    public class Item<T> 
    { 
        public List<T> Items = new List<T>(); 
    } 
     
    private static string directory = "SaveData"; 
    private static string typeFile = ".json"; 
    public static void SaveDates<T>(string key,List<T> newDates,string fileName,ModificationType modType = ModificationType.Append) 
    {
        if (!DirectoryExists()) 
            Directory.CreateDirectory(directory); 
        Wrapper<T> wrapper = null; 
        switch (modType) 
        { 
            case ModificationType.Replaced: 
                wrapper = CreateData<T>(key, newDates, fileName); 
                break; 
            case ModificationType.Append: 
                wrapper = ModificateData<T>(key, newDates, fileName); 
                break; 
        } 
         
        FileStream dataStream = new FileStream(GetFullPath(fileName), FileMode.Create); 
        using BinaryWriter writer = new BinaryWriter(dataStream); 
        var jsonString = JsonUtility.ToJson(wrapper, true); 
        writer.Write(jsonString); 
         
        dataStream.Close(); 
    } 
    private static Wrapper<T> CreateData<T>(string key,List<T> newDates,string fileName) 
    { 
        Wrapper<T> wrapper = new Wrapper<T>(); 
         
        if (SaveExsist(fileName)) 
            wrapper.Items = GetFile<T>(fileName);
        if (wrapper.Items.ContainsKey(key)) 
            wrapper.Items.Remove(key);
        
        wrapper.Items.Add(key,new Item<T>() { Items = newDates } ); 
        return wrapper; 
    } 
    private static Wrapper<T> ModificateData<T>(string key,List<T> newDates,string fileName) 
    { 
        Wrapper<T> wrapper = new Wrapper<T>(); 
         
        if (SaveExsist(fileName)) 
            wrapper.Items = GetFile<T>(fileName); 
        if (!wrapper.Items.ContainsKey(key)) 
            wrapper.Items.Add(key, new Item<T>() { Items = newDates } ); 
        else 
            foreach (var data in newDates) 
                wrapper.Items[key].Items.Add(data); 
         
        return wrapper; 
    } 
    public static List<T> LoadDates<T>(string key,string fileName) 
    { 
        if (SaveExsist(fileName)) 
        {    
            try 
            { 
                FileStream dataStream = new FileStream(GetFullPath(fileName), FileMode.Open); 
                using BinaryReader reader = new BinaryReader(dataStream); 
                var jsonString = reader.ReadString(); 
                Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(jsonString); 
                dataStream.Close(); 
                 
                if (wrapper.Items.ContainsKey(key)) return wrapper.Items[key].Items; 
            } 
            catch (SerializationException exc) 
            { 
                Debug.Log("Failed to load file"); 
            } 
        } 
         
         
        return new List<T>(){}; 
    } 
    private static SerializedDictionary<string,Item<T>> GetFile<T>(string fileName) 
    { 
        if (SaveExsist(fileName)) 
        {    
            try 
            { 
                FileStream dataStream = new FileStream(GetFullPath(fileName), FileMode.Open); 
                using BinaryReader reader = new BinaryReader(dataStream); 
                var jsonString = reader.ReadString(); 
                Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(jsonString); 
                dataStream.Close(); 
                return wrapper.Items; 
            } 
            catch (SerializationException exc) 
            { 
                Debug.Log("Failed to load file"); 
            } 
        } 
        return new SerializedDictionary<string, Item<T>>(); 
    } 
    public static bool SaveExsist(string fileName) 
    { 
        return File.Exists(GetFullPath(fileName)); 
    } 
    private static bool DirectoryExists() 
    { 
        return Directory.Exists(directory); 
    } 
    private static string GetFullPath(string fileName)  
    { 
        return directory + "/" + fileName + typeFile; 
    } 
} 
public enum ModificationType  
{ 
    Replaced, 
    Append 
}