using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTP : MonoBehaviour
{
    public void EndGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("EndScene");
    }
}
