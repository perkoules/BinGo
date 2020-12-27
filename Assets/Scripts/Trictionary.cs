using System.Collections.Generic;

public class Trictionary : Dictionary<string, TeamNameRubbish>
{
    public void Add(string key, string teamname, int rubbishCollected)
    {
        TeamNameRubbish t;
        t.Value1 = teamname;
        t.Value2 = rubbishCollected;
        Add(key, t);
    }
}
