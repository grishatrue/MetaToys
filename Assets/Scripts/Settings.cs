using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Threading.Tasks;

public class Settings : MonoBehaviour
{
    [HideInInspector] public Base_Data dataObject;
    private bool isDataObjectAttached = false;

    public Slider musicVolume;

    private void Awake()
    {
        isDataObjectAttached = TryGetComponent(out dataObject);

        if (isDataObjectAttached)
        {
            InitProps();
        }
    }

    private void InitProps()
    {
        string path = Path.Combine(Application.persistentDataPath, dataObject.entityName + ".json");

        if (File.Exists(path))
        {
            Data_Manager.TryLoadJSON(path, dataObject);

            if (dataObject.TryGetProperty("Music volume", out string value))
            {
                musicVolume.value = float.Parse(value);
            }
        }
        else
        {
            Data_Manager.SaveJSON(dataObject);
        }
    }

    /// <summary>
    /// Вызывается по нажатию на кнопку "Сохранить" в настройках
    /// </summary>
    public async void SaveSettingsToData()
    {
        isDataObjectAttached = TryGetComponent(out dataObject);

        if (isDataObjectAttached)
        {
            string path = Path.Combine(Application.persistentDataPath, dataObject.entityName + ".json");

            dataObject.TrySetProperty("Music volume", musicVolume.value.ToString());
            Data_Manager.SaveJSON(dataObject);

            await Task.Delay(50);

            if (File.Exists(path))
            {
                Data_Manager.TryLoadJSON(path, dataObject);
            }
        }
    }

    private void OnApplicationQuit()
    {
        string path = Path.Combine(Application.persistentDataPath, dataObject.entityName + ".json");

        if (File.Exists(path))
        {
            Data_Manager.TryLoadJSON(path, dataObject);
        }
    }
}
