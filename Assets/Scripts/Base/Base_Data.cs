using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// <para>
/// Свойства хранятся в папке DefaultCompany.
/// </para>
/// (Полный путь сохранения в классе DataManager)
/// </summary>

public class Base_Data : MonoBehaviour
{
    public string currentClassName = "Base_Data";

    public List<Model_Property_Wrapper> properties = new List<Model_Property_Wrapper>();
    [Space(2)]
    public string entityName = "";
    public static string pathSaveFile;

    public bool TryGetProperty(string name, out string res)
    {
        string result = "";
        foreach (Model_Property_Wrapper pw in properties)
        {
            if (pw.name == name)
            {
                result = pw.value;
                res = result;
                return true;
            }
        }
        res = result;
        return false;
    }

    public bool TrySetProperty(string name, in string value)
    {
        foreach (Model_Property_Wrapper pw in properties)
        {
            if (pw.name == name)
            {
                pw.value = value;
                return true;
            }
        }
        return false;
    }
/*
    public object ConvertToObject(Property_Wrapper pw, out string type)
    {
        type = pw.originType;
        //switch (pw.originType)
        {
            //case "int":
            return pw.value;
        }
    }

    public object GetTransformedValue(object pw, string type)
    {
        switch (type)
        {
            case "int":
                return int.Parse((string)pw);
            case "float":
                return float.Parse(pw.ToString());
            default: 
                break;
        }
        return 0; //
    }*/
}
