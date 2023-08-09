using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public bool gameFinished;
    private bool attemptedRestart;
    private float gameTimer;
    // Start is called before the first frame update
    void Start()
    {
        gameTimer = 0;
        gameFinished = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameFinished)
        {
            if (!attemptedRestart)
            {
                attemptedRestart = true;
                print("You completed the game in: " + Time.timeSinceLevelLoad.ToString("F3") + "s");
                Invoke(nameof(restart), 2.0f);

            }
        }
    }

    public void resetScene(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            restart();
        }
    }

    public void restart()
    {
        SceneManager.LoadScene(0);
    }
}
