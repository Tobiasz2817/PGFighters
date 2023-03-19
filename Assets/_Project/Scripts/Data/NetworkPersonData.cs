using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class NetworkPersonData : MonoBehaviour, IDataInstances 
{
    public static NetworkPersonData Instance;

    private string nickName;
    public string NickName
    {
        set {
            if(value != string.Empty)
                if (value.Length > 4 && value.Length <= 12) {
                    SavePersonData(value); 
                    nickName = value;
                }
        }
        get => nickName;
    }

    public const string KEY_PERSON_DATA = "KEY_PERSON_DATA";
    public const string KEY_PERSON_FILE_NAME = "KEY_PERSON_FILE_NAME";
    
    private string LoadNickNamePerson() {
        var data = SaveManager.LoadDates<string>(KEY_PERSON_DATA, KEY_PERSON_FILE_NAME);
        if (data.Count != 0)
            return data[0];

        return string.Empty;
    }
    private void SavePersonData(string nickName_) {
        SaveManager.SaveDates<string>(KEY_PERSON_DATA,new List<string>() {nickName_} , KEY_PERSON_FILE_NAME);
    }

    public bool NickNameRules(string nickName_) {
        if (!string.IsNullOrEmpty(nickName_) && nickName_.Length > 4 && nickName_.Length <= 10)
            return true;

        return false;
    }

    public Task IsDone() {
        nickName = LoadNickNamePerson();
        Instance = this;
        return Task.Delay(1);
    }
}
