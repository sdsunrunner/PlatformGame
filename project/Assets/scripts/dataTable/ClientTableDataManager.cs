using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ClientTableDataManager
{
    static ClientTableDataManager mInstance = null;
    public static ClientTableDataManager Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = new ClientTableDataManager();
            }
            return mInstance;
        }
    }

    public static void Reset()
    {
        mInstance = null;
        mInstance = new ClientTableDataManager();
    }
    public void Init()
    { 
        InitUnitBaseInfo();       
    }
    void InitUnitBaseInfo()
    {
        m_TableUnitsBaseInfo = new TableUnitsBaseInfo();
        m_TableUnitsBaseInfo.ReadTable();
        m_TableUnitsBaseInfo.ParseData();
    }


    TableUnitsBaseInfo m_TableUnitsBaseInfo;
#region Units
    public ObjUnitConfigBaseInfo GetUnitConfigBaseInfoById(int _id)
    {
        if (null == m_TableUnitsBaseInfo)
        {
            m_TableUnitsBaseInfo = new TableUnitsBaseInfo();
            m_TableUnitsBaseInfo.ReadTable();
            m_TableUnitsBaseInfo.ParseData();
        }
        return m_TableUnitsBaseInfo.GetConfigInfoById(_id);

    }
#endregion
}
