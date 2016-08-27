using UnityEngine;
using System.Collections;
namespace menuSpace
{
    public class mainMenu : MonoBehaviour
    {

        public void gameNew()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(2);
            //Application.LoadLevel(1);
        }

        public void gameQuit()
        {
            Application.Quit();
        }
    }
}