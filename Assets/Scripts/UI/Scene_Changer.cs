using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Scene_Changer : MonoBehaviour
{
    private string nameForConfirm;

    public void GoToSceneAtName(string name)
    {
        SceneManager.LoadSceneAsync(name);
    }

    public void GoToSceneAtNameWithConfirm(string name)
    {
        nameForConfirm = name;
        Confirm.OnActionConfirmed += GoToSceneAtNameWithConfirm2;
    }

    private void GoToSceneAtNameWithConfirm2()
    {
        Confirm.OnActionConfirmed -= GoToSceneAtNameWithConfirm2;
        SceneManager.LoadSceneAsync(nameForConfirm);
    }

    public void SceneReplay()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }

    public void SceneReplayWithConfirm()
    {
        Confirm.OnActionConfirmed += SceneReplayWithConfirm2;
    }

    private void SceneReplayWithConfirm2()
    {
        Confirm.OnActionConfirmed -= SceneReplayWithConfirm2;
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }
}
