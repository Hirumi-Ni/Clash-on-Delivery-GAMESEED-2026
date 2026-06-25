using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject creditPanel;
    [SerializeField] private GameObject howToPlayPanel;

    private void Start()
    {
        CloseAllPanels();
    }

    // ── Tombol Start ─────────────────────────────
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    // ── Tombol Option ─────────────────────────────
    public void OpenOptions()
    {
        CloseAllPanels();
        optionsPanel.SetActive(true);
    }

    public void CloseOptions()
    {
        optionsPanel.SetActive(false);
    }

    // ── Tombol Credit ─────────────────────────────
    public void OpenCredit()
    {
        CloseAllPanels();
        creditPanel.SetActive(true);
    }

    public void CloseCredit()
    {
        creditPanel.SetActive(false);
    }

    // ── Tombol How To Play ────────────────────────
    public void OpenHowToPlay()
    {
        CloseAllPanels();
        howToPlayPanel.SetActive(true);
    }

    public void CloseHowToPlay()
    {
        howToPlayPanel.SetActive(false);
    }

    // ── Tombol Exit ───────────────────────────────
    public void ExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    // ── Helper ────────────────────────────────────
    private void CloseAllPanels()
    {
        if (optionsPanel)    optionsPanel.SetActive(false);
        if (creditPanel)     creditPanel.SetActive(false);
        if (howToPlayPanel)  howToPlayPanel.SetActive(false);
    }
}
