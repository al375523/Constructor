using HttpClientStatus;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProjectManager : MonoBehaviour
{
    ClientHTTP _clientHTTP;
    UriFactory _uriFactory;
    // Start is called before the first frame update
    void Awake()
    {
        SceneManager.LoadSceneAsync("CircuitArtDemo", LoadSceneMode.Additive);
        _clientHTTP = GetComponent<ClientHTTP>();

        DontDestroyOnLoad(this);


    }
    private void Start()
    {
        _uriFactory = GetComponent<UriFactory>();
        Uri uri = _uriFactory.GetUri("", false);
        _clientHTTP.SetJsonConstructionfiles();
    }
    private void Update()
    {

    }

}
