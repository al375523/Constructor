using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GestureDetector : MonoBehaviour
{
    public float threshold = 0.05f;
    public OVRSkeleton skeleton;
    public List<Gesture> gestures;
    public bool debugMode = true;
    public Gesture actualGesture;

    private List<OVRBone> fingerBones;
    private Gesture previousGesture;
    bool canRecognize = false;
    bool canceled = false;
    // Start is called before the first frame update
    void Start()
    {
        previousGesture = new Gesture();
        StartCoroutine(WaitSkeleton());
    }

    // Update is called once per frame
    void Update()
    {        
            if (debugMode && Input.GetKeyDown(KeyCode.Space))
            {
                Save();
            }
            if (!debugMode && canRecognize)
            {
                Gesture currentGesture = Recognize();
                actualGesture = currentGesture;
                bool hasRecognized = !currentGesture.Equals(new Gesture());
                if (hasRecognized && !currentGesture.Equals(previousGesture))
                {
                    Debug.Log("New Gesture Found: " + currentGesture.name);
                    previousGesture = currentGesture;
                    canceled = false;
                    currentGesture.onRecognized.Invoke();
                }
                if (!canceled && previousGesture.onCanceled != null && !currentGesture.Equals(previousGesture))
                {
                    previousGesture.onCanceled.Invoke();
                    previousGesture = new Gesture();
                    canceled = true;
                }

            }
        
    }

    void Save()
    {
        Gesture g = new Gesture();
        g.name = "New Gesture";
        List<Vector3> data = new List<Vector3>();
        int i = 1;
        foreach (var bone in fingerBones)
        {
            i += 1;
            data.Add(skeleton.transform.InverseTransformPoint(bone.Transform.position));
        }

        g.fingerDatas = data;
        gestures.Add(g);
    }

    Gesture Recognize()
    {
        Gesture currentGesture = new Gesture();
        float currentMin = Mathf.Infinity;

        foreach (var gesture in gestures)
        {
            float sumDistance = 0;
            bool discarded = false;
            for (int i = 0; i < fingerBones.Count; i++)
            {
                Vector3 currentData = skeleton.transform.InverseTransformPoint(fingerBones[i].Transform.position);
                float distance = Vector3.Distance(currentData, gesture.fingerDatas[i]);
                if (distance > threshold)
                {
                    discarded = true;
                    break;
                }

                sumDistance += distance;
            }

            if (!discarded && sumDistance < currentMin)
            {
                currentMin = sumDistance;
                currentGesture = gesture;
            }
        }

        return currentGesture;
    }

    public void Prueba()
    {
        Debug.Log("Hola");
    }

    IEnumerator WaitSkeleton()
    {
        yield return new WaitUntil(() => skeleton.Bones.Count != 0);
        fingerBones = new List<OVRBone>(skeleton.Bones);
        canRecognize = true;
    }

}
[System.Serializable]
public struct Gesture
{
    public string name;
    public List<Vector3> fingerDatas;
    public UnityEvent onRecognized;
    public UnityEvent onCanceled;

}
