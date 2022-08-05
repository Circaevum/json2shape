using System;
using System.ComponentModel;
using System.Dynamic;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
        int full_size = Dig(obj,0,"0");
        print("FULL DIG SIZE: "+full_size);
    }

    //Recursive method to retrieve all nested data types
    int Dig(JToken objecto, int layer, string address)
    {
        int size = 0;
        layer++;//
        address +="_"+layer+"-"+objecto.GetType().ToString().Substring(22,1);
        //print(layer+"_"+address+"_"+objecto.GetType());
        //print(layer+"_"+address+"_"+objecto);
        foreach (JToken sub in objecto)
        {
            print(layer+":"+sub.GetType());
            print(layer+":"+sub.HasValues);
            if (sub.GetType().ToString()=="Newtonsoft.Json.Linq.JProperty")
            {
                foreach (JToken sub_prop in sub)
                    size += Dig(sub_prop,layer,address+"-P-"+(size+2));
                print(layer+" Property Size: "+size);
            }
            else if (sub.GetType().ToString()=="Newtonsoft.Json.Linq.JObject")
            {
                foreach (JToken sub_obj in sub)
                    size += Dig(sub_obj,layer,address+"-O-"+(size+2));
                print(layer+"Object Size: "+size);
            }
            else if (sub.GetType().ToString()=="Newtonsoft.Json.Linq.JArray")
            {
                int array_values = 0;
                foreach (JToken sub_obj in sub)
                {
                    size += Dig(sub_obj,layer,address+"-A-"+(size+2));
                    array_values++;
                }
                print(layer+"\nArray Size: "+size+"\nArray count: "+array_values);
            }
            else if (sub.GetType().ToString()=="Newtonsoft.Json.Linq.JValue")
            {
                size += sub.ToString().Length/100+1;
                print(sub.Type+": "+size);
            }
        }  
        print(address);
        return size;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

