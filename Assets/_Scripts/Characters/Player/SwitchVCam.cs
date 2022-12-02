using UnityEngine;
using Cinemachine;

public class SwitchVCam : MonoBehaviour
{
    [SerializeField] private Canvas _canvasMain;
    [SerializeField] private Canvas _canvasAim;

    private CinemachineVirtualCamera _vCam;
 


    private void Awake()
    {
        _vCam = GetComponent<CinemachineVirtualCamera>();
        CancelAim();
      
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(2))        
            StartAim();
        

        if (Input.GetMouseButtonUp(2))        
            CancelAim();
        
    }

    private void StartAim()
    {
        _vCam.Priority = 20;
        _canvasMain.enabled = false;
        _canvasAim.enabled = true;

    }

    private void CancelAim()
    {
        _vCam.Priority = 9;
        _canvasMain.enabled = true;
        _canvasAim.enabled = false;
    }

    
}
