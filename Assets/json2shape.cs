using System;
using System.ComponentModel;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

/// <summary>
/// Class <c>json2shape</c> models JSON data into representative 3D shapes.
/// </summary>
public class json2shape : MonoBehaviour
{
    JContainer obj = JObject.Parse(File.ReadAllText("./Assets/projects.json"));
    List<string> master_index = new List<string>();
    // Start is called before the first frame update
    void Start()
    {
        //print(obj.First);
        //print(obj.First.Type);
        //print(obj.Type);
        //print(obj.Count);
        //print(obj.Root);
        //print(obj.Path);
        int full_size = Dig(obj,0,"0",Color.blue);
        Reposition(master_index);
        //print("FULL DIG SIZE: "+full_size);
    }

    /// <summary>
    /// "Dig" recursively calls itself to retrieve all nested data types.
    /// </summary>
    int Dig(JToken objecto, int layer, string address, Color thisColor)
    {
        int size = 0;
        layer++;//
        string type = objecto.GetType().ToString().Substring(22,1);
        address +="_"+layer+"-"+type;
        Color sub_color = new Color();
        //print(layer+"_"+address+"_"+objecto.GetType());
        //print(layer+"_"+address+"_"+objecto);
        foreach (JToken sub in objecto)
        {
            //print(layer+":"+sub.GetType());
            //print(layer+":"+sub.HasValues);
            if (sub.GetType().ToString()=="Newtonsoft.Json.Linq.JValue")
            {
                sub_color = Color.yellow;
                size += sub.ToString().Length/100+1;
                address += "-V-"+size;
                //print(sub.Type+": "+size);
                Build(address, sub_color, size);
            }
            else
            {
                if (sub.GetType().ToString()=="Newtonsoft.Json.Linq.JProperty")
                {
                    foreach (JToken sub_prop in sub)
                    {
                        size += Dig(sub_prop,layer,address,Color.red);
                        address += "-P-"+(size+2);
                    }
                    //print(layer+" Property Size: "+size);
                }
                else if (sub.GetType().ToString()=="Newtonsoft.Json.Linq.JObject")
                {
                    foreach (JToken sub_obj in sub)
                    {
                        size += Dig(sub_obj,layer,address,Color.blue);
                        address += "-O-"+(size+2);
                    }
                        
                    //print(layer+"Object Size: "+size);
                }
                else if (sub.GetType().ToString()=="Newtonsoft.Json.Linq.JArray")
                {
                    int array_values = 0;
                    foreach (JToken sub_obj in sub)
                    {
                        size += Dig(sub_obj,layer,address,Color.green);
                        address += "-A-"+(size+2);
                        array_values++;
                    }
                    //print(layer+"\nArray Size: "+size+"\nArray count: "+array_values);
                }
            }
            
        }  
        GameObject thisObject = Build(address, thisColor, size);
        //print(address);
        return size;
    }

    /// <summary>
    /// "Build" creates a new block for each data type based on size and address).
    /// </summary>
    GameObject Build(string object_address, Color localColor, int size)
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
        if(size==0)
            size=1;
        local_object.transform.localScale = new Vector3(1,size,1);
        local_object.name = object_address;
        master_index.Add(object_address);
        //print("Adding "+local_object.name+" to index");
        return local_object;
    }

    /// <summary>
    /// "Resposition" assigns each GameObject to its proper parent.
    /// </summary>
    void Reposition(List<string> index)
    {
        print("YO!");
        print(index.Count);
        foreach (string address in index)
        {
            string[] levels = address.Split("_");
            string parent_address = address.Replace("_"+levels.Last(),"");
            print(address+" : "+parent_address);
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

