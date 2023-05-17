using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Chiligames.MetaAvatarsPun;
using Photon.Pun;
using System;

public class MinimapController : MonoBehaviour
{
    public List<GameObject> pointerList = new List<GameObject>();
    public GameObject map;
    public Sprite highligthedPointer;
    public Sprite defaultPointer;
    public TMP_Text actualText;

    GameObject player;
    public string actualSection = "";
    ProjectManagerDemo projectManager;

    PlayerManager playerManager;
    PhotonView pv;
    OculusInputs inputSytem;
    AsyncOperation unloadOperation;
    AsyncOperation loadOperation;
    FadeScreen fader;
    // Start is called before the first frame update
    void Start()
    {
        projectManager = FindObjectOfType<ProjectManagerDemo>();
        player = GameObject.FindGameObjectWithTag("Player");
        inputSytem = player.GetComponent<OculusInputs>();
        actualSection = projectManager.sceneName;
        actualText.text = "Actual Section: " + actualSection;
        playerManager = FindObjectOfType<PlayerManager>();
        Image[] images = map.GetComponentsInChildren<Image>();
        fader = FindObjectOfType<FadeScreen>();
        pv = GetComponent<PhotonView>();
        int j = 1;
        for (int i=0; i< images.Length; i++)
        {
            if (images[i].gameObject.name.Contains("Pointer"))
            {               
                images[i].gameObject.GetComponentInChildren<TMP_Text>().text = "Section: " + j;
                pointerList.Add(images[i].gameObject);
                j += 1;
                if (images[i].gameObject.name.Contains("Pointer_01"))
                {
                    images[i].sprite = highligthedPointer;
                }
            }
        }        
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    [PunRPC]
    public void ChangeSection(string name)
    {
        if (PhotonNetwork.IsConnected && pv.AmOwner)
        {
            pv.RPC("ChangeSection", RpcTarget.Others);
        }      
        if (actualSection != name)
        {    
            StartCoroutine(WaitToScene(name));                     
        }
       
        else
        {
            Debug.Log("La escena ya está cargada");
        }
    }

    IEnumerator WaitToScene(string name)
    {
        EventManager.TriggerEvent("LOADING_SCREEN");
        fader.FadeOut();
        unloadOperation = SceneManager.UnloadSceneAsync(actualSection);
        actualSection = name;
        Resources.UnloadUnusedAssets();
        yield return new WaitUntil(() => unloadOperation.isDone);
        loadOperation = SceneManager.LoadSceneAsync(actualSection, LoadSceneMode.Additive);
        yield return new WaitUntil(() => loadOperation.isDone);
        playerManager.GetPositions();
        actualText.text = "Actual Section: " + actualSection;
        EventManager.TriggerEvent("LOADING_SECTION");
        EventManager.TriggerEvent("LOADING_SCREEN");
        EventManager.TriggerEvent("FORCE_SHOW");
        fader.FadeIn();
        inputSytem.OpenCloseMinimap();
        if (PhotonNetwork.IsMasterClient)
        {
            pv.RPC("ChangeSection", RpcTarget.AllBuffered, name);
        }
    }

    public void PointerHitted(GameObject pointer)
    {
        pointer.GetComponent<Image>().sprite = highligthedPointer;
        foreach(GameObject pt in pointerList)
        {
            if (pt != pointer)
            {
                pt.GetComponent<Image>().sprite = defaultPointer;
            }
        }
    }    
}
