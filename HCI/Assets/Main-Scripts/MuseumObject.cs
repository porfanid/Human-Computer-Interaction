using Firebase.Firestore;
using System.Collections.Generic;

[FirestoreData]
public class MuseumObject
{
    // Dictionary to store translations for selected fields, keyed by language code
    [FirestoreProperty]
    public Dictionary<string, Dictionary<string, string>> Translations { get; set; }
    
    // Non-translatable fields
    [FirestoreProperty]
    public int Year { get; set; }
    
    [FirestoreProperty]
    public string Location { get; set; }

    // Constructor
    public MuseumObject()
    {
        Translations = new Dictionary<string, Dictionary<string, string>>();
    }

    // Method to add translation for a field in a specific language
    public void AddTranslation(string language, string field, string translation)
    {
        if (!Translations.ContainsKey(language))
        {
            Translations[language] = new Dictionary<string, string>();
        }

        Translations[language][field] = translation;
    }

    // Method to retrieve translation for a field in a specific language
    public string GetTranslation(string language, string field)
    {
        if (Translations.ContainsKey(language) && Translations[language].ContainsKey(field))
        {
            return Translations[language][field];
        }

        return null;
    }
}