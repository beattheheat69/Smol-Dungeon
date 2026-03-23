using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SmolUI
{
    public class TabMenu : MonoBehaviour
    {
        [Header("Selected Tab Index")]
        [SerializeField] private int pageIndex = 0;

        [Header("Components")]
        [SerializeField] private ToggleGroup toggleGroup;
        [SerializeField] private List<Toggle> tabs = new List<Toggle>();
        [SerializeField] private List<CanvasGroup> pages = new List<CanvasGroup>();

        private void Initialize()
        {
            //Initialize the toggleGroup
            toggleGroup = GetComponentInChildren<ToggleGroup>();

            //Clear the lists and initialize them with the current elements
            tabs.Clear();
            tabs.AddRange(GetComponentsInChildren<Toggle>());

            pages.Clear();
            pages.AddRange(GetComponentsInChildren<CanvasGroup>());

            //Open the default selected index
            OpenPage(pageIndex);
        }

        private void Start()
        {
            //Assign a Listener to every tab (activated when toggled)
            foreach (var toggle in tabs)
            {
                toggle.onValueChanged.AddListener(CheckForTab);
                toggle.group = toggleGroup;
            }
        }

        private void OnDestroy()
        {
            //Remove the Listener on every tab when the menu is destroyed
            foreach (var toggle in tabs)
            {
                toggle.onValueChanged.RemoveListener(CheckForTab);
            }
        }

        private void CheckForTab(bool value)
        {
            //Find the index of the active tab
            for (int i = 0; i < tabs.Count; i++)
            {
                if (!tabs[i].isOn) continue;
                pageIndex = i;
            }

            //Display the page corresponding to the tab
            OpenPage(pageIndex);
        }

        private void OpenPage(int index)
        {
            ChangePageIndex(index);

            for (int i = 0; i < pages.Count; i++)
            {
                //Determine if the index of the page is currently active (only true for the one active page)
                bool isActivePage = (i == pageIndex);

                //Toggle the Canvas properties of the page
                pages[i].alpha = isActivePage ? 1.0f : 0.0f;
                pages[i].interactable = isActivePage;
                pages[i].blocksRaycasts = isActivePage;
            }

        }

        private void ChangePageIndex(int index)
        {
            //Avoid running the function if the tabs or pages are empty
            if (tabs.Count == 0 || pages.Count == 0)
            {
                Debug.Log("Pages or Tabs null");
                return;
            }

            //Clamp the current index between possible values
            pageIndex = Mathf.Clamp(index, 0, pages.Count - 1);
        }
    }
}