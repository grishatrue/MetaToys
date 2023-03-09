using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Input_History : MonoBehaviour
{
    private List<KeyCode> inputHistory = new List<KeyCode>();
    public Text inpHistoryText;
    private bool isOutTextAttached;
    [SerializeField] private int LinesNumber = 10;

    private void Start()
    {
        if (inpHistoryText == null)
            isOutTextAttached = false;
        else
            isOutTextAttached = true;
    }

    private void Update()
    {
        if (isOutTextAttached)
        {
            if (Input.anyKeyDown)
            {
                inputHistory.AddRange(GetInpStr());
                inpHistoryText.text = ConvertLineToText(DeleteExtras(inputHistory));
            }
        }
    }

    /// <summary>
    /// Возвращает коды кнопок нажатых за текущий кадр
    /// </summary>
    /// <returns></returns>
    private List<KeyCode> GetInpStr()
    {
        List<KeyCode> res = new List<KeyCode>();
        string frameString = Input.inputString;

        // If you remove the loop and press more than 1 buttons an exeption
        // "Unable to find key name that matches <your string>" will be thrown
        foreach (char i in frameString)
        {
            res.Add(Event.KeyboardEvent(Char.ToString(i)).keyCode);
        }

        return res;
    }

    private string ConvertLineToText(List<KeyCode> keyCodes)
    {
        string newStr = "";

        foreach (var i in keyCodes)
        {
            newStr += i + "\n";
        }

        return newStr;
    }

    public List<KeyCode> DeleteExtras(List<KeyCode> list)
    {
        int listCount = list.Count;

        if (listCount >= LinesNumber)
        {
            list.RemoveRange(0, listCount - LinesNumber);
        }

        return list;
    }
}