using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public Text txt;

    private void Start()
    {
        Debug.Log(Path.GetDirectoryName("Assets/Root/l.txt"));
        Debug.Log(Path.GetDirectoryName("Assets/Root/l"));
    }

    private void Update()
    {

    }

    [MenuItem("Tools/move")]
    private static void Test12()
    {
        string srcDirPath = "Assets/Data";
        string destDirPath = "Assets/NewData/Game";
        IOUtils.CopyFolder(srcDirPath, destDirPath, false);
        AssetDatabase.Refresh();
    }
}
