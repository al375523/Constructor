using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LightningManager : MonoBehaviour
{
    [SerializeField] private Material skybox;

    private static readonly int Exposure = Shader.PropertyToID("_Exposure");

    public Transform rotationCenter;
    public float rotationSpeed;
    private float rotationSky = 0f;

    private float minExposure = 0.1f; // Exposición mínima del skybox
    private float maxExposure = 1.0f; // Exposición máxima del skybox
    private float minY = -55.0f; // La posición Y mínima del sol
    private float maxY = 55.0f; // La posición Y máxima del sol

    private Vector3 initialPosition;
    private Vector3 axis;
    private Vector3 point;

    private float normalizedY;
    private float targetExposure;

    private float initialSun;

    public MenuManager menuManager;
    public string menuScene;

    public GameObject sun;

    private void Start()
    {
        initialSun = 0;
        skybox.SetFloat(Exposure, maxExposure);
        axis = Vector3.left;
        point = rotationCenter.position;
        initialPosition = sun.transform.position;
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (!SceneManager.GetSceneByName(menuScene).isLoaded)
        {
            sun.SetActive(true);
            if(initialSun == 0)
            {
                initialSun = 1;
                sun.transform.position = new Vector3(sun.transform.position.x, -sun.transform.position.y, sun.transform.position.z);
            } 
        }
        else sun.SetActive(false);
            
        if(menuManager.play) UpdateLight();
    }

    public void UpdateLight()
    {
        rotationSky += rotationSpeed * Time.deltaTime;
        sun.transform.RotateAround(point, axis, Time.deltaTime * rotationSpeed);
        skybox.SetFloat("_Rotation", -rotationSky);

        normalizedY = Mathf.InverseLerp(minY, maxY, sun.transform.position.y);
        targetExposure = Mathf.Lerp(minExposure, maxExposure, normalizedY);

        skybox.SetFloat(Exposure, targetExposure);
    }

    public void CalculateSpeed()
    {
        rotationSpeed = 360f * menuManager.timePlants / menuManager.auxTime;
    } 

    public void ChangeSunPosition()
    {
        float actualHour = 360f * menuManager.timeHour / 24;
        sun.transform.position = initialPosition;
        sun.transform.RotateAround(point, axis, actualHour);
  
        normalizedY = Mathf.InverseLerp(minY, maxY, sun.transform.position.y);
        targetExposure = Mathf.Lerp(minExposure, maxExposure, normalizedY);

        skybox.SetFloat(Exposure, targetExposure);
    }
}
