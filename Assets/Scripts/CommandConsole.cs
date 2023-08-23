using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;
using WEditor;
using static WInput;
using WEditor.Game.Player;
using WEditor.UI;
using TMPro;
using Unity.VisualScripting;

public class CommandConsole : MonoBehaviour, ICommandConsoleActions
{
    [SerializeField] Health health;
    [SerializeField] GunHandler gunHandler;
    [SerializeField] GameObject console;
    [SerializeField] TextMeshProUGUI helpText;
    [SerializeField] TMP_InputField inputField;
    private bool isEnable;
    public string commandLine { get; set; }
    private List<Tuple<string, string>> commandsList = new()
    {
        Tuple.Create(nameof(Hurt),"integer"),
        Tuple.Create(nameof(Heal),"integer"),
        Tuple.Create(nameof(Charge),""),
        Tuple.Create(nameof(God),"integer (0:yes or 1:false)"),
    };
    private void Start()
    {
        ConsoleInput.instance.EnableAndSetCallbacks(this);
    }
    void OnEnable()
    {
        commandsList.ForEach(command =>
        {
            helpText.text += command.Item1 + ": " + command.Item2 + "\n";
        });
    }
    void OnDisable()
    {
        helpText.text = "";
    }

    public void Reader()
    {
        var commandSplitted = HandleCommandLine();
        if (commandLine == null)
        {
            return;
        }
        string methodName = commandSplitted[0];
        print(methodName);
        MethodInfo method = this.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

        bool validMethod = commandsList.Exists(x => x.Item1 == methodName);
        if (!validMethod)
        {
            MessageHandler.instance.SetError("editor_command");
            return;
        }
        if (method.GetParameters().Length > 0)
        {
            method.Invoke(this, new object[] { int.Parse(commandSplitted[1]) });
        }
        else
        {
            method.Invoke(this, null);
        }
    }
    private string[] HandleCommandLine()
    {
        string[] split = new string[2];
        if (commandLine.Contains(" "))
        {
            split = commandLine.Split(" ");
            split[0] = CapitalizeAndLowerString(split[0]);
            return split;
        }
        else
        {

            split[0] = CapitalizeAndLowerString(commandLine);
            return split;
        }
    }
    private string CapitalizeAndLowerString(string methodName)
    {
        methodName = methodName.ToLower();
        methodName = methodName.FirstCharacterToUpper();
        return methodName;
    }
    private void Hurt(int amount)
    {
        health.Take(amount);
    }
    private void Heal(int amount)
    {
        health.Add(amount);
    }
    private void God(int value)
    {
        health.isImmortal = value == 0;
    }
    private void Gamemode(int amount)
    {

    }
    private void Charge()
    {
        gunHandler.currentGun.Add(999);
    }
    public void OnOpen(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (isEnable)
            {
                Cursor.lockState = CursorLockMode.Locked;
                console.SetActive(false);
                PlayerControllerInput.instance.Enable();
                GunInput.instance.Enable();
                isEnable = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                console.SetActive(true);
                PlayerControllerInput.instance.Disable();
                GunInput.instance.Disable();
                inputField.text = "";
                isEnable = true;
            }
        }
    }

    public void OnEnter(InputAction.CallbackContext context)
    {
        if (context.started && isEnable)
        {
            Reader();
        }
    }
}
