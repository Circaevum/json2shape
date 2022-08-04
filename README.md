# json2shape
**Creates 3D structures based on JSON object formatting and data types**

JSON files are built in a way to be serialized and deserialized data objects with a lot of flexibility over the structure of variable layers, as opposed to flat, tabular formats. [This post](https://stackoverflow.com/questions/38558844/jcontainer-jobject-jtoken-and-linq-confusion/38560188#38560188) captures useful conversation on the confusion and clarity around JSON Object types, but generally they can be described at the types listed below:
```
JToken             - abstract base class     
   JContainer      - abstract base class of JTokens that can contain other JTokens
       JArray      - represents a JSON array (contains an ordered list of JTokens)
       JObject     - represents a JSON object (contains a collection of JProperties)
       JProperty   - represents a JSON property (a name/JToken pair inside a JObject)
   JValue          - represents a primitive JSON value (string, number, boolean, null)
         String    - general collection of characters. Could be word, paragraph, code
         Number    - could be integer or float I believe
         Boolean   - true or false
         Null      - empty
```

The goal of this project is to build the functionality to ingest any properly formatted JSON file, and visualize the data contained in the form of a 3D shape whose structure is entirely reflective of data contained within.




