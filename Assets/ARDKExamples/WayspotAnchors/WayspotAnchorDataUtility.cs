// Copyright 2022 Niantic, Inc. All Rights Reserved.
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

using Niantic.ARDK.AR.WayspotAnchors;

using UnityEngine;

namespace Niantic.ARDKExamples.WayspotAnchors
{
  public static class WayspotAnchorDataUtility
  {
    private const string DataKey = "wayspot_anchor_payloads";

    public static void SaveLocalPayloads(WayspotAnchorPayload[] wayspotAnchorPayloads)
    {
      var wayspotAnchorsData = new WayspotAnchorsData();
      wayspotAnchorsData.Payloads = wayspotAnchorPayloads.Select(a => a.Serialize()).ToArray();
      string wayspotAnchorsJson = JsonUtility.ToJson(wayspotAnchorsData);
      string path = Path.Combine(Application.persistentDataPath, "AnchorJSON.json");
      File.WriteAllText(path, wayspotAnchorsJson);
            
      PlayerPrefs.SetString(DataKey, wayspotAnchorsJson);
    }

    public static WayspotAnchorPayload[] LoadLocalPayloads()
    {
            string path = Path.Combine(Application.persistentDataPath, "AnchorJSON.json");
      if(File.Exists(path))
      // if (PlayerPrefs.HasKey(DataKey))
      {
        var payloads = new List<WayspotAnchorPayload>();
        // var json = PlayerPrefs.GetString(DataKey);

        string jsonStr = File.ReadAllText(path);
        var wayspotAnchorsData = JsonUtility.FromJson<WayspotAnchorsData>(jsonStr);
        foreach (var wayspotAnchorPayload in wayspotAnchorsData.Payloads)
        {
          var payload = WayspotAnchorPayload.Deserialize(wayspotAnchorPayload);
          payloads.Add(payload);
        }

        return payloads.ToArray();
      }
      else
      {
        Debug.Log("No payloads were found to load.");
        return Array.Empty<WayspotAnchorPayload>();
      }
    }

    public static void ClearLocalPayloads()
    {
      if (PlayerPrefs.HasKey(DataKey))
      {
        PlayerPrefs.DeleteKey(DataKey);
      }
    }

    [Serializable]
    private class WayspotAnchorsData
    {
      /// The payloads to save via JsonUtility
      public string[] Payloads = Array.Empty<string>();
    }
  }
}
