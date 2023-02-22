using System;
using System.ComponentModel;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

/// <summary>
/// Class <c>json2shape</c> models JSON data into representative 3D shapes.
/// </summary>
public class json2shape : MonoBehaviour
{
    JContainer obj = JObject.Parse(File.ReadAllText("./Assets/project.json"));
    List<string> master_index = new List<string>();
    int full_size;
    // Start is called before the first frame update
    void Start()
    {
        print(obj.First);
        print(obj.First.Type);
        print(obj.Type);
        print(obj.Count);
        print(obj.Root);
        print(obj.Path);
        full_size = Dig(obj,0,0,"0",Color.white);
        Reposition(master_index);
        //print("FULL DIG SIZE: "+full_size);
    }

    /// <summary>
    /// "Dig" recursively calls itself to retrieve all nested data types.
    /// </summary>
    int Dig(JToken objecto, int layer, int index, string address, Color color)
    {
        layer++;
        address +="."+layer+"-"+index;
        int size=0;

        foreach (JToken sub in objecto)
        {
        
            int array_values = 0;
            foreach (JToken sub_obj in sub)
            {
                address += "-A";
                size = Dig(sub_obj,layer,index,address,Color.green);
                full_size += size;
                master_index.Add(address);
                array_values++;
            }
            //print(layer+"\nArray Size: "+size+"\nArray count: "+array_values);
            master_index.Add(address);
        }
        index++;
        
        //print(address);
        return size;
    }

    /// <summary>
    /// "Build" creates a new block for each data type based on size and address).
    /// </summary>
    GameObject Build(string object_address, Color localColor)
    {
        GameObject local_object = GameObject.CreatePrimitive(PrimitiveType.Cube);
        string[] levels = object_address.Split("_");
        
        // Position block based on layer
        int local_layer = levels.Last()[0];
        local_object.transform.position = new Vector3(local_layer,0,0);

        // Color the block
        Renderer block_renderer = local_object.GetComponent<Renderer>();
        localColor.a = 0.5f;
        block_renderer.material.SetColor("_Color",localColor);

        // Make it transparent
        // Like in this example: https://answers.unity.com/questions/1004666/change-material-rendering-mode-in-runtime.html
        block_renderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        block_renderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        block_renderer.material.SetInt("_ZWrite", 0);
        block_renderer.material.DisableKeyword("_ALPHATEST_ON");
        block_renderer.material.DisableKeyword("_ALPHABLEND_ON");
        block_renderer.material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
        block_renderer.material.renderQueue = 3000;

        // Scale the block based on size
        //if(size==0)
        //    size=1;
        //local_object.transform.localScale = new Vector3(1,size,1);
        local_object.name = object_address;
        master_index.Add(object_address);
        //print("Adding "+local_object.name+" to index");
        return local_object;
    }

    /// <summary>
    /// "Resposition" assigns each GameObject to its proper parent.
    /// </summary>
    void Reposition(IEnumerable<string> index)
    {
        //print(index.Count);
        Build("0",Color.white);
        foreach (string address in index.Reverse())
        {
            string[] levels = address.Split("_");
            string new_address = "";
            Color local_color = Color.white;
            for(int i =0;i<levels.Count()-1;i++)
            {
                if(i==levels.Count()-2)
                    new_address+=levels[i];
                else
                    new_address+=levels[i]+"_";
            }
            string parent_address = address.Replace("_"+levels.Last(),"");
            string type = address[address.Count()-1].ToString();
            if (type=="V")
                local_color = Color.yellow;
            else if (type=="P")
                local_color = Color.red;
            else if (type=="O")
                local_color = Color.blue;
            else if (type=="A")
                local_color = Color.green;
            else
                local_color = Color.gray;
            if(GameObject.Find(address)==null)
                Build(address,local_color);
            else
                print(address+"     "+new_address+"     "+parent_address);
            Transform old_transform = GameObject.Find(address).transform;
            Transform new_transform = GameObject.Find(parent_address).transform;
            old_transform.SetParent(new_transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

