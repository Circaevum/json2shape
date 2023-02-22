using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

public class TextPositionXYZ2 : MonoBehaviour
{
    public TextAsset jsonFile; // The JSON file to parse

    public float zSpacing = 1f; // The distance between features on the Z axis
    public float ySpacing = 0.5f; // The distance between TextMesh objects on the Y axis

    private float currentZ = 0f; // The current Z position for the features
    private float currentY = 0f; // The current Y position for the TextMesh objects

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
        // Increment the Z position for the next feature
        if (parent.name == "features")
        {
            currentY = 0f;
            currentZ += zSpacing;
        }
        switch (token.Type)
        {
            case JTokenType.Object: // Create a GameObject for each property in the object
                foreach (JProperty property in token)
                {
                    GameObject obj = new GameObject(property.Name);
                    obj.transform.SetParent(parent);
                    CreateModularStructure(property.Value, obj.transform);
                    obj.transform.localPosition = new Vector3(0f, currentY, currentZ);
                    currentY -= ySpacing;
                }
                break;
            case JTokenType.Array: // Create a GameObject for each item in the array
                for (int i = 0; i < token.Count(); i++)
                {
                    GameObject obj = new GameObject(i.ToString());
                    obj.transform.SetParent(parent);
                    CreateModularStructure(token[i], obj.transform);
                    obj.transform.localPosition = new Vector3(0f, currentY, currentZ);
                    currentY -= ySpacing;
                }
                break;
            case JTokenType.String: // Create a TextMesh for the string value
                GameObject textObject = new GameObject("Text");
                textObject.transform.SetParent(parent);
                TextMesh textMesh = textObject.AddComponent<TextMesh>();
                textMesh.text = token.ToString();
                textObject.transform.localPosition = new Vector3(0f, currentY, currentZ);
                currentY -= ySpacing;
                break;
            case JTokenType.Integer: // Create a TextMesh for the integer value
                GameObject integerObject = new GameObject("Integer");
                integerObject.transform.SetParent(parent);
                TextMesh integerMesh = integerObject.AddComponent<TextMesh>();
                integerMesh.text = token.ToString();
                integerObject.transform.localPosition = new Vector3(0f, currentY, currentZ);
                currentY -= ySpacing;
                break;
            case JTokenType.Float: // Create a TextMesh for the float value
                GameObject floatObject = new GameObject("Float");
                floatObject.transform.SetParent(parent);
                TextMesh floatMesh = floatObject.AddComponent<TextMesh>();
                floatMesh.text = token.ToString();
                floatObject.transform.localPosition = new Vector3(0f, currentY, currentZ);
                currentY -= ySpacing;
                break;
            default: // Do nothing for other types
                break;
        }
    }
}
