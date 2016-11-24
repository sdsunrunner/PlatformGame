using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ClientTableDataManager
{
    static ClientTableDataManager s_pInstance = null;
    public static ClientTableDataManager Instance
    {
        get
        {
            if (s_pInstance == null)
            {
                s_pInstance = new ClientTableDataManager();
            }
            return s_pInstance;
        }
    }

    public static void Reset()
    {
        s_pInstance = null;
        s_pInstance = new ClientTableDataManager();
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
