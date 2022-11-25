using UnityEngine;

public class FinalDoor : MonoBehaviour
{
    [SerializeField] private SceneLoader sceneLoader;

    [SerializeField] private int scene;
    
    public void ChangeSceneAfter (float seconds)
    {
        Invoke(nameof(LoadFinalScene), seconds);
    }

    private void LoadFinalScene ()
    {
        sceneLoader.LoadSceneWithData(scene);
    }
}
