using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "Tasks", menuName = "TaskScriptableObject", order = 1)]
public class Tasks : ScriptableObject
{
    public string task1;
    public string task2;
    public string task3;

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
    public void SetName(GameObject obj1, GameObject obj2, GameObject obj3)
    {
        obj1.name = task1;
        obj2.name = task2;
        obj3.name = task3;
    }
}