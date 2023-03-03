using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CSVLoad : MonoBehaviour
{
    public static List<string> LoadData(string fileName)
    {
        //저장할 Path 설정
        TextAsset filePath = Resources.Load<TextAsset>(fileName);

        StringReader tr = new StringReader(filePath.text);
        if (tr == null) return null;

        List<string> dataList = new List<string>();
        string line = tr.ReadLine(); //첫줄은 Properties이므로 따로 처리안함


        while (line != null)
        {
            line = tr.ReadLine();
            dataList.Add(line);
            if (line == null) break;
        }

        return dataList;
    }
}
