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
public class json2ship : MonoBehaviour
{
    JContainer obj = JObject.Parse(File.ReadAllText("./Assets/project.json"));
    //JFT container = Describe(obj,"0","0",0,obj.GetType().ToString().Substring(22,1),obj.ToString(),Color.blue,0,0,0);

    List<JFT> master_index = new List<JFT>();
    // Start is called before the first frame update
    void Start()
    {
        print(obj.First);
        print(obj.First.Type);
        print(obj.Type);
        print(obj.Count);
        print(obj.Root);
        print(obj.Path);
        //int full_size = Dig(container);
        //Reposition(master_index);
        //print("FULL DIG SIZE: "+full_size);
    }
}

//     JFT Describe(JFT input)
//     {
//         JFT output = input;
//         output.Type = input.Token.GetType().ToString().Substring(22,1);
//         if(output.Type=="V")
//             output.Color = Color.yellow;
//         else if(output.Type=="P")
//             output.Color = Color.red;
//         else if(output.Type=="O")
//             output.Color = Color.blue;
//         else if(output.Type=="A")
//             output.Color = Color.green;
//         return output;
//     }

//     /// <summary>
//     /// "Dig" recursively calls itself to retrieve all nested data types.
//     /// </summary>
//     int Dig(JFT token)
//     {
//         token.Size = 0;
//         token = Describe(token);
//         token.Level++;
//         token.Address +="."+token.Level+"-"+token.LocalAddress;

//         foreach (JToken sub in token.Token)
//         {
//             //sub = Describe(sub);
//             //If it's a simple value with no child objects, add it to the list with its address
//             if (sub.Type.ToString()=="V")
//             {
//                 sub.Value = sub.ToString();
//                 sub.Color = Color.yellow;
//                 sub.Size += sub.ToString().Length;
//                 sub.Type = sub.GetType().ToString().Substring(22,1);
//                 master_index.Add(sub);
//             }
//             else if (sub.Type=="A")
//                 {
//                     int array_values = 0;
//                     foreach (JFT sub_obj in sub.Token)
//                     {
//                         size += Dig(sub_obj);
//                         master_index.Add(sub_obj);
//                         array_values++;
//                     }
//                     //print(layer+"\nArray Size: "+size+"\nArray count: "+array_values);
//                 }
//             else
//             {
//                 foreach (JFT sub_sub in sub.Token)
//                 {
//                     sub_sub = Describe(sub_sub);
//                     sub.Size += Dig(sub_sub);
//                     master_index.Add(sub_sub);
//                 }
//             }
                
//             master_index.Add(address);
//         }
//         //print(address);
//         return token.Size;
//     }

//     /// <summary>
//     /// "Build" creates a new block for each data type based on size and address).
//     /// </summary>
//     GameObject Build(string object_address, Color localColor)
//     {
//         GameObject local_object = GameObject.CreatePrimitive(PrimitiveType.Cube);
//         string[] levels = object_address.Split("_");
        
//         // Position block based on layer
//         int local_layer = levels.Last()[0];
//         local_object.transform.position = new Vector3(local_layer,-levels[1][0]*10,0);

//         // Color the block
//         Renderer block_renderer = local_object.GetComponent<Renderer>();
//         localColor.a = 0.5f;
//         block_renderer.material.SetColor("_Color",localColor);

//         // Make it transparent
//         // Like in this example: https://answers.unity.com/questions/1004666/change-material-rendering-mode-in-runtime.html
//         block_renderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
//         block_renderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
//         block_renderer.material.SetInt("_ZWrite", 0);
//         block_renderer.material.DisableKeyword("_ALPHATEST_ON");
//         block_renderer.material.DisableKeyword("_ALPHABLEND_ON");
//         block_renderer.material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
//         block_renderer.material.renderQueue = 3000;

//         // Scale the block based on size
//         //if(size==0)
//         //    size=1;
//         //local_object.transform.localScale = new Vector3(1,size,1);
//         local_object.name = object_address;
//         master_index.Add(object_address);
//         //print("Adding "+local_object.name+" to index");
//         return local_object;
//     }

//     /// <summary>
//     /// "Resposition" assigns each GameObject to its proper parent.
//     /// </summary>
//     void Reposition(IEnumerable<JFT> index)
//     {
//         //print(index.Count);
//         Build("0",Color.white);
//         foreach (string address in index.Reverse())
//         {
//             string[] levels = address.Split("_");
//             string new_address = "";
//             Color local_color = Color.white;
//             for(int i =0;i<levels.Count()-1;i++)
//             {
//                 if(i==levels.Count()-2)
//                     new_address+=levels[i];
//                 else
//                     new_address+=levels[i]+"_";
//             }
//             string parent_address = address.Replace("_"+levels.Last(),"");
//             string type = address[address.Count()-1].ToString();
//             if (type=="V")
//                 local_color = Color.yellow;
//             else if (type=="P")
//                 local_color = Color.red;
//             else if (type=="O")
//                 local_color = Color.blue;
//             else if (type=="A")
//                 local_color = Color.green;
//             else
//                 local_color = Color.gray;
//             if(GameObject.Find(address)==null)
//                 Build(address,local_color);
//             else
//                 print(address+"     "+new_address+"     "+parent_address);
//             Transform old_transform = GameObject.Find(address).transform;
//             Transform new_transform = GameObject.Find(parent_address).transform;
//             old_transform.SetParent(new_transform);
//         }
//     }

//     // Update is called once per frame
//     void Update()
//     {
        
//     }
// }

[System.Serializable]
public class JFT
{
    public JToken Token { get; set; }
    public string ParentAddress { get; set; }
    public string Address { get; set; }
    public int LocalAddress { get; set; }
    public string Type { get; set; }
    public string Value { get; set; }
    public Color Color { get; set; }
    public int Size { get; set; }
    public int Height { get; set; }
    public int Level { get; set; }
    public JFT(JToken token, string parentAddress, string address, int localAddress, string type, string value, Color color, int size, int height, int level)
    {
        Token = token;
        ParentAddress = parentAddress;
        Address = address;
        LocalAddress = localAddress;
        Type = type;
        Value = value;
        Color = color;
        Size = size;
        Height = height;
        Level = level;
    }
}

