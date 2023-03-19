using System;
using System.Linq;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InstancesManager : MonoBehaviour
{
    [SerializeField] private SceneReference menuScene;
    [SerializeField] private SceneReference userScene;
    [SerializeField] private Slider slider;
    
    private async void Awake() {
        if (slider == null) throw new Exception("References to slider are null");
        if (userScene == null) throw new Exception("References to scene are null");
        if (menuScene == null) throw new Exception("References to scene are null");
        
        var iDataTypes = FindObjectsOfType<MonoBehaviour>().OfType<IDataInstances>().ToArray();
        for (int i = 0; i < iDataTypes.Length; i++)
        {
            await iDataTypes[i].IsDone();
            LoadSlider(i + 1,iDataTypes.Length);
        }

        LoadScene();
    }

    private void LoadSlider(float currentTaskIndex, float range)
    {
        slider.value = currentTaskIndex / range;
    }

    private void LoadScene() {
        SceneReference loadedScene = null; 
        if (string.IsNullOrEmpty(NetworkPersonData.Instance.NickName)) loadedScene = userScene;
        else loadedScene = menuScene;

        SceneManager.LoadScene(loadedScene);
    }
}

public interface IDataInstances
{
    Task IsDone();
}