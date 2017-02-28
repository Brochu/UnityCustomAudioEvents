using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ExampleScript : MonoBehaviour
{
    protected void Start ()
    {
        StartCoroutine(DebugRoutine());
    }

    protected void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }
    }

    private IEnumerator DebugRoutine()
    {
        yield return new WaitForSeconds(2.0f);
        AudioManager.Instance.PlayEvent("DEBUG_EVENT", 2.0f);
        yield return new WaitForSeconds(1.0f);
        AudioManager.Instance.StopOldest("DEBUG_EVENT");
    }
}