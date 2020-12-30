using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldObject : MonoBehaviour
{
    public delegate void ButtonReleased();
    public static event ButtonReleased OnButtonReleased;

    private void OnDestroy()
    {
        OnButtonReleased?.Invoke();
    }
}
