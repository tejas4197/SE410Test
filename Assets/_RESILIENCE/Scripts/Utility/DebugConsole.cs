/* DebugConsole
 * 
 * For checking logs in build because fuck Unity
 * */

using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

using Sirenix.OdinInspector;

using System.IO;
using System.Collections;

public class DebugConsole : MonoBehaviour
{
    #region VARIABLES

    [SerializeField, Tooltip("GUI Object used to display text")]
    private GameObject DebugGui = null;

    [MinValue(1)]
    [SerializeField, Tooltip("Maximum number of messages stored in-game")]
    private  int maxMessages = 2000;                  

    private ArrayList messages = new ArrayList();

    [HorizontalGroup("Vis", 100), ReadOnly]
    [SerializeField, Tooltip("Whether Debug GUI is visible")]
    private bool isVisible = true;

    private static InputField inputText;
    
    private static DebugConsole instance = null;

    #endregion VARIABLES

    #region GETTERS_SETTERS

    /// <summary>
    /// Accessor for isVisible
    /// </summary>
    public bool GetIsVisible()
    {
        return GetInstance().isVisible;
    }
    
    /// <summary>
    /// Mutator for isVisible
    /// </summary>
    /// <param name="value">New value of isVisible</param>
    private void SetIsVisible(bool value)
    {
        isVisible = value;
        if (value == true)
        {
            GetInstance().Display();
        }
        else if (value == false)
        {
            GetInstance().ClearScreen();
        }
    }

    /// <summary>
    /// Accessor for instance, allows the instance of this script to be called without a direct connection
    /// </summary>
    public static DebugConsole GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType(typeof(DebugConsole)) as DebugConsole;
            if (instance == null)
            {
                // Load prefab which contains everything needed minus the script
                GameObject prefab = Resources.Load("Prefabs/UtilityPrefabs/DebugConsoleChild") as GameObject;
                prefab = Instantiate(prefab);
                GameObject console = new GameObject();
                // Add Script to console object
                console.AddComponent<DebugConsole>();
                // Set prefav as child of console
                prefab.transform.parent = console.transform;
                console.name = "DebugConsoleController";
                // Because it starts disabled otherwise?
                console.GetComponent<DebugConsole>().enabled = true;
                // Update instance
                instance = FindObjectOfType(typeof(DebugConsole)) as DebugConsole;
                inputText = GameObject.FindGameObjectWithTag("DebugConsoleInput").GetComponent<InputField>();
            }
        }
        return instance;
    }

    #endregion GETTERS_SETTERS

    #region EVENTS

    /// <summary>
    /// The delegate for subscribing to the SubmitCommand event
    /// </summary>
    public delegate void SubmitEventHandler(string command);
    /// <summary>
    /// Event invoked whenever a user enters a command in the console
    /// </summary>
    public event SubmitEventHandler SubmitCommand;

    #endregion EVENTS

    #region MONOBEHAVIOR

    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        InitGuis();
    }

    // To always receive Unity's logs
    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
        GetInstance().SubmitCommand += ProcessCommand;
    }
    void OnDisable()
    {
        Application.logMessageReceived += HandleLog;
        GetInstance().SubmitCommand -= ProcessCommand;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            ToggleDisplay();
        }

        if (Input.GetKeyDown(KeyCode.F12))
        {
            ExportLogs();
        }

        if (GetIsVisible() && inputText.text != "" && Input.GetKey(KeyCode.Return))
        {
            SubmitCommand?.Invoke(inputText.text);
            inputText.text = "";
        }

    }

    #endregion MONOBEHAVIOR

    #region PUBLIC_METHODS

    /// <summary>
    /// Toggles display of Debug GUI
    /// </summary>
    [MenuItem("Tools/DebugConsole/Toggle Console Visibility")]
    [Button("Toggle"), HorizontalGroup("Vis")]
    public static void ToggleDisplay()
    {
        GetInstance().SetIsVisible(!GetInstance().GetIsVisible());
        GetInstance().DebugGui.SetActive(!GetInstance().DebugGui.activeInHierarchy);
    }

    /// <summary>
    /// Clears entire debug log
    /// </summary>
    public static void Clear()
    {
        GetInstance().ClearMessages();
    }

    /// <summary>
    /// Displays debug logs
    /// </summary>
    public static void DisplayConsole()
    {
        GetInstance().Display();
    }

    /// <summary>
    /// A function that will do some basics to process commands.  Currently does nothing
    /// </summary>
    public void ProcessCommand(string command)
    {
        if (command.ToLower() == "export" || command.ToLower() == "save")
            ExportLogs();
    }

    #endregion PUBLIC_METHODS

    #region PRIVATE_METHODS

    /// <summary>
    /// Handles adding log to the message array
    /// </summary>
    /// <param name="logString">Message to be logged</param>
    /// <param name="stackTrace">Accompanying stack trace, for errors and exceptions</param>
    /// <param name="type">Type of item to be logged, as the LogType enum</param>
    void HandleLog(string logString, string stackTrace, LogType type)
    {
        DebugConsole.GetInstance().AddMessage(type.ToString() + ": " + logString);
        if (type == LogType.Exception || type == LogType.Error)
        {
            instance.AddMessage("Stack Trace: " + stackTrace);
        }
    }
    
    /// <summary>
    /// Initializes GUI items as disabled
    /// </summary>
    void InitGuis()
    {
        DebugGui = instance.GetComponentInChildren<Canvas>().gameObject;
        instance.SetIsVisible(false);
        DebugGui.SetActive(false);

    }

    /// <summary>
    /// Adds a plaintext message to the logs
    /// </summary>
    /// <param name="message">Message to be added</param>
    void AddMessage(string message)
    {
        messages.Add(message);
        Display();
    }

    /// <summary>
    /// Deletes all messages
    /// </summary>
    void ClearMessages()
    {
        messages.Clear();
        ClearScreen();
    }

    /// <summary>
    /// Clears GUI text field, but does NOT delete messages stored
    /// </summary>
    void ClearScreen()
    {
        if (DebugGui.GetComponentInChildren<Text>() != null)
            DebugGui.GetComponentInChildren<Text>().text = "";
    }

    /// <summary>
    /// Limits the maximum number of messages stored in-game
    /// </summary>
    void Prune()
    {
        int diff;
        if (messages.Count > maxMessages)
        {
            if (messages.Count <= 0)
            {
                diff = 0;
            }
            else
            {
                diff = messages.Count - maxMessages;
            }
            messages.RemoveRange(0, (int)diff);
        }

    }

    /// <summary>
    /// Displays messages in GUI text field
    /// </summary>
    public void Display()
    {
        if (isVisible == true && DebugGui != null)
        {
            if (messages.Count > maxMessages)
            {
                Prune();
            }

            // Finish displaying 
            int x = 0;
            string temp = "";
            while (x < messages.Count)
            {
                // Store all the messages in a string prior to setting it, to prevent any issues
                temp += "\n" + messages[x];
                x += 1;
            }
            DebugGui.GetComponentInChildren<Text>().text = temp;
        }
    }

    /// <summary>
    /// Exports the logs to local disk for upload.  Currently only appends to the file of the date, not replaces it
    /// </summary>
    [MenuItem("Tools/DebugConsole/Export Logs")]
    [Button("Export Logs")]
    private static void ExportLogs()
    {
        string date = System.DateTime.Now.ToString("yyyy-MM-dd");
        string datetime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        string path = "Logs/" + date + ".txt";

        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine("Exported Logs");
        writer.WriteLine(datetime + "\n\n");
        foreach (string message in GetInstance().messages)
        {
            writer.WriteLine(message);
        }

        writer.Close();
        Debug.Log("Export Successful");
    }

    #endregion PRIVATE_METHODS
}// End DebugConsole Class
