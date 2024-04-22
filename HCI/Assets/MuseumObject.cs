using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

[System.Serializable]
public class MuseumObject
{
    public int Year;
    public Dictionary<string, TranslationData> Translations; // Dictionary for translations
    public string Location;

    // Method to sort MuseumObjects by year
    public static int CompareByYear(MuseumObject a, MuseumObject b)
    {
        return a.Year.CompareTo(b.Year);
    }

    // Method to check equality between two MuseumObjects
    public bool Equals(MuseumObject other)
    {
        if (other == null)
            return false;

        // Check if the dictionaries have the same keys and values
        if (Translations == null || other.Translations == null || !Translations.DictionaryEqual(other.Translations))
            return false;

        return Year == other.Year &&
            Location == other.Location;
    }
}

[System.Serializable]
public class MuseumObjectData
{
    public int Year;
    public string Location;
    public string Translations;
}

[System.Serializable]
public class TranslationData
{
    public string Artist;
    public string Description;
    public string Name;


}

// Extension method to check equality between dictionaries
public static class DictionaryExtensions
{
    public static bool DictionaryEqual<TKey, TValue>(this Dictionary<TKey, TValue> dict1, Dictionary<TKey, TValue> dict2)
    {
        if (dict1 == dict2) return true;
        if (dict1 == null || dict2 == null || dict1.Count != dict2.Count) return false;

        foreach (var pair in dict1)
        {
            if (!dict2.TryGetValue(pair.Key, out TValue value) || !Equals(pair.Value, value))
                return false;
        }

        return true;
    }
}