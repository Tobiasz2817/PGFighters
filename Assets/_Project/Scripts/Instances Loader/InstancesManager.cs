using System;
using System.Linq;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InstancesManager : MonoBehaviour
{
    [SerializeField] private SceneReference loadScene;
    [SerializeField] private Slider slider;
    
    private async void Awake()
    {
        if (slider == null) throw new Exception("References to slider are null");
        if (loadScene == null) throw new Exception("References to scene are null");
        
        var iDataTypes = FindObjectsOfType<MonoBehaviour>().OfType<IDataInstances>().ToArray();
        for (int i = 0; i < iDataTypes.Length; i++)
        {
            await iDataTypes[i].IsDone();
            LoadSlider(i + 1,iDataTypes.Length);
        }

        SceneManager.LoadScene(loadScene);
    }

    private void LoadSlider(float currentTaskIndex, float range)
    {
        slider.value = currentTaskIndex / range;
    }
}

public interface IDataInstances
{
    Task IsDone();
}