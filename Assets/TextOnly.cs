using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

public class TextOnly : MonoBehaviour
{
    public TextAsset jsonFile; // The JSON file to parse

    void Start()
    {
        // Load the JSON file
        string json = jsonFile.text;

        // Parse the JSON
        JToken token = JToken.Parse(json);

        // Create a modular structure based on the shape of the data
        GameObject root = new GameObject("Root"); // Create a root object for the modular structure
        CreateModularStructure(token, root.transform); // Create the modular structure recursively
    }

    // Create a modular structure recursively
    void CreateModularStructure(JToken token, Transform parent)
    {
        switch (token.Type)
        {
            case JTokenType.Object: // Create a GameObject for each property in the object
                foreach (JProperty property in token)
                {
                    GameObject obj = new GameObject(property.Name);
                    obj.transform.SetParent(parent);
                    CreateModularStructure(property.Value, obj.transform);
                }
                break;
            case JTokenType.Array: // Create a GameObject for each item in the array
                for (int i = 0; i < token.Count(); i++)
                {
                    GameObject obj = new GameObject(i.ToString());
                    obj.transform.SetParent(parent);
                    CreateModularStructure(token[i], obj.transform);
                }
                break;
            case JTokenType.String: // Create a TextMesh for the string value
                GameObject textObject = new GameObject("Text");
                textObject.transform.SetParent(parent);
                TextMesh textMesh = textObject.AddComponent<TextMesh>();
                textMesh.text = token.ToString();
                break;
            case JTokenType.Integer: // Create a TextMesh for the integer value
                GameObject integerObject = new GameObject("Integer");
                integerObject.transform.SetParent(parent);
                TextMesh integerMesh = integerObject.AddComponent<TextMesh>();
                integerMesh.text = token.ToString();
                break;
            case JTokenType.Float: // Create a TextMesh for the float value
                GameObject floatObject = new GameObject("Float");
                floatObject.transform.SetParent(parent);
                TextMesh floatMesh = floatObject.AddComponent<TextMesh>();
                floatMesh.text = token.ToString();
                break;
            default: // Do nothing for other types
                break;
        }
    }
}
