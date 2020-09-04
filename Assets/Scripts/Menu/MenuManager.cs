using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class MenuManager : MonoBehaviour
{
    [Header("Global Buttons & Settings")]
    public Button optionsButton;
    public Button closeButton;
    public int inputAllowance = 4;

    [Header("Login Panel")]
    public GameObject loginPanel;
    public TMP_InputField usernameLoginInput;
    public TMP_InputField passwordLoginInput;
    public Button loginButton;

    [Header("Main Panel")]
    public GameObject mainPanel;

    [Header("Password Change Panel")]
    public GameObject changePasswordPanel;
    public TMP_InputField securityCodeInput;
    public TMP_InputField usernamePassChangeInput;
    public TMP_InputField passwordChangeInput;
    public TMP_InputField repeatPasswordChangeInput;
    public Button changePasswordButton;

    [Header("Register Panel")]
    public GameObject registerPanel;
    public TMP_InputField usernameRegisterInput;
    public TMP_InputField passwordRegisterInput;
    public Button signUpButton;

    [Header("Options Panel")]
    public GameObject optionsPanel;
    public TextMeshProUGUI changeUIModeButtonText;
    public bool optionsPanelOpened = false;

    [Header("Error Panel")]
    public GameObject errorPanel;
    public TextMeshProUGUI errorTextMeshProUGUI;
    public Button backErrorButton;

    [Header("Message Panel")]
    public GameObject messagePanel;
    public TextMeshProUGUI messageTextMeshProUGUI;
    public Button messageOkButton;

    [Header("Leaderboards Panel")]
    public GameObject leaderboardsPanel;

    [Header("Debug")]
    public TextMeshProUGUI debugTextMeshProUGUI;

    [Header("UI Mode")]
    public Sprite darkButton;
    public Sprite darkInputField;
    public Sprite darkPanel;
    public Sprite darkOptionsButton;
    public Sprite darkExitButton;
    public Sprite lightButton;
    public Sprite lightInputField;
    public Sprite lightPanel;
    public Sprite lightOptionsButton;
    public Sprite lightExitButton;

    public new Camera camera;
    public Color darkModeColor;

    [Header("UI Mode Lists")]
    public List<TextMeshProUGUI> texts;
    public List<Button> buttons;
    public List<Image> panels;
    public List<TMP_InputField> inputfields;

    UIStates ui = UIStates.DarkMode;
    
    public enum UIStates
    {
        LightMode,
        DarkMode
    }


    public enum PanelStates
    {
        Login,
        Main,
        ChangePassword,
        Register,
        Options,
        Message,
        Error,
        Leaderboards
    };

    public PanelStates states = PanelStates.Login;

    public void CloseButton()
    {
        Debug.Log("State" + states);
        switch (states)
        {
            case PanelStates.Login:
                optionsButton.interactable = false;
                Debug.Log("Application Quit()");
                Application.Quit();
                break;
            case PanelStates.Main:
                optionsButton.interactable = false;
                mainPanel.SetActive(false);
                loginPanel.SetActive(true);
                states = PanelStates.Login;
                PhotonNetwork.Disconnect();
                break;
            case PanelStates.ChangePassword:
                changePasswordPanel.SetActive(false);
                loginPanel.SetActive(true);
                states = PanelStates.Login;
                break;
            case PanelStates.Register:
                registerPanel.SetActive(false);
                loginPanel.SetActive(true);
                states = PanelStates.Login;
                break;
            case PanelStates.Options:
                optionsPanelOpened = false;
                optionsPanel.SetActive(false);
                mainPanel.SetActive(true);
                states = PanelStates.Main;
                break;
            case PanelStates.Message:
                messagePanel.SetActive(false);
                break;
            case PanelStates.Error:
                errorPanel.SetActive(false);
                break;
            case PanelStates.Leaderboards:
                leaderboardsPanel.SetActive(false);
                optionsPanel.SetActive(true);
                states = PanelStates.Options;
                break;
            default:
                states = PanelStates.Main;
                loginPanel.SetActive(true);
                break;
        }

    }


    private void Start()
    {
        OptionSettings.uiMode = OptionSettings.UIMode.darkMode;
        loginPanel.SetActive(true);
    }

    public void OpenRegisterPanel()
    {
        registerPanel.SetActive(true);
        states = PanelStates.Register;
    }

    public void OpenLeaderboardsPanel()
    {
        leaderboardsPanel.SetActive(true);
        states = PanelStates.Leaderboards;
    }

    public void OpenOptionsPanel()
    {
        if (optionsPanelOpened)
        {
            states = PanelStates.Main;
            optionsPanel.SetActive(false);
            optionsPanelOpened = false;
        }
        else
        {
            states = PanelStates.Options;
            optionsPanel.SetActive(true);
            optionsPanelOpened = true;
        }
    }

    public void OpenPasswordChangePanel()
    {
        states = PanelStates.ChangePassword;
        changePasswordPanel.SetActive(true);
    }
    
    public void Singleplayer()
    {
        UIManager.multiplayer = false;
        SceneManager.LoadScene("SingleplayerDebug");
    }

    public void Multiplayer()
    {
        UIManager.multiplayer = true;
        SceneManager.LoadScene("MultiplayerDebug");
    }

    public void CallRegister()
    {
        StartCoroutine(Register());
    }

    public void CallLogin()
    {
        StartCoroutine(Login());
    }

    public void CallPasswordChange()
    {
        StartCoroutine(PasswordChange());
    }




    public void OpenLoginPanel()
    {
        states = PanelStates.Login;
        loginPanel.SetActive(true);
    }

    IEnumerator Register()
    {
        WWWForm form = new WWWForm();
        string securityCode = DatabaseManager.SecurityCode();
        form.AddField("username",usernameRegisterInput.text);
        form.AddField("password", passwordRegisterInput.text);
        form.AddField("securitycode", securityCode);
        WWW www = new WWW("http://localhost/sqlconnect/register.php",form);
        yield return www;
        if(www.text == "0")
        {
            Debug.Log("User created successfully. Use this security code: " + securityCode + " to change your password!");
            Message("User created successfully. Use this security code: " + securityCode + " to change your password!");
            registerPanel.SetActive(false);
        } else
        {
            Debug.Log("User creation failed. Error #" + www.text);
            ErrorMessage("User creation failed: " + www.text);
        }
    }

    IEnumerator Login()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", usernameLoginInput.text);
        form.AddField("password", passwordLoginInput.text);
        WWW www = new WWW("http://localhost/sqlconnect/login.php",form);
        yield return www;
        if(www.text[0] == '0')
        {
            DatabaseManager.username = usernameLoginInput.text;
            DatabaseManager.score = int.Parse(www.text.Split('\t')[1]);
            //Debug
            if (DatabaseManager.LoggedIn)
            {
                debugTextMeshProUGUI.text = "Logged in as: " + DatabaseManager.username;
                loginPanel.SetActive(false);
                mainPanel.SetActive(true);
                optionsButton.interactable = true;
                states = PanelStates.Main;
            }
        } else
        {
            Debug.Log("User login failed. Error #" + www.text);
            ErrorMessage("User login failed: " + www.text);
        }
    }

    IEnumerator PasswordChange()
    {
        WWWForm form = new WWWForm();
        string newsecurityCode = DatabaseManager.SecurityCode();
        form.AddField("username", usernamePassChangeInput.text);
        form.AddField("password", passwordChangeInput.text);
        form.AddField("oldsecuritycode", securityCodeInput.text);
        form.AddField("newsecuritycode", newsecurityCode);
        WWW www = new WWW("http://localhost/sqlconnect/passwordchange.php",form);
        yield return www;
        if(www.text == "0")
        {
            Debug.Log("User's password changed successfully. Use this new security code: " + newsecurityCode + " to change your password!");
            Message("User's password changed successfully. Use this new security code: " + newsecurityCode + " to change your password!");
            registerPanel.SetActive(false);
        }
        else
        {
            Debug.Log("User password change failed. Error #" + www.text);
            ErrorMessage("User password change failed: " + www.text);
        }
    }

    public void ErrorMessage(string msg)
    {
        errorTextMeshProUGUI.text = msg;
        errorPanel.SetActive(true);
    }

    public void Message(string msg)
    {
        messageTextMeshProUGUI.text = msg;
        messagePanel.SetActive(true);
    }



    //remake this function so that it verifies multiple panels!
    public void VerifySignUpInputs()
    {
        signUpButton.interactable = (usernameRegisterInput.text.Length >= inputAllowance && passwordRegisterInput.text.Length >= inputAllowance);
    }
    public void VerifyLoginInputs()
    {
        loginButton.interactable = (usernameLoginInput.text.Length >= inputAllowance && passwordLoginInput.text.Length >= inputAllowance);
    }

    public void VerifyPasswordChangeInputs()
    {
        changePasswordButton.interactable = (securityCodeInput.text.Length == 6 &&
            usernamePassChangeInput.text.Length >= inputAllowance &&
            passwordChangeInput.text.Length >= inputAllowance &&
            repeatPasswordChangeInput.text.Length >= inputAllowance &&
            repeatPasswordChangeInput.text == passwordChangeInput.text);
    }



    //Button functions
    public void closeErrorButton()
    {
        errorPanel.SetActive(false);
    }
    public void closeMessageButton()
    {
        messagePanel.SetActive(false);
    }

    //dropdown options panel functionality
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


    public void ChangeUIMode()
    {
        Debug.Log("Changing UI Mode!");
        if(ui == UIStates.DarkMode)
        {
            //change to light mode:
            foreach(TextMeshProUGUI text in texts)
            {
                text.color = Color.black;
            }
            foreach(Button buttons in buttons)
            {
                buttons.image.sprite = lightButton;
            }
            foreach(Image panel in panels)
            {
                panel.sprite = lightPanel;
            }
            foreach(TMP_InputField inputfield in inputfields)
            {
                inputfield.image.sprite = lightInputField;
            }
            OptionSettings.uiMode = OptionSettings.UIMode.lightMode;
            ui = UIStates.LightMode;
            closeButton.image.sprite = lightExitButton;
            optionsButton.image.sprite = lightOptionsButton;
            camera.backgroundColor = Color.white;
            changeUIModeButtonText.text = "Dark Mode";
        } else if (ui == UIStates.LightMode)
        {
            //change to dark mode:
            foreach (TextMeshProUGUI text in texts)
            {
                text.color = Color.white;
            }
            foreach (Button buttons in buttons)
            {
                buttons.image.sprite = darkButton;
            }
            foreach (Image panel in panels)
            {
                panel.sprite = darkPanel;
            }
            foreach (TMP_InputField inputfield in inputfields)
            {
                inputfield.image.sprite = darkInputField;
            }
            ui = UIStates.DarkMode;
            OptionSettings.uiMode = OptionSettings.UIMode.darkMode;
            closeButton.image.sprite = darkExitButton;
            optionsButton.image.sprite = darkOptionsButton;
            camera.backgroundColor = darkModeColor;
            changeUIModeButtonText.text = "Light Mode";
        }

    }
}
