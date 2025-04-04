using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonScene : MonoBehaviour
{
    public string sceneName;
    
    void Start () {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(changeScene);
    }
    
    void changeScene()
    {
        Debug.Log("Hello World");
        SceneManager.UnloadSceneAsync("Scenes/Menu");
        SceneManager.LoadScene("Scenes/Museum");
    }
}
