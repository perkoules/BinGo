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
    public Image frames;
    public TextMeshProUGUI messageText;
    public Animator anim;
    public GameObject nextLevelAnimator;
    public RectTransform parent;
    public MusicController musicController;

    private PlayerDataSaver playerDataSaver;
    private GetCurrentLocation currentLocation;
    private ScanRubbish scanRubbish;
    private int progressLevel = 1;
    private int rubbishCollected = 0;
    private int wasteCollected = 0;
    private int recycleCollected = 0;
    private int coinsAvailable = 0;
    private int currentLevel = 0;
    private int distanceAcceptable = 5;
    private string rubbishScanned = "";
    private float timeLeft = 20;
    private bool pointerDown = false;
    private bool barcodeDetected = false;

    public bool isSfxOn = true;
    public bool isVibrationOn = true;

    public RubbishDataHandler dataHandler;
    private string place, district, region, country;

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
    }

    private void Awake()
    {
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
        if (fillerImage.fillAmount == 1 && CalculateDistance.Instance.MinDistance() <= distanceAcceptable)
        {
            frames.color = Color.white;
            StopCoroutine(rubbishCoroutine);
            StartCoroutine(ShowTextMessage());


            string typeOfRubbish = CalculateDistance.Instance.GetClosestBinData();
            if (!string.IsNullOrEmpty(typeOfRubbish))
            {                
                SetRubbishCollection(typeOfRubbish);
            }
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
        if (isVibrationOn)
        {
            Handheld.Vibrate();
        }
        if (isSfxOn)
        {
            MusicController.Instance.PlayRubbishCollectedSound();
        }
        messageText.text = "WELL DONE! You helped the environment!!!";
        fillerImage.fillAmount = 0;
        yield return new WaitForSeconds(2);
        messageText.text = "Please scan another rubbish!!!";
        barcodeDetected = false;
        timeLeft = 10f;
        anim.SetBool("fill", true);
    }

    public void QrScanFinished(string dataText)
    {
        if (!barcodeDetected)
        {
            if (isSfxOn)
            {
                MusicController.Instance.PlayBeepSound();
            }
            barcodeDetected = true;
            string comparer = "";
            comparer = rubbishScanned;
            rubbishScanned = dataText;

            if (rubbishScanned == comparer && !string.IsNullOrEmpty(rubbishScanned))
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

    private IEnumerator DataTextCooldown()
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

    public delegate void AdjustValues(int recycle, int waste, int coins, int level);
    public static event AdjustValues OnValuesAdjusted;
    public delegate void AdjustImage(int level);
    public static event AdjustImage OnImageAdjusted;

    public void SetRubbishCollection(string typeOfRubbish)
    {
        GetLocationDataOfRubbish();

        dataHandler.placeRubbishPair[place]++;
        dataHandler.districtRubbishPair[district]++;
        dataHandler.regionRubbishPair[region]++;
        dataHandler.countryRubbishPair[country]++;
        if (typeOfRubbish == "Waste")
        {
            wasteCollected++;
            coinsAvailable++;
            playerDataSaver.SetWasteCollected(wasteCollected);
        }
        else if (typeOfRubbish == "Recycle")
        {
            recycleCollected++;
            coinsAvailable += 2;
            playerDataSaver.SetRecycleCollected(recycleCollected);
        }
        achievementsController.wasteToUnlockCounter = wasteCollected;
        achievementsController.recycleToUnlockCounter = recycleCollected;
        rubbishCollected = wasteCollected + recycleCollected;
        playerDataSaver.SetRubbishCollected(rubbishCollected);
        playerDataSaver.SetCoinsAvailable(coinsAvailable);
        TaskChecker.Instance.CheckTaskDone();
        UpdatePlayerStats();
        StartCoroutine(achievementsController.CheckAchievementUnlockability());
        ProgressLevelCheck();
    }

    public void ProgressLevelCheck()
    {
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
            if (progressByRubbish.ElementAt(i - 1).Value <= rubbishCollected && rubbishCollected <= progressByRubbish.ElementAt(i).Value)
            {
                currentLevel = playerDataSaver.GetProgressLevel();
                progressLevel = progressByRubbish.ElementAt(i - 1).Key;
                playerDataSaver.SetProgressLevel(progressLevel);
                if (currentLevel != progressLevel)
                {
                    GameObject obj = Instantiate(nextLevelAnimator, parent);
                    Destroy(obj, 6f);
                    OnImageAdjusted(progressLevel);
                }
            }
        }
        OnValuesAdjusted?.Invoke(recycleCollected, wasteCollected, coinsAvailable, progressLevel);
    }

    public void UpdatePlayerStats()
    {
        if (dataHandler.districtRubbishPair.ContainsKey(""))                     //For places with no districts
        {
            dataHandler.districtRubbishPair.Add("NullDistricts", 0);
        }
        if (dataHandler.regionRubbishPair.ContainsKey(""))                       //For places with no regions
        {
            dataHandler.districtRubbishPair.Add("NullRegions", 0);
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
                cloudStatisticNamePlace = place + " isPlace",
                cloudStatisticNameDistrict = district + " isDistrict",
                cloudStatisticNameRegion = region + " isRegion",
                cloudStatisticNameCountry = country + " isCountry",
                cloudRubbishCollectedInPlace = dataHandler.placeRubbishPair[place],
                cloudRubbishCollectedInDistrict = dataHandler.districtRubbishPair[district],
                cloudRubbishCollectedInRegion = dataHandler.regionRubbishPair[region],
                cloudRubbishCollectedInCountry = dataHandler.countryRubbishPair[country],
                cloudCoinsAvailable = coinsAvailable
            },
            GeneratePlayStreamEvent = true
        },
        result =>
        {
            Debug.Log(result.FunctionResult);
        },
        error => Debug.Log(error.GenerateErrorReport()));
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
            if (!dataHandler.placeRubbishPair.ContainsKey(place))
            {
                dataHandler.placeRubbishPair.Add(place, 0);
            }
        }
        if (d >= 0)
        {
            district = myResult.features[d].text;
            if (!dataHandler.districtRubbishPair.ContainsKey(district))
            {
                dataHandler.districtRubbishPair.Add(district, 0);
            }
        }
        if (r >= 0)
        {
            region = myResult.features[r].text;
            if (!dataHandler.regionRubbishPair.ContainsKey(region))
            {
                dataHandler.regionRubbishPair.Add(region, 0);
            }
        }
        if (c >= 0)
        {
            country = myResult.features[c].text;
            if (!dataHandler.countryRubbishPair.ContainsKey(country))
            {
                dataHandler.countryRubbishPair.Add(country, 0);
            }
        }
    }

    #endregion PlayfabCommunications
}