using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Texts : MonoBehaviour
{
    public TextMeshProUGUI rec, was, nxtLvl, shieldAmount, attackAmount, teamname;
    public List<TextMeshProUGUI> rubbish;
    public List<TextMeshProUGUI> level;
    public List<TextMeshProUGUI> coins;
    public List<TextMeshProUGUI> vouchers;
    public List<TextMeshProUGUI> usernames;

    private void Awake()
    {
        PlayfabManager.OnValuesAdjusted += AdjustAllTexts;
        PlayfabManager.OnNamesAdjusted += AdjustNames;
        SetTeamname.OnNamesAdjusted += AdjustTeamName;
        CollectRubbish.OnValuesAdjusted += AdjustAllTexts;
        GiftCardEnabler.OnValuesAdjusted += AdjustCoins;
        GetReduction.OnValuesAdjusted += AdjustCoins;
        LineTrace.OnValuesAdjusted += AdjustCoinsByLogo;
    }

    private void AdjustTeamName(string team, bool on)
    {
        if (on)
        {
            teamname.text = team;
        }
        else
        {
            SetTeamname.OnNamesAdjusted -= AdjustTeamName;
        }
    }

    private void AdjustNames(string team, string user)
    {
        usernames.ForEach(t => t.text = user.Replace(user.First(), char.ToUpper(user.First())));
        teamname.text = team;
    }

    private void AdjustAllTexts(int recycle, int waste, int coi, int lev)
    {
        Debug.Log("Values adjusted as: " + recycle + " " + waste + " " + coi + " " + lev + " =======================================================================================");
        int rub = recycle + waste;
        rec.text = recycle.ToString();
        shieldAmount.text = recycle.ToString();
        was.text = waste.ToString();
        attackAmount.text = waste.ToString();
        nxtLvl.text = (lev + 1).ToString();
        rubbish.ForEach(t => t.text = rub.ToString());
        level.ForEach(t => t.text = lev.ToString());
        coins.ForEach(t => t.text = coi.ToString());
        float voucher = coi / 100.0f;
        vouchers.ForEach(t => t.text = voucher.ToString(("F2")) + " £");
    }
    private void AdjustCoins(int coi)
    {
        coins.ForEach(t => t.text = coi.ToString());
        float voucher = coi / 100.0f;
        vouchers.ForEach(t => t.text = voucher.ToString(("F2")) + " £");
    }
    private void AdjustCoinsByLogo(int coi)
    {
        LineTrace.OnValuesAdjusted -= AdjustCoinsByLogo;
        coins.ForEach(t => t.text = coi.ToString());
        float voucher = coi / 100.0f;
        vouchers.ForEach(t => t.text = voucher.ToString(("F2")) + " £");
    }
}