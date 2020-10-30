using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TogglePannel : ToggleGroup
{
    public ELEMENT elementAll = ELEMENT.None;

    [SerializeField]
    public List<Toggle> toggleItems = new List<Toggle>();

    public GameObject togglePrefeb;

    protected override void Start()
    {
        base.Start();

        var toggles = gameObject.GetComponentsInChildren<Toggle>();

        for (int i = 0; i < toggles.Length; i++)
        {
            //toggles[i].group = this;
            RegisterToggle(toggles[i]);
            toggleItems.Add(toggles[i]);
        }

        m_Toggles.ForEach((x) =>
        {
            x.onValueChanged.AddListener(ToggleItemManager.Instance.ChangToggleValue);
        });
    }

    public bool GetToggleItemAny()
    {
        for (int i = 0; i < toggleItems.Count; i++)
        {
            if (toggleItems[i].isOn == true)
            {
                return true;
            }
        }
        return false;
    }
}
