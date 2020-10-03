using Assets.Core.Options;
using UnityEngine;

public class DebugTestStarter : MonoBehaviour
{
    private static DebugTestStarter _instance;

    public static DebugTestStarter Instance => _instance ?? (_instance = FindObjectOfType<DebugTestStarter>());

    public CameraOptions cameraOptions;

    public GridGeneratorOptions gridGeneratorOptions;

    public GridEnvironmentOptions gridEnvironmentOptions;

    public PlayerOptions playerOptions;
}
