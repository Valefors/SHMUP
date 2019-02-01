using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Linq;
using UnityEditor;
using System;
using System.IO;
using System.Text;

[Serializable]
public class WwiseEventEnumData : ScriptableObject
{
    [SerializeField]
    public List<string> lastEventName;
}

public static class EventEnumCreator
{
    public static string PATH_OF_WWISE_SCRIPT = @".\Assets\Wwise\Deployment\API\Generated\Windows\AkSoundEngine_Windows.cs";
    public static string PATH_OF_DATA_FOR_ENUM = "Assets/Wwise/Editor/WwiseData.asset";
    public static string PATH_OF_ID = @".\Assets\StreamingAssets\Audio\GeneratedSoundBanks\Windows\SoundbanksInfo.xml";
    public static string XML_PATH = "//SoundBanksInfo/SoundBanks/SoundBank/IncludedEvents/Event";

    public static string PATH_FOR_INIT = @".\Assets\Wwise\Editor\WwiseWindows\AkWwisePicker.cs";

    [MenuItem("Component/Wwise/Init Event Enum")]
    public static void Init()
    {
        string lineToAdd = "				EventEnumCreator.RefreshEnumNameEvent(); //AUTOMATIC ADDITION BY THE SCRIPT NAMED : EventEnumCreator.cs (function Init() )";

        //Get the files 
        StreamReader tR = new StreamReader(PATH_FOR_INIT);
        List<string> completeVersion = new List<string>();

        //Find the right place to place the above code
        for (int i = 0; i < 40; i++)
            completeVersion.Add(tR.ReadLine());

        //Add the above code 
        completeVersion.Add(lineToAdd);

        //Ignore all the lines whose not from the original files (aka : ignore the obsolete line)
        string lineToFind = "AkUtilities";
        string nextLine = tR.ReadLine();
        while (!nextLine.Contains(lineToFind))
        {
            nextLine = tR.ReadLine();
        }
        //But still add the line who have the "Ak Soundbank version" which is part of the original file
        completeVersion.Add(nextLine);

        //Add add the rest of the files
        completeVersion.Add(tR.ReadToEnd());
        tR.Close();

        //Convert it and write it
        File.WriteAllLines(PATH_FOR_INIT, completeVersion.ToArray());
    }

    public static void RefreshEnumNameEvent()
    {
        RefreshEnumNameEvent(false);
    }

    public static void RefreshEnumNameEvent(bool force)
    {
        //Go get the info
        string data = File.ReadAllText(PATH_OF_ID);
        //Read it
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(new StringReader(data));
        //Store the info
        XmlNodeList fecthedNodeList = xmlDoc.SelectNodes(XML_PATH);
        //Get data into string : 
        List<string> event_name_list = new List<string>();
        foreach (XmlNode node in fecthedNodeList)
        {
            event_name_list.Add(node.Attributes[1].Value);
        }

        //Local save with the string name of events
        WwiseEventEnumData past = AssetDatabase.LoadAssetAtPath<WwiseEventEnumData>(PATH_OF_DATA_FOR_ENUM);
        if (past == null) //if doesn't exists : create it
        {
            WwiseEventEnumData newData = ScriptableObject.CreateInstance<WwiseEventEnumData>();
            newData.lastEventName = event_name_list;
            AssetDatabase.CreateAsset(newData, PATH_OF_DATA_FOR_ENUM);
            AssetDatabase.SaveAssets();
            Debug.Log("Creation of : " + newData.name);
            past = newData;
            //Launch the event enum part
            CreateEventEnum(event_name_list);
            //Stock the new events names
            past.lastEventName = event_name_list;
            AssetDatabase.SaveAssets();
        }
        else if (!compareStringList(past.lastEventName, event_name_list) || force) 
        {
            Debug.Log("Not the same !");
            CreateEventEnum(event_name_list);
            //Stock the new events names
            past.lastEventName = event_name_list;
            AssetDatabase.SaveAssets();
        }
    }

    public static void CreateEventEnum(List<string> eventNameList)
    {
        //Prepare the string to integrate : write the enum
        List<string> addLines = new List<string>();
        addLines.Add("  //AUTOMATIC ADDITION BY THE SCRIPT NAMED : EventEnumCreator.cs (function CreateEventEnum() )");
        addLines.Add("  public enum AKEventName {");
        foreach (string name in eventNameList)
            addLines.Add("        " + name + ",");
        addLines.Add("    }");
        addLines.Add("    ");
        //                                   write the enum to string
        addLines.Add("  public static string eventEnumToString(AKEventName in_EventName) {");
        addLines.Add("        return in_EventName.ToString();");
        addLines.Add("  }");
        addLines.Add("  ");
        //                                   write the override version who take an Event and not a string
        addLines.Add("  public static uint PostEvent(AKEventName in_EnumEventName, UnityEngine.GameObject in_gameObjectID) {");
        addLines.Add("        string local_EventNameStr = eventEnumToString(in_EnumEventName);");
        addLines.Add("        return AkSoundEngine.PostEvent(local_EventNameStr, in_gameObjectID);");
        addLines.Add("  }");
        addLines.Add("  //END OF ADDITION ");

        //Get the files 
        StreamReader tR = new StreamReader(PATH_OF_WWISE_SCRIPT);
        List<string> completeVersion = new List<string>();

        //Find the right place to place the above code
        for (int i = 0; i < 13; i ++)
            completeVersion.Add(tR.ReadLine());

        //Add the above code 
        foreach (string line in addLines)
            completeVersion.Add(line);

        //Ignore all the lines whose not from the original files (aka : ignore our obsolete lines)
        string lineToFind = "AK_SOUNDBANK_VERSION";
        string nextLine = tR.ReadLine();
        while (!nextLine.Contains(lineToFind))
        {
            nextLine = tR.ReadLine();
        }
        //But still add the line who have the "Ak Soundbank version" which is part of the original file
        completeVersion.Add(nextLine);

        //Add add the rest of the files
        completeVersion.Add(tR.ReadToEnd());
        tR.Close();

        //Convert it and write it
        File.WriteAllLines(PATH_OF_WWISE_SCRIPT, completeVersion.ToArray());

    }

    //Just in case
    public static void ForceRefreshEnumNameEvent()
    {
        RefreshEnumNameEvent(true);
    }

    //Just in case 2 : the son
    static void DeleteClean()
    {
        AssetDatabase.DeleteAsset(PATH_OF_DATA_FOR_ENUM);
    }

    static bool compareStringList(List<string> listStringA, List<string> listStringB)
    {
        bool res = true;
        if (listStringA.Count != listStringB.Count)
        {
            res = false;
        }
        else
        {
            for (int i = 0; i < listStringA.Count; i++)
            {
                if (listStringA[i] != listStringB[i])
                    res = false;
            }
        }
        return res;
    }
}
