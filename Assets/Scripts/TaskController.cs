using UnityEngine;
[System.Serializable]
public class TaskController
{
    public UnlockRubbish unlockRubbish;
    public UnlockRecycle unlockRecycle;
    public UnlockCity unlockCity;
    public UnlockCountry unlockCountry;
    public UnlockMountain unlockMountain;
    public UnlockSea unlockSea;
    public UnlockContinents unlockContinents;
    public UnlockStates unlockStates;
    public UnlockChampion unlockChampion;

}

[System.Serializable]
public class UnlockRubbish
{
    public bool _1;
    public bool _10;
    public bool _50;
    public bool _100;
    public bool _500;
    public bool _1000;
    public bool _5000;
}
[System.Serializable]
public class UnlockRecycle
{
    public bool _1;
    public bool _10;
    public bool _50;
    public bool _100;
    public bool _500;
    public bool _1000;
    public bool _5000;
}
[System.Serializable]
public class UnlockCity
{
    public bool _1;
    public bool _10;
    public bool _50;
    public bool _100;
    public bool _500;
    public bool _1000;
    public bool _5000;
}
[System.Serializable]
public class UnlockCountry
{
    public bool _1;
    public bool _5;
    public bool _10;
    public bool _50;
    public bool _100;
}
[System.Serializable]
public class UnlockMountain
{
    public bool _1;
    public bool _5;
    public bool _10;
    public bool _50;
    public bool _100;
    public bool _500;
}
[System.Serializable]
public class UnlockSea
{
    public bool _1;
    public bool _5;
    public bool _10;
    public bool _50;
    public bool _100;
    public bool _500;
}
[System.Serializable]
public class UnlockContinents
{
    public bool africa;
    public bool asia;
    public bool australia;
    public bool europe;
    public bool northAmerica;
    public bool southAmerica;
}
[System.Serializable]
public class UnlockStates
{
    public bool _1;
    public bool _5;
    public bool _10;
}
[System.Serializable]
public class UnlockChampion
{
    public bool firstLocal;
    public bool firstTeam;
    public bool firstWorld;
}