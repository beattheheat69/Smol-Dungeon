using System.Collections.Generic;
using System;
using UnityEngine;

public class IMGUIHelper : MonoBehaviour
{
    public static Action OnGenGridButtonPressed;
    public static Action OnResetGridButtonPressed;
    public static Action OnValidateDungeonButtonPressed;
    public static Action OnExploreDungeonButtonPressed;

    public IMGUIHelperConfig Config;
    public bool DrawGizmos;

    private static IMGUIHelper _instance;
    public static IMGUIHelper Instance { get { return _instance; } }
    private Vector2 _scrollViewVector;
    private List<string> _gridEventLogs = new List<string>();

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void AddGridEventLog(string log)
    {
        _gridEventLogs.Add(log);
        // Remove old logs to make space for new ones if we exceed the max log count
    }

    private void OnGUI()
    {
        float nextLineOffset = Config.TextHeight + Config.InterlineSpacing;

        // DUNGEON GRID AND DUNGEON VALIDATION BUTTONS
        if (GUI.Button(new Rect(Screen.width - Config.ButtonWidth - Config.MarginRight, Screen.height - Screen.height / 4 - Config.ButtonHeight, Config.ButtonWidth, Config.ButtonHeight), "Generate Grid"))
        {
            OnGenGridButtonPressed?.Invoke();
        }
        if (GUI.Button(new Rect(Screen.width - Config.ButtonWidth - Config.MarginRight, Screen.height - Screen.height / 4, Config.ButtonWidth, Config.ButtonHeight), "Reset Dungeon"))
        {
            OnResetGridButtonPressed?.Invoke();
        }
        if (GUI.Button(new Rect(Screen.width - Config.ButtonWidth - Config.MarginRight, Screen.height - Screen.height / 4 + Config.ButtonHeight, Config.ButtonWidth, Config.ButtonHeight), "Validate Dungeon"))
        {
            OnValidateDungeonButtonPressed?.Invoke();
        }
        if (GUI.Button(new Rect(Screen.width - Config.ButtonWidth - Config.MarginRight, Screen.height - Screen.height / 4 + 2 * Config.ButtonHeight, Config.ButtonWidth, Config.ButtonHeight), "Explore Dungeon"))
        {
            OnExploreDungeonButtonPressed?.Invoke();
        }
        // END

        // GRID LOG
        float gridLogWidth = 570f;
        float scrollViewWidth = 550f;
        float scrollViewHeight = 600f;
        float scrollViewStartY = Config.MarginTop + nextLineOffset;
        float linePos = scrollViewStartY;

        GUI.Box(new Rect(Screen.width - (gridLogWidth + Config.MarginRight), Config.MarginTop, gridLogWidth, 650f), " -- GRID EVENTS LOG -- ");
        Rect positionRect = new Rect(Screen.width - gridLogWidth, scrollViewStartY, scrollViewWidth, scrollViewHeight);
        Rect viewRect = new Rect(Screen.width - gridLogWidth, scrollViewStartY, scrollViewWidth, 2 * scrollViewHeight);
        _scrollViewVector = GUI.BeginScrollView(positionRect, _scrollViewVector, viewRect);

        foreach (string log in _gridEventLogs)
        {
            GUI.Label(new Rect(Screen.width - gridLogWidth, linePos, scrollViewWidth, Config.TextHeight), log);
            linePos += nextLineOffset;
        }
        GUI.EndScrollView();
        // END  
    }

    private void OnEnable()
    {
        InteractiveGrid.OnGridEventLogAdded += AddGridEventLog;
    }

    private void OnDisable()
    {
        InteractiveGrid.OnGridEventLogAdded -= AddGridEventLog;
    }

    private void OnDestroy()
    {
        InteractiveGrid.OnGridEventLogAdded -= AddGridEventLog;
    }
}
