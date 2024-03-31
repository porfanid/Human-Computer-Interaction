using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using Firebase.Extensions;


public class FirebaseController : MonoBehaviour
{

    public string name = "Medicine UOI Museum";

    private FirebaseFirestore firestore;
    private Firebase.FirebaseApp app;
    
    
    
    void Start()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
          var dependencyStatus = task.Result;
          if (dependencyStatus == Firebase.DependencyStatus.Available) {
            // Create and hold a reference to your FirebaseApp,
            // where app is a Firebase.FirebaseApp property of your application class.
              app = Firebase.FirebaseApp.DefaultInstance;

            // Set a flag here to indicate whether Firebase is ready to use by your app.
          } else {
            UnityEngine.Debug.LogError(System.String.Format(
              "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
            // Firebase Unity SDK is not safe to use here.
          }
        });
        firestore = FirebaseFirestore.DefaultInstance;





        MuseumObject newObject = new MuseumObject();

        // Add translations for Name, Description, and Artist
        newObject.AddTranslation("en", "Name", "Monalisa");
        newObject.AddTranslation("en", "Description", "A famous painting by Leonardo da Vinci");
        newObject.AddTranslation("en", "Artist", "Leonardo da Vinci");

        // Set non-translatable fields
        newObject.Year = 1503;
        newObject.Location = "Louvre Museum, Paris";

        // Add the MuseumObject to Firestore
        AddMuseumObject(newObject);
    }

    public void AddMuseumObject(MuseumObject obj)
    {
        DocumentReference docRef = firestore.Collection(name).Document(obj.Translations["en"]["Name"]);
        docRef.SetAsync(obj).ContinueWithOnMainThread(task =>
        {
            Debug.Log("Document added with ID: " + docRef.Id);
        });
    }
}
