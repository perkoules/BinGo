[System.Serializable]
public class TeammateInfo
{
    public string username;
    public string avatar;
    public string country;
    public int level;

    public string Username
    {
        get { return username; }
        set { username = value; }
    }
    public string Avatar
    {
        get { return avatar; }
        set { avatar = value; }
    }
    public string Country
    {
        get { return country; }
        set { country = value; }
    }
    public int Level
    {
        get { return level; }
        set { level = value; }
    }
}