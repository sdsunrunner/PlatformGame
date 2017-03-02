using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class InuData 
{
    public const string kSpliterState = "|||";
    public const string kSpliterCmd = ";";
    public const string kSpliterParam = ",";
    public const string kSpliterDef = ":";
    public const string kSpliterVector = "*";

    static Dictionary<string, object> sHashDicData = new Dictionary<string, object>();

    public static Dictionary<InuDefine.EInuState, List<Dictionary<string, object>>> ParseData(string _data)
    {
        if (Application.isPlaying)
        {
            if (sHashDicData == null)
                sHashDicData = new Dictionary<string, object>();

            if (_data != null)
            {
                if (sHashDicData.ContainsKey(_data))
                {
                    return (Dictionary<InuDefine.EInuState, List<Dictionary<string, object>>>)sHashDicData[_data];
                }
            }
        }

        Dictionary<InuDefine.EInuState, List<Dictionary<string, object>>> result = new Dictionary<InuDefine.EInuState, List<Dictionary<string, object>>>();

        if (_data != null && _data != string.Empty)
        {
            string[] stateStrings = _data.Split(new string[] { kSpliterState }, StringSplitOptions.None);
            string[] cmdStrings;
            string[] paramStrings;

            // all states string
            for (int i = 0; i < stateStrings.Length; i++)
            {
                InuDefine.EInuState state = InuDefine.EInuState.eIdle;

                cmdStrings = stateStrings[i].Split(new string[] { kSpliterCmd }, StringSplitOptions.None);

                if (cmdStrings.Length > 0)
                {
                    // first string of cmd strings is state enum
                    state = (InuDefine.EInuState)(short.Parse(cmdStrings[0]));

                    // convert all strings to cmd hashtable
                    List<Dictionary<string, object>> listHashtable = new List<Dictionary<string, object>>();
                    for (int j = 1; j < cmdStrings.Length; j++)
                    {
                        string cmdString = cmdStrings[j];
                        paramStrings = cmdString.Split(new string[] { kSpliterParam }, StringSplitOptions.None);
                        Dictionary<string, object> table = new Dictionary<string, object>();

                        if (paramStrings.Length > 0)
                        {
                            // parse each parameter
                            for (int k = 0; k < paramStrings.Length; k++)
                            {
                                ConvertStringToHashtable(paramStrings[k], table);
                            }

                            listHashtable.Add(table);
                        }
                    }

                    result.Add(state, listHashtable);
                }
                else
                {
                    Debug.Log("error when parsing cmd string length is zero: " + stateStrings[i]);                    
                }
            }

            if (Application.isPlaying)
            {
                sHashDicData.Add(_data, result);
            }
        }

        return result;
    }

    static void ConvertStringToHashtable(string _paramString, Dictionary<string, object> _hash)
    {
        string[] param = _paramString.Split(new string[] { kSpliterDef }, StringSplitOptions.None);
        if (param.Length != 2)
        {
            Debug.Log("error when parsing Inu string: " + _paramString);
            return;
        }

        string _paramName = param[0];
        string _paramValue = param[1];

        if (_paramName == InuParam.cmd)
        {
            short val = short.Parse(_paramValue);
            _hash.Add(_paramName, (InuCmd)val);
        }
        else if (_paramName == InuParam.time)
        {
            float val = float.Parse(_paramValue);
            _hash.Add(_paramName, val);
        }
        else if (_paramName == InuParam.step)
        {
            short val = short.Parse(_paramValue);
            _hash.Add(_paramName, val);
        }
        else if (_paramName == InuParam.name || _paramName == InuParam.animname || _paramName == InuParam.dummy)
        {
            _hash.Add(_paramName, _paramValue);
        }
        else if (_paramName == InuParam.during)
        {
            float val = float.Parse(_paramValue);
            _hash.Add(_paramName, val);
        }
        else if (_paramName == InuParam.destroy)
        {
            bool val = bool.Parse(_paramValue);
            _hash.Add(_paramName, val);
        }
        else if (_paramName == InuParam.amount1)
        {
            float val = float.Parse(_paramValue);
            _hash.Add(_paramName, val);
        }
        else if (_paramName == InuParam.amount2)
        {
            string[] vectors = _paramValue.Split(new string[] { kSpliterVector }, StringSplitOptions.None);
            if (vectors.Length == 2)
            {
                float x = float.Parse(vectors[0]);
                float y = float.Parse(vectors[1]);
                _hash.Add(_paramName, new Vector2(x, y));
            }
        }
        else if (_paramName == InuParam.amount3)
        {
            string[] vectors = _paramValue.Split(new string[] { kSpliterVector }, StringSplitOptions.None);
            if (vectors.Length == 3)
            {
                float x = float.Parse(vectors[0]);
                float y = float.Parse(vectors[1]);
                float z = float.Parse(vectors[2]);
                _hash.Add(_paramName, new Vector3(x, y, z));
            }
        }
        else if (_paramName == InuParam.looptype)
        {
            short val = short.Parse(_paramValue);
            _hash.Add(_paramName, (iTween.LoopType)val);
        }
        else if (_paramName == InuParam.easetype)
        {
            short val = short.Parse(_paramValue);
            _hash.Add(_paramName, (iTween.EaseType)val);
        }
        else if (_paramName == InuParam.random)
        {
            bool val = bool.Parse(_paramValue);
            _hash.Add(_paramName, val);
        }
        else if (_paramName == InuParam.axisstrict)
        {
            short val = short.Parse(_paramValue);
            _hash.Add(_paramName, (InuDefine.EAxisStrict)val);
        }
        else if (_paramName == InuParam.projectile)
        {
            short val = short.Parse(_paramValue);
            _hash.Add(_paramName, (InuDefine.EInuAmmoType)val);
        }
        else if (_paramName == InuParam.effectskip)
        {
            int val;
            int.TryParse(_paramValue, out val);
            _hash.Add(_paramName, val);
        }
    }
}

