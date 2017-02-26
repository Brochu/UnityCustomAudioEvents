using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ExampleScript : MonoBehaviour
{
    protected void Start ()
    {
        AudioManager.Instance.PlayEvent("DEBUG_EVENT", 5.0f);
    }

    protected void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }
    }
}