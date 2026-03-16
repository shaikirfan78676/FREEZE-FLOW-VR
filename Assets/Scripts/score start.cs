using UnityEngine;

public class GameStart : MonoBehaviour
{
    public GameObject scoreUI;

    public void StartGame()
    {
        scoreUI.SetActive(true);
    }
}