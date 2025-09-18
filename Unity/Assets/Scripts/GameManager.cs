using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    [DllImport("__Internal")]
    private static extern void startGPSOverlay();

    [DllImport("__Internal")]
    private static extern void sendFacilityData();

    void Start()
    {
        startGPSOverlay();
        sendFacilityData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
