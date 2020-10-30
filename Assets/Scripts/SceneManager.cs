using DanielLochner.Assets.SimpleScrollSnap;
using Gravitons.UI.Modal;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Debug = UnityEngine.Debug;

public enum ELEMENT
{
    None = 0,
    A = 1 << 0,
    B = 1 << 1,
    C = 1 << 2,
    D = 1 << 3,
    E = 1 << 4,
    AB = A | B,
    All = int.MaxValue
};

public class SceneManager : MonoBehaviour
{
    public RectTransform main;
    public RectTransform title;
    public RectTransform guide;
    public RectTransform mode;
    public RectTransform topUi;
    public RectTransform learning;
    public RectTransform test;
    private ELEMENT element;
    public enum PAGE
    {
        TITLE,
        MAIN,
        GUIDE,
        MODE,
        LEARNING,
        TEST,
        TOPUI
    }

    public PAGE currentPage = PAGE.TITLE;
    public PAGE prevePage = PAGE.TITLE;
    // Start is called before the first frame update
    void Start()
    {
        //DisablePage();
        //title.gameObject.SetActive(true);
    }

    public void SetButton()
    {
        simpleScrollSnap = FindObjectOfType<SimpleScrollSnap>();
        simpleScrollSnap.onPanelChanged.AddListener(ArrowState);
        Debug.Log($"{simpleScrollSnap.gameObject.name} GetInstanceID : {simpleScrollSnap.gameObject.GetInstanceID()}");
    }
    #region Enum flag 연산

    public ELEMENT AddEnum(ELEMENT oriEnum, ELEMENT addenum)
    {
        oriEnum |= addenum;
        return oriEnum;
    }
    public ELEMENT RemoveEnum(ELEMENT oriEnum, ELEMENT addenum)
    {
        oriEnum &= ~addenum;
        return oriEnum;
    }
    public bool CheckExistsEnum(ELEMENT oriEnum, ELEMENT addenum)
    {
        return (oriEnum & addenum) != 0;
    }
    public ELEMENT InvertEnum(ELEMENT oriEnum, ELEMENT addenum)
    {
        oriEnum ^= addenum;
        return oriEnum;
    }
    public ELEMENT DeleteAllEnum(ELEMENT oriEnum)
    {
        oriEnum = ELEMENT.None;
        return oriEnum;
    }
    public ELEMENT SetAllEnum(ELEMENT oriEnum)
    {
        oriEnum = ELEMENT.All;
        return oriEnum;
    }
    public ELEMENT SetAllExceptEnum(ELEMENT oriEnum, params ELEMENT[] addenums)
    {
        oriEnum = ELEMENT.All;
        for (int i = 0; i < addenums.Length; i++)
        {
            oriEnum = RemoveEnum(oriEnum, addenums[i]);
        }
        return oriEnum;
    }

    #endregion


    DanielLochner.Assets.SimpleScrollSnap.SimpleScrollSnap simpleScrollSnap;
    private void ArrowState()
    {
        if (simpleScrollSnap.CurrentPanel == 0)
        {
            simpleScrollSnap.previousButton.gameObject.SetActive(false);
            simpleScrollSnap.nextButton.gameObject.SetActive(true);
        }
        else if (simpleScrollSnap.CurrentPanel == simpleScrollSnap.Panels.Length - 1)
        {
            simpleScrollSnap.previousButton.gameObject.SetActive(true);
            simpleScrollSnap.nextButton.gameObject.SetActive(false);
        }
    }

    public void InitGoTitle()
    {
        currentPage = PAGE.TITLE;
        MovePage(currentPage);
    }
    public void GoMainPage()
    {
        currentPage = PAGE.MAIN;
        MovePage(currentPage);
    }
    public void OpenModePage()
    {
        string name = EventSystem.current.currentSelectedGameObject.name;
        Debug.Log("call : " + name);
        currentPage = PAGE.MODE;
        MovePage(currentPage);
    }
    public void OpenLearning()
    {
        currentPage = PAGE.LEARNING;
        MovePage(currentPage);
    }
    public void OpenTest()
    {
        currentPage = PAGE.TEST;
        MovePage(currentPage);
    }
    public void OpenGuide()
    {
        var checkItem = EventSystem.current.currentSelectedGameObject.transform.parent.GetComponentInChildren<TogglePannel>().GetToggleItemAny();
        if (!checkItem)
        {
            ShowLearningCheckNullAlert();
            return;
        }

        prevePage = currentPage;
        currentPage = PAGE.GUIDE;
        MovePage(currentPage);
    }

    void MovePage(PAGE page)
    {
        switch (page)
        {
            case PAGE.TITLE:
                DisablePage();
                prevePage = PAGE.TITLE;
                title.gameObject.SetActive(true);
                break;
            case PAGE.MAIN:
                DisablePage();
                prevePage = PAGE.TITLE;
                main.gameObject.SetActive(true);
                SetButton();
                //topUi.gameObject.SetActive(true);
                break;
            case PAGE.GUIDE:
                DisablePage();
                guide.gameObject.SetActive(true);
                //topUi.gameObject.SetActive(true);
                SetButton();
                break;
            case PAGE.MODE:
                DisablePage();
                prevePage = PAGE.MAIN;
                mode.gameObject.SetActive(true);
                break;
            case PAGE.LEARNING:
                DisablePage();
                prevePage = PAGE.MODE;
                element = ELEMENT.None;
                learning.gameObject.SetActive(true);
                ToggleItemManager.Instance.SetTogglePannelList();
                break;
            case PAGE.TEST:
                DisablePage();
                prevePage = PAGE.MODE;
                element = ELEMENT.None;
                test.gameObject.SetActive(true);
                break;
            default:
                break;
        }

    }

    void DisablePage()
    {
        title.gameObject.SetActive(false);
        main.gameObject.SetActive(false);
        guide.gameObject.SetActive(false);
        mode.gameObject.SetActive(false);
        learning.gameObject.SetActive(false);
        test.gameObject.SetActive(false);
        //topUi.gameObject.SetActive(false);
    }

    /// <summary>
    /// Shows a modal with callback
    /// </summary>
    public void ShowModalWithCallback()
    {
        //var mc = new GenericModalContent()
        //{
        //    Body = "GenericModalContent test",
        //    Title = "test111"
        //};

        ModalManager.Show(null, "상세 리스트를 선택 후 선택하기를 눌러주세요.", new[] { new ModalButton() { Text = "확인" } });
    }

    public void ShowMainAlert()
    {
        ModalManager.Show(null, $"본 버전에서는 활력징후만 지원합니다.{Environment.NewLine}활력징후를 선택해주세요.", new[] { new ModalButton() { Text = "확인" } });
    }
    public void ShowGuideAlert()
    {
        ModalManager.Show(null, $"본 버전에서는 활력징후만 지원합니다.{Environment.NewLine}활력징후를 선택해주세요.", new[] { new ModalButton() { Text = "확인" } });
    }
    public void ShowModeAlert()
    {
        ModalManager.Show(null, $"본 버전에서는 학습하기만 지원합니다.{Environment.NewLine}학습하기를 선택해주세요.", new[] { new ModalButton() { Text = "확인" } });
    }
    public void ShowLearningAlert()
    {
        ModalManager.Show(null, "본 버전에서는 혈압측정만 체험 가능합니다.", new[] { new ModalButton() { Text = "확인" } });
    }
    public void ShowLearningCheckNullAlert()
    {
        ModalManager.Show(null, "상세 리스트를 선택 후 선택하기를 눌러주세요.", new[] { new ModalButton() { Text = "확인" } });
    }
    public void ShowTestAlert()
    {
        ModalManager.Show(null, "본 버전에서는 활력징후만 지원합니다.\r활력징후를 선택해주세요.", new[] { new ModalButton() { Text = "확인" } });
    }
    public void ShowTestCheckNullAlert()
    {
        ModalManager.Show(null, "상세 리스트를 선택 후 선택하기를 눌러주세요.", new[] { new ModalButton() { Text = "확인" } });
    }

    Modal md;
    /// <summary>
    /// Change background color to a random color
    /// </summary>
    private void ChangeColor()
    {
        md.Close();
    }

    public void BackPage()
    {
        currentPage = prevePage;
        MovePage(prevePage);
    }
}
