using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraMachineController : MonoBehaviour
{

    [SerializeField] List <CinemachineVirtualCameraBase> cameras;
    [SerializeField] int cameraIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        foreach( var camera in cameras)
        {
            camera.gameObject.SetActive(false);
        }
        cameras[cameraIndex].gameObject.SetActive(true);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            SwitchCamera();
        }
    }

    void SwitchCamera(){
        cameraIndex++;
        if(cameraIndex >= cameras.Count){
            cameraIndex = 0;
        }
        foreach( var camera in cameras)
        {
            camera.gameObject.SetActive(false);
        }
        cameras[cameraIndex].gameObject.SetActive(true);
    }
}
