using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
public abstract class TableData
{
    // table
    protected RawTable mTableData;
    protected uint mHash = 0;
    static Dictionary<int, uint> s_hashDictionary = new Dictionary<int, uint>();
    public static void Reset()
    {
        s_hashDictionary.Clear();
    }
    static void CheckHash(string _path, uint _hash)
    {
        int key = _path.GetHashCode();
        uint oldHash;
        if (s_hashDictionary.TryGetValue(key, out oldHash))
        {
            if (oldHash != _hash)
            {
                //if (Constants.CRASH_GAME_WHEN_HACKED) Application.Quit();
            }
        }
        else
            s_hashDictionary.Add(key, _hash);
    }

    public static uint ComputeHash(byte[] s)
    {
        uint h = 0;
        for (int i = s.Length - 1; i >= 0; --i)
        {
            h = (h << 5) - h + s[i];
        }
        return h;
    }
    public static uint ComputeHash(char[] s)
    {
        uint h = 0;
        for (int i = s.Length - 1; i >= 0; --i)
        {
            h = (h << 5) - h + s[i];
        }
        return h;
    }
    protected virtual uint GetHash()
    {
        return 0;
    }
    protected abstract string GetPath();
    protected abstract void _ParseData();

    public uint CheckHash()
    {
        uint hash = GetHash();

        CheckHash(GetPath(), hash);

        return hash;
    }
    public void ReadTable()
    {
        if (mTableData == null)
            mTableData = new RawTable();

        mTableData.readBinary(GetPath());
    }
    public void ParseData()
    {
        _ParseData();
        CheckHash();
        mTableData = null;
    }
}


#region Units

public class ObjUnitConfigBaseInfo
{
    public int mUnitId;

}
public class TableUnitsBaseInfo : TableData
{
    // file path
    public readonly string sFilePath = "tUnits";
    protected override string GetPath()
    {
        return (sFilePath);
    }

    private Dictionary<int, ObjUnitConfigBaseInfo> mData;
    protected override void _ParseData()
    {
        mData = new Dictionary<int, ObjUnitConfigBaseInfo>();
        for (int i = 0; i < mTableData._nRows; i++)
        {
            ObjUnitConfigBaseInfo configData = new ObjUnitConfigBaseInfo();
            configData.mUnitId = mTableData.GetInt(i, DataDefine.Units_Unit_ID);           

            mData[configData.mUnitId] = configData;
        }
    }
    public ObjUnitConfigBaseInfo GetConfigInfoById(int _id)
    {
        if (null != mData && mData.ContainsKey(_id))
            return mData[_id];
        return null;
    }
}
#endregion