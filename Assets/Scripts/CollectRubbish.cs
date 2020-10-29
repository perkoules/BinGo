using Mapbox.Geocoding;
using Mapbox.Unity.Location;
using Mapbox.Utils;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerDataSaver), typeof(Image), typeof(ScanRubbish))]
public class CollectRubbish : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public static CollectRubbish Instance { get; private set; }

    public AchievementsController achievementsController;
    public PlayfabManager playfabManager;
    public DeviceLocationProvider locationProvider;
    public CalculateDistance calculateDistance;
    public Image frames;
    public TextMeshProUGUI messageText;
    public PlayerInfo playerInfo;
    public Animator anim;
    public GameObject nextLevelAnimator;
    public RectTransform parent;

    private PlayerDataSaver playerDataSaver;
    private GetCurrentLocation currentLocation;
    private ScanRubbish scanRubbish;
    private int progressLevel = 1;
    private int rubbishCollected = 0;
    private int wasteCollected = 0;
    private int recycleCollected = 0;
    private int coinsAvailable = 0;
    private int rubbishInPlace = 0;
    private int rubbishInDistrict = 0;
    private int rubbishInRegion = 0;
    private int rubbishInCountry = 0;
    private int currentLevel = 0;
    private int distanceAcceptable = 100;
    private string place, district, region, country;
    private string rubbishScanned = "";
    private bool pointerDown = false;
    private bool barcodeDetected = false;
    private float timeLeft = 20;

    [HideInInspector] public Image fillerImage;    
    private IEnumerator rubbishCoroutine;

    private void OnEnable()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        playerInfo = playfabManager.playerInfo;
        rubbishInPlace = playerInfo.RubbishInPlace;
        rubbishInDistrict = playerInfo.RubbishInDistrict;
        rubbishInRegion = playerInfo.RubbishInRegion;
        rubbishInCountry = playerInfo.RubbishInCountry;
    }

    private void Awake()
    {
        if(calculateDistance == null)
        {
            calculateDistance = FindObjectOfType<CalculateDistance>();
        }
        scanRubbish = GetComponent<ScanRubbish>();
        fillerImage = GetComponent<Image>();
        playerDataSaver = GetComponent<PlayerDataSaver>();
    }

    private void Start()
    {
        wasteCollected = playerDataSaver.GetWasteCollected();
        recycleCollected = playerDataSaver.GetRecycleCollected();
        coinsAvailable = playerDataSaver.GetCoinsAvailable();
        progressLevel = playerDataSaver.GetProgressLevel();
        rubbishCoroutine = RubbishCooldown();
    }

    private void Update()
    {
        if (pointerDown && frames.color == Color.green)
        {
            if (anim.GetBool("fill"))
            {
                anim.SetBool("fill", false);
                fillerImage.fillAmount = 0;
            }
            fillerImage.fillAmount += Time.deltaTime;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        pointerDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pointerDown = false;
        if (fillerImage.fillAmount == 1 && calculateDistance.distances[calculateDistance.minIndex] <= distanceAcceptable)
        {
            frames.color = Color.white;
            StopCoroutine(rubbishCoroutine);
            StartCoroutine(ShowTextMessage());
            string typeOfRubbish = "";
            if (calculateDistance.bins[calculateDistance.minIndex].transform.parent.name.Contains("Waste"))
            {
                typeOfRubbish = "waste";
            }
            else if (calculateDistance.bins[calculateDistance.minIndex].transform.parent.name.Contains("Recycle"))
            {
                typeOfRubbish = "recycle";
            }
            SetRubbishCollection(typeOfRubbish);
            anim.SetBool("fill", false);
        }
        else
        {
            messageText.text = "After scanning, hold the button until full.";
        }        
        fillerImage.fillAmount = 0;
    }

    private IEnumerator ShowTextMessage()
    {
        messageText.text = "WELL DONE! You helped the environment!!!";
        fillerImage.fillAmount = 0;
        yield return new WaitForSeconds(2);
        messageText.text = "Please scan another rubbish!!!";
        barcodeDetected = false;
        timeLeft = 10f;
        anim.SetBool("fill", true);
        scanRubbish.Play();
    }

    public void QrScanFinished(string dataText)
    {
        if (!barcodeDetected)
        {
            barcodeDetected = true;
            string comparer = "";
            comparer = rubbishScanned;
            rubbishScanned = dataText;
            
            if(rubbishScanned == comparer && ! string.IsNullOrEmpty(rubbishScanned))
            {
                messageText.text = "Please scan a different rubbish!!!";
                barcodeDetected = false;
                scanRubbish.Play();
                StartCoroutine(DataTextCooldown());
            }
            else
            {
                frames.color = Color.green;
                StartCoroutine(rubbishCoroutine);
                if (scanRubbish.scanLineObj != null)
                {
                    scanRubbish.scanLineObj.SetActive(false);
                }
            }            
        }
    }

    IEnumerator DataTextCooldown()
    {
        yield return new WaitForSeconds(10f);        
        rubbishScanned = "";
    }

    private IEnumerator RubbishCooldown()
    {
        timeLeft = 10f;
        anim.SetBool("fill", true);
        while (timeLeft > 0)
        {
            timeLeft--;
            yield return new WaitForSeconds(1.0f);
            messageText.text = "Throw it in the bin. Time Left: " + Convert.ToInt32(timeLeft).ToString() + "s";
        }
        frames.color = Color.white;
        barcodeDetected = false;
        anim.SetBool("fill", false);
        timeLeft = 10f;
        messageText.text = "Please rescan the rubbish!!!";
        scanRubbish.Play();
    }

    #region PlayfabCommunications

    public void SetRubbishCollection(string typeOfRubbish)
    {
        GetLocationDataOfRubbish();

        if (typeOfRubbish == "waste")
        {
            wasteCollected++;
            rubbishInPlace++;
            rubbishInDistrict++;
            rubbishInRegion++;
            rubbishInCountry++;
            coinsAvailable++;
            playerDataSaver.SetWasteCollected(wasteCollected);
        }
        else if (typeOfRubbish == "recycle")
        {
            recycleCollected++;
            rubbishInPlace++;
            rubbishInDistrict++;
            rubbishInRegion++;
            rubbishInCountry++;
            coinsAvailable += 2;
            playerDataSaver.SetRecycleCollected(recycleCollected);
        }
        achievementsController.wasteToUnlockCounter = wasteCollected;
        achievementsController.recycleToUnlockCounter = recycleCollected;
        rubbishCollected = wasteCollected + recycleCollected;
        TaskChecker.Instance.CheckTaskDone();
        ProgressLevelCheck();
        UpdatePlayerStats();
        StartCoroutine(achievementsController.CheckAchievementUnlockability());
        UpdatePlayerInfo();
    }

    private void UpdatePlayerInfo()
    {
        playerInfo = new PlayerInfo
        {
            PlayerUsername = playerDataSaver.GetUsername(),
            PlayerPassword = playerDataSaver.GetPassword(),
            PlayerEmail = playerDataSaver.GetEmail(),
            PlayerTeamName = playerDataSaver.GetTeamname(),
            PlayerWaste = playerDataSaver.GetWasteCollected(),
            PlayerRecycle = playerDataSaver.GetRecycleCollected(),
            PlayerRubbish = playerDataSaver.GetRubbishCollected(),
            PlayerCoins = playerDataSaver.GetCoinsAvailable(),
            PlayerCurrentLevel = playerDataSaver.GetProgressLevel(),
            RubbishInPlace = rubbishInPlace,
            RubbishInDistrict = rubbishInDistrict,
            RubbishInRegion = rubbishInRegion,
            RubbishInCountry = rubbishInCountry,
            RubbishPlace = place,
            RubbishDistrict = district,
            RubbishRegion = region,
            RubbishCountry = country
        };
    }

    public void ProgressLevelCheck()
    {
        int allRubbish = playerDataSaver.GetWasteCollected() + playerDataSaver.GetRecycleCollected();
        playerDataSaver.SetRubbishCollected(allRubbish);
        Dictionary<int, int> progressByRubbish = new Dictionary<int, int>()
        {
            {1, 20},
            {2, 50},
            {3, 100},
            {4, 200},
            {5, 300},
            {6, 500},
            {7, 700},
            {8, 1000},
            {9, 1250},
            {10, 1500},
            {11, 1750},
            {12, 2000},
            {13, 2500},
            {14, 3000},
            {15, 5000}
        };
        for (int i = 1; i < progressByRubbish.Count; i++)
        {
            if (progressByRubbish.ElementAt(i - 1).Value <= allRubbish && allRubbish <= progressByRubbish.ElementAt(i).Value)
            {
                currentLevel = playerDataSaver.GetProgressLevel();
                progressLevel = progressByRubbish.ElementAt(i - 1).Key;
                playerDataSaver.SetProgressLevel(progressLevel);
                if(currentLevel != progressLevel)
                {
                    GameObject obj = Instantiate(nextLevelAnimator, parent);
                    Destroy(obj, 4f);
                    FindObjectOfType<PlayfabManager>().ReInitialize();
                }
            }
        }        
    }

    public void UpdatePlayerStats()
    {
        if (playerInfo.RubbishDistrict == null)                     //For places with no districts
        {
            playerInfo.RubbishDistrict = "NullDistricts";
        }
        if (playerInfo.RubbishRegion == null)                       //For places with no regions
        {
            playerInfo.RubbishRegion = "NullRegions";
        }
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "UpdatePlayerStats",
            FunctionParameter = new
            {
                cloudProgressLevel = progressLevel,
                cloudWasteCollected = wasteCollected,
                cloudRubbishCollected = rubbishCollected,
                cloudRecycleCollected = recycleCollected,
                cloudStatisticNamePlace = playerInfo.RubbishPlace + " isPlace",
                cloudStatisticNameDistrict = playerInfo.RubbishDistrict + " isDistrict",
                cloudStatisticNameRegion = playerInfo.RubbishRegion + " isRegion",
                cloudStatisticNameCountry = playerInfo.RubbishCountry + " isCountry",
                cloudRubbishCollectedInPlace = rubbishInPlace,
                cloudRubbishCollectedInDistrict = rubbishInDistrict,
                cloudRubbishCollectedInRegion = rubbishInRegion,
                cloudRubbishCollectedInCountry = rubbishInCountry,
                cloudCoinsAvailable = coinsAvailable
            },
            GeneratePlayStreamEvent = true,
        },
        result => GetPlayerStats(),
        error => Debug.Log(error.GenerateErrorReport()));
    }

    public void GetPlayerStats()
    {
        PlayFabClientAPI.GetPlayerStatistics(
            new GetPlayerStatisticsRequest(),
            result =>
            {
                foreach (var eachStat in result.Statistics)
                {
                    switch (eachStat.StatisticName)
                    {
                        case "ProgressLevel":
                            progressLevel = eachStat.Value;
                            playerInfo.PlayerCurrentLevel = progressLevel;
                            playerDataSaver.SetProgressLevel(progressLevel);
                            break;

                        case "WasteCollected":
                            wasteCollected = eachStat.Value;
                            playerInfo.PlayerWaste = wasteCollected;
                            playerDataSaver.SetWasteCollected(wasteCollected);
                            break;

                        case "RecycleCollected":
                            recycleCollected = eachStat.Value;
                            playerInfo.PlayerRecycle = recycleCollected;
                            playerDataSaver.SetRecycleCollected(recycleCollected);
                            break;

                        case "RubbishCollected":
                            rubbishCollected = eachStat.Value;
                            playerInfo.PlayerRubbish = rubbishCollected;
                            playerDataSaver.SetRubbishCollected(rubbishCollected);
                            break;

                        case "CoinsAvailable":
                            coinsAvailable = eachStat.Value;
                            playerInfo.PlayerCoins = coinsAvailable;
                            playerDataSaver.SetCoinsAvailable(coinsAvailable);
                            break;

                        default:
                            break;
                    }
                    if (playerInfo.RubbishPlace != null)
                    {
                        if (eachStat.StatisticName == (playerInfo.RubbishPlace + " isPlace"))
                        {
                            rubbishInPlace = eachStat.Value;
                            playerInfo.RubbishInPlace = eachStat.Value;
                        }
                        else if (eachStat.StatisticName == playerInfo.RubbishDistrict + " isDistrict")
                        {
                            rubbishInDistrict = eachStat.Value;
                            playerInfo.RubbishInDistrict = eachStat.Value;
                        }
                        else if (eachStat.StatisticName == playerInfo.RubbishRegion + " isRegion")
                        {
                            rubbishInRegion = eachStat.Value;
                            playerInfo.RubbishInRegion = eachStat.Value;
                        }
                        else if (eachStat.StatisticName == (playerInfo.RubbishCountry) + " isCountry")
                        {
                            rubbishInCountry = eachStat.Value;
                            playerInfo.RubbishInCountry = eachStat.Value;
                        }
                    }
                    else
                    {
                        GetLocationDataOfRubbish();
                        GetPlayerStats();
                    }
                }
            }, error => Debug.LogError(error.GenerateErrorReport()));
    }

    public void GetLocationDataOfRubbish()
    {
        Vector2d latlon = locationProvider.CurrentLocation.LatitudeLongitude;
        currentLocation = new GetCurrentLocation(latlon)
        {
            Types = new string[] { "country", "region", "district", "place" }       //What features to focus on
        };

        string locationUrl = currentLocation.GetUrl();                              //Get Api location url
        var jsonLocationData = new WebClient().DownloadString(locationUrl);         //Get json results from url

        MyResult myResult = JsonUtility.FromJson<MyResult>(jsonLocationData);       //Example of results:
        int p = myResult.features.FindIndex(f => f.id.Contains("place"));           //[0] = Bishop Auckland
        int d = myResult.features.FindIndex(f => f.id.Contains("district"));        //[1] = Durham
        int r = myResult.features.FindIndex(f => f.id.Contains("region"));          //[2] = England
        int c = myResult.features.FindIndex(f => f.id.Contains("country"));         //[3] = United Kingdom
        if (p >= 0)
        {
            place = myResult.features[p].text;
        }
        if (d >= 0)
        {
            district = myResult.features[d].text;
        }
        if (r >= 0)
        {
            region = myResult.features[r].text;
        }
        if (c >= 0)
        {
            country = myResult.features[c].text;
        }
    }

    #endregion PlayfabCommunications
}