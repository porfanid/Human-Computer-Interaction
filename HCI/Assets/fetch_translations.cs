using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using SimpleJSON;




public class fetch_translations : MonoBehaviour
{

    [System.Serializable]
    public class MuseumObjectDataList
    {
        public MuseumObjectData[] MuseumObjects;
    }
    public string url = "https://us-central1-human-computer-int.cloudfunctions.net/getFirestoreData";

    void Start()
    {
        Debug.Log("Downloading data from URL: " + url);
        StartCoroutine(DownloadData(OnDataDownloaded));
    }

    void OnDataDownloaded(List<MuseumObject> museumObjects)
    {
        Debug.Log("Downloaded list:");
        Debug.Log("Number of MuseumObjects: " + museumObjects.Count);
        
        foreach (var museumObject in museumObjects)
        {
            Debug.Log("Year: " + museumObject.Year + ", Location: " + museumObject.Location);
            
            // Print Translations for the current MuseumObject
            Debug.Log("Translations:");
            Debug.Log(museumObject.Translations);
            foreach (var kvp in museumObject.Translations)
            {
                string language = kvp.Key;
                TranslationData translationData = kvp.Value;
                Debug.Log("- Language: " + language);
                Debug.Log("  - Artist: " + translationData.Artist);
                Debug.Log("  - Description: " + translationData.Description);
                Debug.Log("  - Name: " + translationData.Name);
            }
        }
    }

public IEnumerator DownloadData(System.Action<List<MuseumObject>> onDataDownloaded)
{
    using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
    {
        yield return webRequest.SendWebRequest();

        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Failed to download data: " + webRequest.error);
            yield break;
        }

        string jsonData = webRequest.downloadHandler.text;
        Debug.Log("JSON Data received: " + jsonData);

        // Parse JSON using Unity's JsonUtility
        List<MuseumObject> museumObjects = new List<MuseumObject>();
        Debug.Log("List is");
        Debug.Log(jsonData);
        MuseumObjectDataList dataContainer = JsonUtility.FromJson<MuseumObjectDataList>(jsonData);
        
        foreach (MuseumObjectData data in dataContainer.MuseumObjects)
        {
            MuseumObject museumObject = new MuseumObject();
            museumObject.Year = data.Year;
            museumObject.Location = data.Location;
            // museumObject.Translations = data.Translations;

            Debug.Log(data.Year);
            Debug.Log(data.Translations);
            /*foreach (var kvp in data.Translations)
            {
                string language = kvp.Key;
                // TranslationData translationData = kvp.Value;
                Debug.Log("- Language: " + language);
                Debug.Log("  - Artist: " + data.Translations[language]);
                //Debug.Log("  - Description: " + translationData.Description);
                //Debug.Log("  - Name: " + translationData.Name);
            }
            */
            museumObjects.Add(museumObject);
        }

        onDataDownloaded?.Invoke(museumObjects);
    }
}


    [System.Serializable]
    private class Serialization<T>
    {
        public List<T> Items;
    }
}


public static class JsonHelper
{
    // Deserialize JSON array into an array of objects
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    // Serialize an array of objects into JSON
    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    // Wrapper class to handle serialization of arrays
    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}
