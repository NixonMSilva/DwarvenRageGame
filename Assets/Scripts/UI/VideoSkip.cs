using UnityEngine;
using UnityEngine.Events;

public class VideoSkip : MonoBehaviour
{
    [SerializeField] private UnityEvent OnKeyPressed;
    
    private void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnKeyPressed?.Invoke();
        }
    }
}
