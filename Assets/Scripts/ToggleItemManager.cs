using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ToggleItemManager : MonoBehaviour
{
    #region singleton

    private static ToggleItemManager instance = null;

    public static ToggleItemManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }

    void Awake()
    {
        if (null == instance)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

    [SerializeField]
    List<TogglePannel> togglePannelList = new List<TogglePannel>();

    public GameObject togglePannelPrefeb;

    void Start()
    {
        togglePannelList = FindObjectsOfType<TogglePannel>().ToList();

        //CreateTogglePannel();
    }

    private void OnEnable()
    {
        SetTogglePannelList();
    }

    public void SetTogglePannelList()
    {
        if (togglePannelList.Count == 0)
            togglePannelList = FindObjectsOfType<TogglePannel>().ToList();
    }

    public void ChangToggleValue(bool value)
    {
        if(value)
        {
            var current = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Toggle>();
            for (int i = 0; i < togglePannelList.Count; i++)
            {
                foreach (Toggle toggle in togglePannelList[i].toggleItems)
                {
                    if (toggle.transform.parent != current.transform.parent)
                    {
                        toggle.isOn = false;
                    }
                }
            }
        }
    }

    //public void CreateTogglePannel()
    //{
    //    for (int i = 0; i < togglePannelList.Count; i++)
    //    {
    //        var o = Instantiate(togglePannelPrefeb, gameObject.transform);
    //        o.GetComponent<TogglePannel>().toggleIteminfos = togglePannelList[i].toggleIteminfos;
    //        o.GetComponent<TogglePannel>().CreateToggleItem();
    //        togglePannels.Add(o);
    //    }
    //}

}
