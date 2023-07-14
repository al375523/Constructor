using Chiligames.MetaAvatarsPun;
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
    public GameObject tabletPrefab;   
    public string menuScene;

    GameObject menuObj;
    GameObject player;
    GameObject tabletInstance;
    
    ButtonListener startButton;
    OculusInputs oculusInputs;
    MenuManager submenuManager;
    //NetworkManager networkManager;
    //FadeScreen fader;
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
        //fader = FindObjectOfType<FadeScreen>();
        AudioListener.volume = 10f;
    }

    void Start()
    {   
        StartCoroutine(LoadMenuScene());       
    }

    void Update()
    {
        
    }

    public void StartButton()
    {
        menuObj.SetActive(false);        
        StartCoroutine(LoadFirstSection());
    }

    public void StartFirstSection()
    {
        StartCoroutine(LoadFirstSection());
    }

    IEnumerator LoadFirstSection()
    {
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(menuScene));
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        EventManager.TriggerEvent("LOADING_SCREEN");
        //fader.FadeOut();
        yield return new WaitUntil(() => SceneManager.GetSceneByName(sceneName).isLoaded);
        //networkManager.gameObject.SetActive(true);
        //playerManager.ConnectToServer();
        EventManager.TriggerEvent("LOADING_SCREEN");
        EventManager.TriggerEvent("LOADED_FIRST_SECTION");
        //fader.FadeIn();
        tabletInstance = Instantiate(tabletPrefab);
        tabletInstance.transform.SetParent(player.transform);
        oculusInputs.tabletPrefab = tabletInstance;
        player.transform.position = new Vector3(player.transform.position.x, originalY, player.transform.position.z);
    }

    IEnumerator LoadMenuScene()
    {
        EventManager.TriggerEvent("LOADING_SCREEN");
        //fader.FadeOut();
        SceneManager.LoadSceneAsync(menuScene, LoadSceneMode.Additive);
        yield return new WaitUntil(() => SceneManager.GetSceneByName(menuScene).isLoaded);       
        
        startButton = GameObject.FindGameObjectWithTag("StartButton").GetComponent<ButtonListener>();
        startButton.proximityEvent.AddListener(StartFirstSection);
        oculusInputs.playerManager = playerManager;
        yield return new WaitForSeconds(2f);
        EventManager.TriggerEvent("LOADING_SCREEN");
        //fader.FadeIn();
        oculusInputs.ChangeInputState(2);
    }   
    public void ReturnToMenu(bool v)
    {
        if (v)
        {            
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
        
}
