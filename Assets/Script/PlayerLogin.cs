using UnityEngine;
using System.IO;

public class PlayerLogin : MonoBehaviour
{

    /// <summary>
    /// persistentDataPath의 PlayerData로부터 Player의 Name을 가져옵니다. 이미 생성되어 있는 파일이 없었다면, null을 반환합니다.
    /// </summary>
    /// <returns></returns>
    public static string getPlayerData()
    {
        string path = Application.persistentDataPath + "\\" + "PlayerData.csv";

        TextReader tr;

        try
        {
            tr = new StreamReader(path);

            //첫번째는 colomn 넣을 예정이라 2번째거를 가져옴
            string[] tok = tr.ReadLine().Split(",");

            tr.Close();

            return tok[1];
        }
        catch
        {
            Debug.Log("noData");
            return null;
        }


    }
    /// <summary>
    /// PlayerData.csv 파일에 플레이어 이름을 저장합니다.
    /// </summary>
    /// <param name="name"></param>
    public static void setPlayedData(string name)
    {
        string path = Application.persistentDataPath + "\\" + "PlayerData.csv";

        TextWriter tw = new StreamWriter(path);

        tw.WriteLine("PlayerName" + "," + name);
        Debug.Log("Save Complete");

        tw.Close();
    }

    /// <summary>
    /// 현재 저장되어있는 PlayerData.csv를 삭제합니다.
    /// </summary>
    public static void deletePlayerData()
    {
        string path = Application.persistentDataPath + "\\" + "PlayerData.csv";

        File.Delete(path);

    }
}
