using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.ComponentModel.Design.Serialization;

public class TextPositionsCustom : MonoBehaviour
{
    public TextAsset jsonFile; // The JSON file to parse

    public float levelOffset = 1.0f; // The distance between levels
    public float ySpacing = 0.5f; // The distance between TextMesh objects in the same level
    void Start()
    {
        // Load the JSON file
        string json = jsonFile.text;

        // Parse the JSON
        JToken token = JToken.Parse(json);

        // Recursively create TextMesh objects for each token
        GameObject root = new GameObject("Root"); // Create a root object for the modular structure
        Vector3 rootXYZ = new Vector3(0, 0, 0);
        CreateTextMeshes(token, root.transform, rootXYZ);
    }

    // Recursively create TextMesh objects for each token
    void CreateTextMeshes(JToken token, Transform parent, Vector3 xyz)
    {
        // Increment the Y position for the next TextMesh
        float xLevel = xyz.x++;
        float yHeight = xyz.y - ySpacing;
        float zFeature = xyz.z;
        if (parent.name == "Root")
            zFeature++;
        xyz = new Vector3(xLevel, yHeight, zFeature);
        switch (token.Type)
        {
            case JTokenType.Object: // Create a GameObject for each property in the object
                foreach (JProperty property in token)
                {
                    GameObject obj = new GameObject(property.Name);
                    obj.transform.SetParent(parent);
                    obj.transform.position = xyz;
                    CreateTextMeshes(property.Value, obj.transform, xyz);
                }
                break;
            case JTokenType.Array: // Create a GameObject for each item in the array
                for (int i = 0; i < token.Count(); i++)
                {
                    GameObject obj = new GameObject(i.ToString());
                    obj.transform.SetParent(parent);
                    CreateTextMeshes(token[i], obj.transform, xyz);
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

/*
 * 
        // Create a TextMesh for the token
        GameObject textObject = new GameObject(token.ToString());
        textObject.transform.SetParent(parent);

        TextMesh textMesh = textObject.AddComponent<TextMesh>();
        textMesh.text = token.ToString();

        // Position the TextMesh based on the nesting level
        textObject.transform.position = new Vector3(xLevel, currentY, zIteration * levelOffset);

        // Increment the Y position for the next TextMesh
        currentY -= Mathf.RoundToInt(ySpacing);

        if (parent.name == "Root")
            zIteration++;

        // Recursively create TextMeshes for the token's children
        if (token.Type == JTokenType.Object)
        {
            foreach (JProperty property in token)
            {
                CreateTextMeshes(property.Value, textObject.transform, xLevel + 1,zIteration);
            }
        }
        else if (token.Type == JTokenType.Array)
        {
            for (int i = 0; i < token.Count(); i++)
            {
                CreateTextMeshes(token[i], textObject.transform, xLevel + 1, zIteration);
            }
        }
*/