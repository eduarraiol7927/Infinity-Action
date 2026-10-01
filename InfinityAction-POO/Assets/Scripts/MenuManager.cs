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
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void Voltar()
    {
        SceneManager.LoadScene("iniciar jogo");
    }
}
