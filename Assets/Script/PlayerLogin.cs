using UnityEngine;
using System.IO;

public class PlayerLogin : MonoBehaviour
{

    /// <summary>
    /// persistentDataPath�� PlayerData�κ��� Player�� Name�� �����ɴϴ�. �̹� �����Ǿ� �ִ� ������ �����ٸ�, null�� ��ȯ�մϴ�.
    /// </summary>
    /// <returns></returns>
    public static string getPlayerData()
    {
        string path = Application.persistentDataPath + "\\" + "PlayerData.csv";

        TextReader tr;

        try
        {
            tr = new StreamReader(path);

            //ù��°�� colomn ���� �����̶� 2��°�Ÿ� ������
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
    /// PlayerData.csv ���Ͽ� �÷��̾� �̸��� �����մϴ�.
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
    /// ���� ����Ǿ��ִ� PlayerData.csv�� �����մϴ�.
    /// </summary>
    public static void deletePlayerData()
    {
        string path = Application.persistentDataPath + "\\" + "PlayerData.csv";

        File.Delete(path);

    }
}
