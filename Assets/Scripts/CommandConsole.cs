using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;
using WEditor;
using static WInput;
using WEditor.Game.Player;
using WEditor.Game.Player.Guns;
using WEditor.UI;
using TMPro;
public class CommandConsole : MonoBehaviour, ICommandConsoleActions
{
    [SerializeField] Health health;
    [SerializeField] GunHandler gunHandler;
    [SerializeField] GameObject console;
    [SerializeField] TextMeshProUGUI helpText;
    [SerializeField] TMP_InputField inputField;
    private bool isEnable;
    public string commandLine { get; set; }
    private List<Tuple<string, string>> commandsList = new List<Tuple<string, string>>()
    {
        Tuple.Create(nameof(HurtPlayer),"numberParam"),
        Tuple.Create(nameof(GiveHealthPlayer),"numberParam"),
        Tuple.Create(nameof(FullAmmo),"numberParam"),
        Tuple.Create(nameof(God),"numberParam (0 or 1)"),
    };
    private void Start()
    {
        ConsoleInput.instance.EnableAndSetCallbacks(this);
    }
    public void Reader()
    {
        var commandSplitted = HandleCommandLine();
        if (commandLine == null)
        {
            return;
        }

        MethodInfo method = this.GetType().GetMethod(commandSplitted[0], BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

        if (method == null)
        {
            TextMessageHandler.instance.SetError("cc_ll");
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
            split = commandLine.Split(' ');
            return split;
        }
        else
        {
            split[0] = commandLine;
            return split;
        }
    }
    private void HurtPlayer(int amount)
    {
        health.Take(amount);
    }
    private void GiveHealthPlayer(int amount)
    {
        health.Add(amount);
    }
    private void Help()
    {
        commandsList.ForEach(command =>
        {
            helpText.text += command.Item1 + ": " + command.Item2 + "<br>";
        });
        Invoke(nameof(HideHelpGuide), 5);
    }
    private void God(int value)
    {
        health.isImmortal = value == 0 ? true : false;
    }
    private void Gamemode(int amount)
    {

    }
    private void FullAmmo()
    {
        gunHandler.currentGun.Add(999);
    }
    private void HideHelpGuide()
    {
        helpText.text = "";
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
