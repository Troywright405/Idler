using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleLogManager : MonoBehaviour
{
    // Reference to the UI text component where the log is displayed.
    public TMP_Text battleLogText;

    public static BattleLogManager Instance { get; private set; }
    
    // Use a queue to store the log lines.
    private Queue<string> battleLogQueue = new Queue<string>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Start the coroutine to remove a log line every second.
        StartCoroutine(RemoveLogLineEverySecond());
    }

    // Call this method to add a new log line.
    public void AddLogLine(string line)
    {
        battleLogQueue.Enqueue(line);
        UpdateLogText();
    }

    // This coroutine removes one log line every second.
    private IEnumerator RemoveLogLineEverySecond()
    {
        while (true)
        {
            if (battleLogQueue.Count > 0)
            {
                // Remove the oldest log line.
                //Debug.Log("Removing log line: " + battleLogQueue.Peek());
                battleLogQueue.Dequeue();
                UpdateLogText();
            }
            else
            {
                //Debug.Log("Battle log queue is empty.");
            }
            yield return new WaitForSeconds(5f);
        }
    }

    // Update the UI text based on the current queue contents.
    private void UpdateLogText()
    {
        battleLogText.text = string.Join("\n", battleLogQueue.ToArray());
    }
}

