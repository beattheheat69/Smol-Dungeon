using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour
{
    [HideInInspector]
    public List<SmolUI.TabButton> tabButtons; //The list of every button

    [Header("Colors")] //The colors of every state of the tab
    [SerializeField] public Color tabHover;
    [SerializeField] public Color tabActive;
    [SerializeField] public Color tabInactive;
    private SmolUI.TabButton selectedTab; //The currently activated tag

    public List<GameObject> pages = new List<GameObject>();

    private void Awake()
    {
        //Initialize the list tabButtons
        tabButtons = new List<SmolUI.TabButton>();
    }

    private void Start()
    {
        //Select the default tab
        foreach (SmolUI.TabButton button in tabButtons)
        {
            if (button.startsSelected)
            {
                selectedTab = button;
            }
        }
    }

    public void Subscribe(SmolUI.TabButton tabButton)
    {
        tabButtons.Add(tabButton);
    }

    public void OnTabEnter(SmolUI.TabButton tabButton)
    {
        ResetTabs();
        if (selectedTab == null || tabButton != selectedTab)
        {
            tabButton.background.color = tabHover;
        }
    }

    public void OnTabExit(SmolUI.TabButton tabButton)
    {
        ResetTabs();
    }

    public void OnTabSelected(SmolUI.TabButton tabButton)
    {
        selectedTab = tabButton;
        tabButton.background.color = tabActive;
        ResetTabs();
        int index = tabButton.transform.GetSiblingIndex();
        for (int i = 0; i < pages.Count; i++)
        {
            if (i == index)
            {
                pages[i].SetActive(true);
            }
            else
            {
                pages[i].SetActive(false);
            }
        }
    }

    private void ResetTabs()
    {
        foreach (SmolUI.TabButton tabButton in tabButtons)
        {
            if (selectedTab != null && tabButton == selectedTab)
            {
                continue;
            }
            tabButton.background.color = tabInactive;
        }
    }
}