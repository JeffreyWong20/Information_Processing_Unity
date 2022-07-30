using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class LoginMenu : MonoBehaviour
{

    public EventSystem system;
    public Selectable firstInput;
    public Button summitButton;
    public string userID, password;
    public fakeDataBase fakeDataBase;
    GameObject SystemReturnMessage;
    GameObject FPGAinput;
    inputfpga FPGAinputscript;
    public GameObject LoginMenu1;
    int previousInput;
    float t1 = 0;
    float t2 = 0;
    // Start is called before the first frame update
    void Start()
    {
        FPGAinput = GameObject.Find("InputFPGA");
        FPGAinputscript = FPGAinput.GetComponent<inputfpga>();

        SystemReturnMessage = GameObject.Find("SystemReturnMessage");
        Debug.Log("Guess");
        system = EventSystem.current;
        firstInput.Select();
        
        
    }

    // Update is called once per frame
    void Update()
    {
        //moving up
        if (LoginMenu1.activeSelf)
        {
            userID = FPGAinputscript.username;
            GameObject.Find("nameInputField").gameObject.GetComponent<InputField>().text = userID;
        }
        if (FPGAinputscript.moving_direction == 1)
        {
            t1 = Time.time;
            if (t1 - t2 > 1)
            {
                try
                {
                    Selectable previous = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp();
                    if (previous != null)
                    {
                        previous.Select();
                    }
                }
                catch
                {
                    Debug.Log("NoButton");
                }
                t2 = Time.time;
            }
            
        }
        //moving down
        else if (FPGAinputscript.moving_direction == 2)
        {
            t1 = Time.time;
            if (t1 - t2 > 1)
            {
                try
                {
                    Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
                    if (next != null)
                    {
                        next.Select();
                    }
                }
                catch
                {
                    Debug.Log("NoButton");
                }
                t2 = Time.time;
            }
        }
            
        
        //moving left
        else if (FPGAinputscript.moving_direction == 3)
        {
            t1 = Time.time;
            if (t1 - t2 > 1)
            {
                try
                {
                    Selectable left = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnLeft();
                    if (left != null)
                    {
                        left.Select();
                    }
                }
                catch
                {
                    Debug.Log("NoButton");
                }
                t2 = Time.time;
            }
        }
        //moving right
        else if (FPGAinputscript.moving_direction == 4)
        {
            t1 = Time.time;
            if (t1 - t2 > 1)
            {
                try
                {
                    Selectable right = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnRight();
                    if (right != null)
                    {
                        right.Select();
                    }
                }
                catch
                {
                    Debug.Log("NoButton");
                }
                t2 = Time.time;
            }
        }

        if (FPGAinputscript.bomb == 1)
        {
            t1 = Time.time;
            if (t1 - t2 > 1)
            {
                system.currentSelectedGameObject.GetComponent<Button>().onClick.Invoke(); //sos
                Debug.Log("Submit");
                t2 = Time.time;
            }
        }

    }
    public void Login()
    {
        Debug.Log("onSummit");
        
        //password = GameObject.Find("passwordInputField").gameObject.GetComponent<InputField>().text.ToString();
        //if (fakeDataBase.userVerification(userName, Convert.ToInt32(password)))
        if(fakeDataBase.userVerification_without_password(Convert.ToInt32(userID)))
        {
           
            //Debug.Log("fakeDataBase is null? " + (fakeDataBase.systemReturnMessage == null), gameObject);
            //Debug.Log("SystemReturnMessage is null? " + (SystemReturnMessage.GetComponent<Text>().text == null), gameObject);
            SystemReturnMessage.GetComponent<TMPro.TextMeshProUGUI>().text = fakeDataBase.systemReturnMessage;
            Debug.Log("onSummit" + userID);
            //GameObject.Find("/LoginMenu").SetActive(false);
            //GameObject.Find("/MainMenu").SetActive(true);
            

        }

        Debug.Log("fakeDataBase is null? " + (fakeDataBase == null), gameObject);

        SystemReturnMessage.GetComponent<TMPro.TextMeshProUGUI>().text = fakeDataBase.systemReturnMessage;



    }


    //public void SetPassword()
    //{
        
    //    userName = GameObject.Find("setnameInputField").gameObject.GetComponent<InputField>().text.ToString();
    //    string oldpasswordInputField = GameObject.Find("oldpasswordInputField").gameObject.GetComponent<InputField>().text.ToString();
    //    string newpasswordInputField = GameObject.Find("newpasswordInputField").gameObject.GetComponent<InputField>().text.ToString();
    //    if(fakeDataBase.ResetItem(userName, Convert.ToInt32(oldpasswordInputField), Convert.ToInt32(newpasswordInputField)))
    //    {
    //        //GameObject.Find("MainMenu").SetActive(true);
    //        GameObject.Find("LoginMenu").SetActive(true);
    //        GameObject.Find("ResetPasswordMenu").SetActive(false);
    //    }
    //    SystemReturnMessage.GetComponent<TMPro.TextMeshProUGUI>().text = fakeDataBase.systemReturnMessage;
    //}

    //public void Register()
    //{

    //    Debug.Log("onSummit");
    //    userName = GameObject.Find("nameInputField").gameObject.GetComponent<InputField>().text.ToString();
    //    password = GameObject.Find("passwordInputField").gameObject.GetComponent<InputField>().text.ToString();
    //    if (fakeDataBase.CreateItem(userName, Convert.ToInt32(password))) { }
        
    //    SystemReturnMessage.GetComponent<TMPro.TextMeshProUGUI>().text = fakeDataBase.systemReturnMessage;
    //}
}
