using Chiligames.MetaAvatarsPun;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProjectManagerDemo : MonoBehaviour
{

    [HideInInspector] public GameObject avatarObject = null;
    [HideInInspector] public Vector3 initialPosition;
    [HideInInspector] public Quaternion initialRotation;
    [HideInInspector] public float originalY;
    [HideInInspector] public float actualY = 0f;

    public string sceneName;
    public string defaultScene;
    public GameObject tabletPrefab;   
    public string menuScene;
    public MinimapController minimapController;
    public GameObject minimap;

    GameObject menuObj;
    GameObject player;
    GameObject tabletInstance;
    
    OculusInputs oculusInputs;
    public MenuManager submenuManager;
    //NetworkManager networkManager;
    FadeScreen fader;
    PlayerManager playerManager;
    void Awake()
    {
        /*networkManager = GameObject.FindGameObjectWithTag("Network").GetComponent<NetworkManager>();
        networkManager.gameObject.SetActive(false);*/
        playerManager = GameObject.FindGameObjectWithTag("PlayerManager").GetComponent<PlayerManager>();
        initialPosition = Vector3.zero;
        initialRotation = Quaternion.identity;
        originalY = 2f;
        actualY = originalY;
        player = GameObject.FindGameObjectWithTag("Player");        
        oculusInputs = player.GetComponent<OculusInputs>();
        fader = FindObjectOfType<FadeScreen>();
        AudioListener.volume = 10f;
    }

    void Start()
    {        
        StartCoroutine(LoadMenuScene());       
    }

    public void StartSection(string name)
    {
        StartCoroutine(LoadSection(name));
    }

    public IEnumerator LoadSection(string name)
    {
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(menuScene));
        sceneName = name;
        SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
        EventManager.TriggerEvent("LOADING_SCREEN");
        fader.FadeOut();
        
        yield return new WaitUntil(() => SceneManager.GetSceneByName(name).isLoaded);
        //networkManager.gameObject.SetActive(true);
        //playerManager.ConnectToServer();
        EventManager.TriggerEvent("LOADING_SCREEN");
        EventManager.TriggerEvent("LOADED_FIRST_SECTION");
        fader.FadeIn();
        
        tabletInstance = Instantiate(tabletPrefab);
        tabletInstance.transform.SetParent(player.transform);
        oculusInputs.tabletPrefab = tabletInstance;
        //player.transform.position = new Vector3(player.transform.position.x, originalY, player.transform.position.z);
        playerManager.GetPositions();
        
    }

    IEnumerator LoadMenuScene()
    {
        EventManager.TriggerEvent("LOADING_SCREEN");
        fader.FadeOut();
        SceneManager.LoadSceneAsync(menuScene, LoadSceneMode.Additive);
        yield return new WaitUntil(() => SceneManager.GetSceneByName(menuScene).isLoaded);

        oculusInputs.playerManager = playerManager;
        yield return new WaitForSeconds(2f);
        EventManager.TriggerEvent("LOADING_SCREEN");
        fader.FadeIn();
        oculusInputs.ChangeInputState(2);
    
    }

    public void ReturnToMenu(bool v)
    {
        if (v)
        {
            submenuManager.timeSpeed = 1f;
            Time.timeScale = Convert.ToSingle(submenuManager.timeSpeed);
            oculusInputs.ChangeMovementState(0);
            StartCoroutine(LoadMenu());
        }
        else
        {
            submenuManager.actualPanel.SetActive(true);
        }
    }

    IEnumerator LoadMenu()
    {        
        EventManager.TriggerEvent("LOADING_SCREEN");
        yield return null;       
        SceneManager.LoadSceneAsync(0);       
    }

    bool DoesSceneExist(string name)
    {
        if (string.IsNullOrEmpty(name))
            return false;

        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            var scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            var lastSlash = scenePath.LastIndexOf("/");
            var sceneName = scenePath.Substring(lastSlash + 1, scenePath.LastIndexOf(".") - lastSlash - 1);

            if (string.Compare(name, sceneName, true) == 0)
                return true;
        }

        return false;
    }
}
