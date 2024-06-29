using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GameMenuManager : MonoBehaviour
{
    public void ContinueGame()
    {
        Debug.Log("Continue Game");
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
    }

    public void NewGame()
    {
        Debug.Log("New Game");
    }
    
    public void Options()
    {
        Debug.Log("Options");
    }
    
    public void Credits()
    {
        Debug.Log("Credits");
    }
}