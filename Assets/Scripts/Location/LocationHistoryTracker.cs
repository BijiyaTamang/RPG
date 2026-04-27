using System.Collections.Generic;
using UnityEngine;
public class LocationHistoryTracker : MonoBehaviour
{
    private HashSet<LocationSO> locationsVisited = new HashSet<LocationSO>();
    public List<LocationSO> allLocations; // assign all LocationSOs in Inspector

    public void RecordLocation(LocationSO locationSO)
    {
        locationsVisited.Add(locationSO);
        Debug.Log("Just visited " + locationSO.locationName);
    }

    public bool HasVisited(LocationSO locationSO)
    {
        return locationsVisited.Contains(locationSO);
    }

    public List<string> GetVisitedLocationNames()
    {
        var names = new List<string>();
        foreach (var location in locationsVisited)
            names.Add(location.locationName);
        return names;
    }

    public void LoadVisitedLocations(List<string> names)
    {
        locationsVisited.Clear();
        foreach (var location in allLocations)
            if (names.Contains(location.locationName))
                locationsVisited.Add(location);
    }
}