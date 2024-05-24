using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AuthUIManager : MonoBehaviour
{
    public TMP_InputField loginEmailInput;
    public TMP_InputField loginPasswordInput;
    public TMP_Text loginFeedbackText;
    public TMP_InputField signupEmailInput;
    public TMP_InputField signupPasswordInput;
    public TMP_Text signupFeedbackText;
    public TMP_InputField findPWEmailInput;
    public TMP_Text findPWFeedbackText;
    public GameObject createID;
    public GameObject FindPW;

    private AuthManager authManager;

    private void Start()
    {
        authManager = FindObjectOfType<AuthManager>();
        loginFeedbackText.SetText("");
    }

    public void ShowSignUPUI()
    {
        if (!createID.activeSelf)
        {
            createID.SetActive(true);
            signupFeedbackText.SetText("");
        }
    }

    public void QuitSignUPUI()
    {
        if (createID.activeSelf)
        {
            createID.SetActive(false);
        }
    }

    public void ShowFindPWUI()
    {
        if (!FindPW.activeSelf)
        {
            FindPW.SetActive(true);
            findPWFeedbackText.SetText("");
        }
    }

    public void QuitFindPWUI()
    {
        if (FindPW.activeSelf)
        {
            FindPW.SetActive(false);
        }
    }

    public void OnSignUpButtonClicked()
    {
        string email = signupEmailInput.text;
        string password = signupPasswordInput.text;
        authManager.SignUp(email, password, OnSignUpSuccess, OnSignUpFail);
    }

    private void OnSignUpSuccess(string email)
    {
        signupFeedbackText.SetText("ID생성 성공! " + email);
    }

    private void OnSignUpFail(string error)
    {
        signupFeedbackText.SetText("ID생성 실패!: " + error);
    }

    public void OnLoginButtonClicked()
    {
        string email = loginEmailInput.text;
        string password = loginPasswordInput.text;
        authManager.LogIn(email, password, OnLoginSuccess, OnLoginFail);
    }

    private void OnLoginSuccess(string email)
    {
        loginFeedbackText.SetText("로그인 성공! " + email);
    }

    private void OnLoginFail(string error)
    {
        loginFeedbackText.SetText("로그인 실패! " + error);
    }

    public void OnResetPasswordButtonClicked()
    {
        string email = findPWEmailInput.text;
        authManager.ResetPassword(email, OnResetPasswordSuccess, OnResetPasswordFail);
    }

    private void OnResetPasswordSuccess(string message)
    {
        findPWFeedbackText.SetText(message);
    }

    private void OnResetPasswordFail(string error)
    {
        findPWFeedbackText.SetText(error);
    }
}
