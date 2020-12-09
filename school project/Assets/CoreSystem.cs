using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CoreSystem : MonoBehaviour
{
    public static string LogFilePath = "/SchoolProjectStuff/log_game.txt";
    private void Awake()
    {
        
        LogFilePath = Application.dataPath + "/log_game.txt";
        Debug.Log(LogFilePath);
        File.Delete(LogFilePath);
    }

    public static void file_write_text(string path, string txt)
    {
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(txt);
        writer.Close();
    }

    public static void system_log_write(string txt)
    {
        string pre_text = "[" + System.DateTime.Now.ToString("hh:mm:ss") + "]: ";
        file_write_text(LogFilePath, pre_text+txt);
    }

    public static GameObject instance_create(float x, float y, GameObject instance)
    {
        return Instantiate(instance, new Vector3(x, 0, y), new Quaternion(0, 0, 0, 0));
    }
    public static GameObject instance_create(float x, float y, float z, GameObject instance)
    {
        return Instantiate(instance, new Vector3(x, y, z), new Quaternion(0, 0, 0, 0));
    }
    public static float smooth_approach(float current, float target, float smooth)
    {
        float diff = target - current;
        if(Mathf.Abs(diff) < 0.0005f)
        {
            return target;
        }
        else
        {
            return current + Mathf.Sign(diff) * Mathf.Abs(diff) * smooth;
        }
    }
}
