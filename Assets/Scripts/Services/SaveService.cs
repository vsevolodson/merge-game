using UnityEngine;

public class SaveService
{
    private const string SaveKey = "Save";

    public void Save(SaveData data)
    {
        string json = JsonUtility.ToJson(data);

        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();

        Debug.LogWarning("saving");
    }

    public SaveData Load()
    {
        if (!PlayerPrefs.HasKey(SaveKey))
        {
            return null;
        }

        try
        {
            string json = PlayerPrefs.GetString(SaveKey);

            SaveData saveData = JsonUtility.FromJson<SaveData>(json);

            if (saveData == null)
            {
                return null;
            }

            return saveData;
        }
        catch
        {
            Debug.LogWarning("Save load error");

            DeleteSave();

            return null;
        }
    }

    public void DeleteSave()
    {
        PlayerPrefs.DeleteKey(SaveKey);
    }
}