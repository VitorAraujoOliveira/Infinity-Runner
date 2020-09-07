using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
   public void openGame() {
     SceneManager.LoadScene("Game", LoadSceneMode.Single);
   }

    public void openMenu() {
     SceneManager.LoadScene("Menu", LoadSceneMode.Single);
   }

    public void openGameOver()
    {
        SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
    }
}
