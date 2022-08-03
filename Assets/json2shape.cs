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
        Dig(obj,0);
        
        foreach(PropertyDescriptor descriptor in TypeDescriptor.GetProperties(obj))
        {
            string name = descriptor.Name;
            object value = descriptor.GetValue(obj);
            print(name+"="+ value);
        }
    }

    //Recursive method to retrieve all nested data types
    void Dig(JToken objecto, int layer)
    {
        layer++;
        print(layer+":"+objecto.GetType());
        print(layer+":"+objecto);
        foreach (JToken sub in objecto)
        {
            print(layer+":"+sub.GetType());
            print(layer+":"+sub.HasValues);
            print(layer+":"+sub);
            if (sub.GetType().ToString()=="Newtonsoft.Json.Linq.JProperty")
            {
                foreach (JToken sub_prop in sub)
                    Dig(sub_prop,layer);
            }
            else if (sub.GetType().ToString()=="Newtonsoft.Json.Linq.JObject")
            {
                foreach (JToken sub_obj in sub)
                    Dig(sub_obj,layer);
            }
            else if (sub.GetType().ToString()=="Newtonsoft.Json.Linq.JValue")
            {
                foreach (JToken sub_val in sub)
                    Dig(sub_val,layer);
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

