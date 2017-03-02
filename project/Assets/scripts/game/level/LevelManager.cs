using UnityEngine;
using System.Collections;

public enum Level
{
    Entry = 0,
    AppInit,
    Opening,
    GameLev_1, 
    None,
}
public class LevelManager
{
    static LevelManager _instance = null;
    private Level curLevel = Level.AppInit;
    private Level nextLevel = Level.None;

    public static LevelManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new LevelManager();

            return _instance;
        }
    }

    public Level GetCurLevel()
    {
        return curLevel;
    }

    public Level GetNextLevel()
    {
        return nextLevel;
    }

    public void LoadLevelAsync(Level _level)
    {
        curLevel = _level;
        nextLevel = Level.None;
    }   

    public string GetLevelName(Level _level)
    {
        string result = null;
        switch (_level)
        {
            case Level.Entry:
                result = "entry";
                break;
            case Level.AppInit:
                result = "appInit";
                break;

            case Level.Opening:
                result = "mainMenu";
                break;

            case Level.GameLev_1:
                result = "game_lv_1";
                break; 
        }
        return result;
    }
}

