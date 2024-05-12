using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttonevents : MonoBehaviour
{
    public void Quit() {
        Application.Quit();
    }

    public void loadGame() {
        SceneManager.LoadScene("Demo Scene");
    }
}
