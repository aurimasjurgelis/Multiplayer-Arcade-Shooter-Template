using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject leavePanel;
    public GameObject optionsPanel;
    public GameObject leaderboardsPanel;
    [Header("Multiplayer Functionality")]
    public static bool multiplayer = false;
    public GameObject multiplayerPanel;
    public static string teamName;

    bool leavePanelOpened = false;
    bool optionPanelOpened = false;
    bool leaderboardsPanelOpened = false;


    public void OpenLeavePanel()
    {
        if (leavePanelOpened)
        {
            leavePanel.SetActive(false);
            leavePanelOpened = false;
        }
        else
        {
            leavePanel.SetActive(true);
            leavePanelOpened = true;
        }
    }
    public void OpenLeaderboardsPanel()
    {
        if (leaderboardsPanelOpened)
        {
            leaderboardsPanel.SetActive(false);
            leaderboardsPanelOpened = false;
        }
        else
        {
            leaderboardsPanel.SetActive(true);
            leaderboardsPanelOpened = true;
        }
    }

    public void OpenOptionsPanel()
    {
        if (optionPanelOpened)
        {
            optionsPanel.SetActive(false);
            optionPanelOpened = false;
        }
        else
        {
            optionsPanel.SetActive(true);
            optionPanelOpened = true;
        }
    }

    public void HandleBulletColorInputData(int val)
    {
        if (val == 0)
        {
            OptionSettings.color = OptionSettings.Colors.orange;
        }
        if (val == 1)
        {
            OptionSettings.color = OptionSettings.Colors.red;

        }
        if (val == 2)
        {
            OptionSettings.color = OptionSettings.Colors.yellow;

        }
        if (val == 3)
        {
            OptionSettings.color = OptionSettings.Colors.green;

        }
        if (val == 4)
        {
            OptionSettings.color = OptionSettings.Colors.cyan;

        }
        if (val == 5)
        {

            OptionSettings.color = OptionSettings.Colors.blue;
        }
        if (val == 6)
        {

            OptionSettings.color = OptionSettings.Colors.violet;
        }


    }

    public void HandleBulletTypeInputData(int val)
    {
        if (val == 0)
        {
            OptionSettings.bulletType = OptionSettings.BulletType.circle;
        }
        if (val == 1)
        {
            OptionSettings.bulletType = OptionSettings.BulletType.triangle;
        }
        if (val == 2)
        {
            OptionSettings.bulletType = OptionSettings.BulletType.star;
        }
    }

    public void ChangeGameplayUIMode()
    {
        Debug.Log("Changing the ui mode!");
        //change ui
        //change the button text to darkmode/lightmode
    }

    public void QuitTheGame()
    {
        SceneManager.LoadScene(0);
    }
}
