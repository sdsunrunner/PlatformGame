/*
 * RawDataTableReader 
 * Cosmosliu: 2011-10-9
 * define basic data of a table
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System;

public class RawTable
{
    static List<ulong> s_hashTable;

    static List<ulong> HashTable
    {
        get
        {
            if (s_hashTable == null)
            {
                s_hashTable = new List<ulong>();
                TextAsset text = null;
#if UNITY_EDITOR
                //if (Constants.EDITOR_USE_ASSETBUNDLE)
                //{
                //    text = ResourceLibrary.instance.Load(AssetBundleManager.EBundleType.eDat, "hash") as TextAsset;
                //}
                //else
                {
                    text = (TextAsset)UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Resources/dat/hash.txt", typeof(TextAsset));
                }
#else
                text = ResourceLibrary.instance.Load(AssetBundleManager.EBundleType.eDat, "hash") as TextAsset;
                if(text == null)
                {
                	text =  Resources.Load("dat1/hash", typeof(TextAsset)) as TextAsset;
                }
#endif
                string[] hashList = text.text.Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string hash in hashList)
                {
                    ulong uint_hash = ulong.Parse(hash);
                    s_hashTable.Add(~uint_hash);
                }

            }
            return s_hashTable;
        }
    }
    public static ulong GetHash(int index)
    {
        return ~s_hashTable[index];
    }

    public string[,] _data;
    public int _nRows;
    public int _nColumns;

    //read binary data
    public void readBinary(string tableName)
    {
        ClearData();

        TextAsset binaryStream = null;

#if UNITY_EDITOR
        //if (Constants.EDITOR_USE_ASSETBUNDLE)
        //{
        //    binaryStream = ResourceLibrary.instance.Load(AssetBundleManager.EBundleType.eDat, tableName) as TextAsset;
        //}
        //else
        {
            binaryStream = (TextAsset)UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Resources/dat/" + tableName + ".bytes", typeof(TextAsset));
        }
#else
		binaryStream = ResourceLibrary.instance.Load(AssetBundleManager.EBundleType.eDat, tableName) as TextAsset;
		if(binaryStream == null)
		{
			binaryStream =  Resources.Load("dat1/" + tableName, typeof(TextAsset)) as TextAsset;
		}
#endif

        if (binaryStream == null)
        {
            DebugUtils.Log("Error reading table:" + tableName);
            return;
        }
        if (!HashTable.Contains(~ComputeHash(binaryStream.bytes)))
        {
            //if (Constants.CRASH_GAME_WHEN_HACKED) Application.Quit();
        }

        byte[] bytes = TEA.decode(binaryStream.bytes);

        MemoryStream ms = new MemoryStream(bytes);
        BinaryReader br = new BinaryReader(ms, Encoding.Unicode);

        int columns = 0, rows = 0;
        rows = br.ReadInt32();
        columns = br.ReadInt32();

        _nRows = rows;
        _nColumns = columns;

        if (_nRows == 0 || _nColumns == 0)
            DebugUtils.Log("Error reading tablesize Rows is " + _nRows.ToString() + " and _nColumns is " + _nColumns.ToString() + ".");
        _data = new string[_nRows, _nColumns];
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                _data[i, j] = br.ReadString();
            }
        }
    }

    public string GetStr(int row, int column)
    {
        if (row < _nRows && column < _nColumns)
            return _data[row, column];
        else
        {
            DebugUtils.Log("Error reading Row: " + row.ToString() + " Columns: " + column.ToString() + ".");
            return string.Empty;
        }
    }

    public int GetInt(int row, int column)
    {
        string result = GetStr(row, column);
        if (result != string.Empty)
        {
            return int.Parse(result);
        }
        else
        {
            DebugUtils.Log("Error when try int.parse in Row: " + row.ToString() + " Columns: " + column.ToString() + ".");
            return 0;
        }
    }

    public short GetShort(int row, int column)
    {
        string result = GetStr(row, column);
        if (result != string.Empty)
        {
            return short.Parse(result);
        }
        else
        {
            DebugUtils.Log("Error when try short.parse in Row: " + row.ToString() + " Columns: " + column.ToString() + ".");
            return 0;
        }
    }

    public byte GetByte(int row, int column)
    {
        string result = GetStr(row, column);
        if (result != string.Empty)
        {
            return byte.Parse(result);
        }
        else
        {
            DebugUtils.Log("Error when try byte.parse in Row: " + row.ToString() + " Columns: " + column.ToString() + ".");
            return 0;
        }
    }

    public float GetFloat(int row, int column)
    {
        string result = GetStr(row, column);
        if (result != string.Empty)
        {
            return float.Parse(result);
        }
        else
        {
            DebugUtils.Log("Error when try float.parse in Row: " + row.ToString() + " Columns: " + column.ToString() + ".");
            return 0;
        }
    }

    public void ClearData()
    {
        _data = null;
    }

    ulong ComputeHash(byte[] s)
    {
        ulong hash = 0x9A9AA99A;
        for (int i = 0; i < s.Length; i++)
        {
            if ((i & 1) == 0)
            {
                hash ^= ((hash << 7) ^ s[i] ^ (hash >> 3));
            }
            else
            {
                hash ^= (~((hash << 11) ^ s[i] ^ (hash >> 5)));
            }
        }
        return hash;
    }
}