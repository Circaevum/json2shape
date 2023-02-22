using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

public class TextPositionXYZ : MonoBehaviour
{
    public TextAsset jsonFile; // The JSON file to parse

    public float levelOffset = 1.0f; // The distance between levels
    public float ySpacing = 0.5f; // The distance between TextMesh objects in the same level

    private int currentY = 0; // The current Y position for the TextMesh objects
    private float[] currentZ; // The current Z position for the TextMesh objects at each level

    void Start()
    {
        // Load the JSON file
        string json = jsonFile.text;

        // Parse the JSON
        JToken token = JToken.Parse(json);

        // Initialize the current Z positions
        currentZ = new float[4 + 1];
        for (int i = 0; i < currentZ.Length; i++)
        {
            currentZ[i] = i * levelOffset;
        }

        // Recursively create TextMesh objects for each token
        CreateTextMeshes(token, 0, 0);
    }

    // Recursively create TextMesh objects for each token
    void CreateTextMeshes(JToken token, int level, float xPosition)
    {
        // Create a TextMesh for the token
        GameObject textObject = new GameObject("Text");
        textObject.transform.SetParent(transform);

        TextMesh textMesh = textObject.AddComponent<TextMesh>();
        textMesh.text = token.ToString();

        // Position the TextMesh based on the nesting level
        textObject.transform.position = new Vector3(xPosition, currentY, currentZ[level]);

        // Increment the Y position for the next TextMesh
        currentY -= Mathf.RoundToInt(ySpacing);

        // Increment the Z position for the next TextMesh at this level
        currentZ[level] += levelOffset;

        // Recursively create TextMeshes for the token's children
        if (token.Type == JTokenType.Object)
        {
            foreach (JProperty property in token)
            {
                CreateTextMeshes(property.Value, level + 1, xPosition);
            }
        }
        else if (token.Type == JTokenType.Array)
        {
            for (int i = 0; i < token.Count(); i++)
            {
                CreateTextMeshes(token[i], level + 1, xPosition + i);
            }
        }
    }
}
