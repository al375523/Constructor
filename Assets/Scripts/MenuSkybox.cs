using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSkybox : MonoBehaviour
{
    public string menuScene;
    public Material materialMenu;
    public Material materialSubmenu;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetSceneByName(menuScene).isLoaded) RenderSettings.skybox = materialMenu;
        else RenderSettings.skybox = materialSubmenu;
    }
}
