using Mapbox.Unity.Location;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetBinLocation : MonoBehaviour
{

    private AbstractLocationProvider _locationProvider = null;

    void Start()
    {
        if (null == _locationProvider)
        {
            _locationProvider = LocationProviderFactory.Instance.DefaultLocationProvider as AbstractLocationProvider;
        }
        Location currLoc = _locationProvider.CurrentLocation;
        Debug.Log(currLoc.LatitudeLongitude );
    }

}
