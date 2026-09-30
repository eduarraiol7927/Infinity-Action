using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    void Start()
    {
        
    }

    public void Iniciar()
    {
        SceneManager.LoadScene("jogo");
    }

    public void Creditos()
    {
        SceneManager.LoadScene("creditos");
    }

    public void Sair()
    {
        EditorApplication.isPlaying = false;
    }

}
