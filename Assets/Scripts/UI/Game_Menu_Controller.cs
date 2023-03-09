using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Menu_Controller : MonoBehaviour
{
    public List<Screen_Wrapper> screens;

    private void Awake()
    {
        Finish.OnLevelFinished += OnFinish;
        Dead_Event.OnPlayerDied += OnPlayerDead;
    }

    private void OnFinish()
    {
        SetActiveAtNames(new List<string>() { "pause panel", "next level button" }, true);
    }

    private void OnPlayerDead()
    {
        SetActiveAtNames(new List<string>() { "pause panel" }, true);
    }

    private void SetActiveAtNames(List<string> names, bool active)
    {
        foreach (var i in screens)
        {
            foreach (var j in names)
            {
                if (i.name == j)
                {
                    i.screen.SetActive(active);
                    break;
                }
            }
        }
    }

    private void OnDestroy()
    {
        Finish.OnLevelFinished -= OnFinish;
        Dead_Event.OnPlayerDied -= OnPlayerDead;
    }
}
