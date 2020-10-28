using TMPro;
using UnityEngine;

[System.Serializable]
public class Tasks 
{
    private string task1;
    private string task2;
    private string task3;

    public string Task1
    {
        get { return task1; }
        set { task1 = value; }
    }
    public string Task2
    {
        get { return task2; }
        set { task2 = value; }
    }
    public string Task3
    {
        get { return task3; }
        set { task3 = value; }
    }
    public void Daily(TextMeshProUGUI t1, TextMeshProUGUI t2, TextMeshProUGUI t3)
    {
        t1.text = task1;
        t2.text = task2;
        t3.text = task3;
    }
}