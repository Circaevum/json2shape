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

    // Start is called before the first frame update
    void Start()
    {
        print(obj.First);
        print(obj.First.Type);
        print(obj.Type);
        print(obj.Count);
        print(obj.Root);
        print(obj.Path);
        int full_size = Dig(obj,0,"0",Color.blue);
        print("FULL DIG SIZE: "+full_size);
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
        //print(layer+"_"+address+"_"+objecto.GetType());
        //print(layer+"_"+address+"_"+objecto);
        foreach (JToken sub in objecto)
        {
            print(layer+":"+sub.GetType());
            print(layer+":"+sub.HasValues);
            if (sub.GetType().ToString()=="Newtonsoft.Json.Linq.JProperty")
            {
                foreach (JToken sub_prop in sub)
                    size += Dig(sub_prop,layer,address+"-P-"+(size+2),Color.red);
                print(layer+" Property Size: "+size);
            }
            else if (sub.GetType().ToString()=="Newtonsoft.Json.Linq.JObject")
            {
                foreach (JToken sub_obj in sub)
                    size += Dig(sub_obj,layer,address+"-O-"+(size+2),Color.blue);
                print(layer+"Object Size: "+size);
            }
            else if (sub.GetType().ToString()=="Newtonsoft.Json.Linq.JArray")
            {
                int array_values = 0;
                foreach (JToken sub_obj in sub)
                {
                    size += Dig(sub_obj,layer,address+"-A-"+(size+2),Color.green);
                    array_values++;
                }
                print(layer+"\nArray Size: "+size+"\nArray count: "+array_values);
            }
            else if (sub.GetType().ToString()=="Newtonsoft.Json.Linq.JValue")
            {
                thisColor = Color.yellow;
                size += sub.ToString().Length/100+1;
                print(sub.Type+": "+size);
                Build(address+"-V-"+size, thisColor, size);
            }
            else
                thisColor = Color.magenta;
        }  
        GameObject thisObject = Build(address, thisColor, size);
        print(address);
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
        block_renderer.material.SetColor("_Color",localColor);

        // Scale the block based on size
        if(size>400)
            size/=200;
        else if (size>100)
            size/=10;
        else
            size=1;
        local_object.transform.localScale = new Vector3(1,size,1);
        local_object.name = object_address;
        return local_object;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

