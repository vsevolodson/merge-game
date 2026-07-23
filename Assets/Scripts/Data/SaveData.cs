using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public int currentLevel;

    public List<CellSaveData> cells = new();

    public bool soundEnabled = true;
}