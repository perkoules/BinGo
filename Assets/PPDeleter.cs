using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PPDeleter : MonoBehaviour
{
    public void DeletePref(string which)
    {
        PlayerPrefs.DeleteKey(which);
    }
}
