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
    public string defaultScene;
    public GameObject tabletPrefab;   
    public string menuScene;
    public MinimapController minimapController;
    public GameObject minimap;

    GameObject menuObj;
    GameObject player;
    GameObject tabletInstance;
    
    ButtonListener startButton1, startButton2, startButton3, startButton4, startButton5, startButton6;
    ButtonListener[] startButtons;
    OculusInputs oculusInputs;
    MenuManager submenuManager;
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

    void Update()
    {
        
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

    public void ShowScenes()
    {
        startButton1 = GameObject.FindGameObjectWithTag("Start0").GetComponent<ButtonListener>();
        startButton1.proximityEvent.AddListener(delegate { StartSection(startButton1.transform.parent.name); });

        startButton2 = GameObject.FindGameObjectWithTag("Start1").GetComponent<ButtonListener>();
        startButton2.proximityEvent.AddListener(delegate { StartSection(startButton2.transform.parent.name); });

        startButton3 = GameObject.FindGameObjectWithTag("Start2").GetComponent<ButtonListener>();
        startButton3.proximityEvent.AddListener(delegate { StartSection(startButton3.transform.parent.name); });

        startButton4 = GameObject.FindGameObjectWithTag("Start3").GetComponent<ButtonListener>();
        startButton4.proximityEvent.AddListener(delegate { StartSection(startButton4.transform.parent.name); });

        startButton5 = GameObject.FindGameObjectWithTag("Start4").GetComponent<ButtonListener>();
        startButton5.proximityEvent.AddListener(delegate { StartSection(startButton5.transform.parent.name); });

        startButton6 = GameObject.FindGameObjectWithTag("Start5").GetComponent<ButtonListener>();
        startButton6.proximityEvent.AddListener(delegate { StartSection(startButton6.transform.parent.name); });
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
