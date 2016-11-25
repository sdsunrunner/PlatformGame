using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class FloatUIManager
{
    // You should replace this with your own enum of FloatUI Types.
    public enum eFloatUIType
    {
        Example,
        BuildingTip,
        TutorialArrow,
        TutorialArrow2,
        TutorialArrowMainCore,
        BuildingTipForDebug,
    }

    static FloatUIManager sInstance;
    public static FloatUIManager instance
    {
    	get
    	{
    		if(sInstance == null)
    		{
    			sInstance = new FloatUIManager();
    		}
    		return sInstance;
    	}
    }

	private const string PATH_PREFIX = "Prefabs/";
	private Dictionary<eFloatUIType, FloatUIHandlerBase> mMonoFloatUIDic = new Dictionary<eFloatUIType, FloatUIHandlerBase>();

	public T AddPopupUI<T>(eFloatUIType type)
		where T: FloatUIHandlerBase
	{
		// only one instance globally
		if (type == eFloatUIType.Example)
		{
			if (mMonoFloatUIDic.ContainsKey(type))
			{
				// mMonoFloatUIDic[type].SetTargetTransform(t);
				return (T)mMonoFloatUIDic[type];
			}
			else
			{
				string name = PATH_PREFIX + GetPrefabUIName(type);
				GameObject prefab = Resources.Load(name) as GameObject;
				GameObject obj = GameObject.Instantiate(prefab) as GameObject;
				FloatUIHandlerBase handler = obj.GetComponent<FloatUIHandlerBase>();
				if (handler)
				{
					handler.Init();
					// handler.SetTargetTransform(t);
					mMonoFloatUIDic.Add(type, handler);
				}
				return (T)handler;
			}
		}
		// more than one instance
		else
		{
			string name = PATH_PREFIX + GetPrefabUIName(type);
			GameObject prefab = Resources.Load(name) as GameObject;
			GameObject obj = GameObject.Instantiate(prefab) as GameObject;
			FloatUIHandlerBase handler = obj.GetComponent<FloatUIHandlerBase>();
			if (handler)
				handler.Init();
			// 	handler.SetTargetTransform(t, false);

			return (T)handler;
		}
	}

	private string GetPrefabUIName(eFloatUIType type)
	{
		string name = "";
		switch (type)
		{
		case eFloatUIType.BuildingTip:
			name = "FloatUIBuilding_tip";
            break;
        case eFloatUIType.TutorialArrow:
            name = "FloatUITutorialArrow";
            break;
        case eFloatUIType.TutorialArrow2:
            name = "FloatUITutorialArrow2";
            break;

        case eFloatUIType.TutorialArrowMainCore:
            name = "FloatUITutorialArrowMainCore";
            break;
        case eFloatUIType.BuildingTipForDebug:
            name = "FloatUIBuilding_tipForDebug";
            break;

		default:
			break;
		}
		return name;
	}
}
