using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UriFactory : MonoBehaviour
{
     static string ipServer = "http://52.59.37.153";
     static string ipLocal = "http://localhost";
    string _ip;
    string _uriBase;
    public static string productionPort = "80";
    public static string stagingPort = "8080";
    string _port;
    public enum DataBase { Production, Staging }
    public DataBase editorDB = DataBase.Staging;
    public DataBase buildDB = DataBase.Production;
    public enum IP { Server, Local }
    public IP editorIP = IP.Server;
    public IP buildIP = IP.Server;
    HttpClientStatus.ClientHTTP _clientHTTP;

    private void Awake()
    {
        _clientHTTP = GetComponent<HttpClientStatus.ClientHTTP>();
        SetURIBase();
    }

    private void SetURIBase()
    {
        if (Application.isEditor)
        {
            Setport(editorDB);
            SetIP(editorIP);
        }
        else
        {
            Setport(buildDB);
            SetIP(buildIP);
        }
        _uriBase = _ip + ":" + _port + "/";
    }

    private void Setport(DataBase db)
    {
        if (db == DataBase.Staging)
        {
            _port = stagingPort;
        }
        else
        {
            _port = productionPort;
        }
    }

    private void SetIP(IP ip)
    {
        if (ip == IP.Server)
        {
            _ip = ipServer;
        }
        else
        {
            _ip = ipLocal;
            _port = "5000";
        }
    }

    public Uri GetConsuptionUri(string category, string interval, string sampleAmount)
    {
        return new Uri(_uriBase + "consumption/" + category + "/" + interval + "/" + sampleAmount + "/" + _clientHTTP.greenhouseID);
    }

    public Uri GetDataPointUri(string category, string initial_date, string final_date)
    {
        return new Uri(_uriBase + category + "/" + initial_date + "/" + final_date + "/" + _clientHTTP.greenhouseID);
    }
    public Uri GetConstructionJsonUri()
    {
        return new Uri(_uriBase + "construction/" + _clientHTTP.greenhouseID);
    }


    internal Uri GetUri(string uri, bool shouldAddGreenhouseID)
    {
        var newUri = _uriBase + uri;
        if (shouldAddGreenhouseID) newUri += _clientHTTP.greenhouseID;
        return new Uri(newUri);
    }

    public Uri GetAutenticateUri()
    {
        print(_uriBase + "users/authenticate");
        return new Uri(_uriBase + "users/authenticate");
    }
}
