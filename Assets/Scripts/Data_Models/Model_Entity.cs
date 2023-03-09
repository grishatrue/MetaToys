using System.Collections.Generic;

[System.Serializable]
public class Model_Entity
{
    public string currentClassName = "Base_Data";
    public string entityName = "";
    public List<Model_Property_Wrapper> properties = new List<Model_Property_Wrapper>();
}
