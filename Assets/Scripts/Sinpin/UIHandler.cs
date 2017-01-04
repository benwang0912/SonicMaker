using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class UIHandler : MonoBehaviour {

    private genProjectile script;
    private GameObject pauseMenu;
    private UISlider audioSlider;
    private AudioSource bgm;

    void Start() {
        script = GameObject.Find("ProjectileManager").GetComponent<genProjectile>();
        pauseMenu = GameObject.Find("PauseMenu");
        audioSlider = pauseMenu.transform.Find("AudioControl").GetComponent<UISlider>();
        pauseMenu.SetActive(false);
        bgm = Camera.main.gameObject.transform.GetComponent<AudioSource>();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            togglePauseMenu();
        }
    }

    public void closeOnClick(GameObject obj)
    {
        obj.SetActive(false);
        Time.timeScale = 1;
    }

    public void backToMenu() {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void restart() {
        SceneManager.LoadScene("Sinpin");
    }

    public void togglePauseMenu() {
        if(pauseMenu.activeSelf == true)
            Time.timeScale = 1;
        else
            Time.timeScale = 0;
        pauseMenu.SetActive(!pauseMenu.activeSelf);
    }

    public void reload() {
        checkPointManager.load();
        togglePauseMenu();
    }

    public void toggleBGM() {
        bgm.volume = audioSlider.value;
    }
}
