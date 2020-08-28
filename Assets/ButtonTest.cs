using System.Collections;
using System.Collections.Generic;
using Unity.UIElements.Runtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class ButtonTest : MonoBehaviour
{
    public PanelRenderer m_MainWindow;
    public PanelRenderer m_PlayerOptions;
    public VisualTreeAsset m_MainWindowUxml;
    public StyleSheet m_MainWindowStyles;

    private void OnEnable()
    {
        m_MainWindow.postUxmlReload = BindMainMenuScreen;
        var treeElement = m_MainWindowUxml.CloneTree();
    }
    private IEnumerable<Object> BindMainMenuScreen()
    {
        var root = m_MainWindow.visualTree;

        var startButton = root.Q<Button>("avatar");
        if (startButton != null)
        {
            startButton.clickable.clicked += () =>
            {
                Debug.LogError("Working");
            };
        }
        return null;
    }

}
