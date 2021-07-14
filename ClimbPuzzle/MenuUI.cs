using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    [SerializeField] private Animator panelStart, panelNext;

    private bool showNextBtn = false;

    void Start()
    {
        panelStart.Play("Show");
    }

    public void StartGame()
    {
        SingleVar.StartGame = true;

        panelStart.Play("Hide");

        showNextBtn = true;

        GAManager.manager.StartLevel(SceneManager.GetActiveScene().name);
        FBManager.manager.StartLevel(SceneManager.GetActiveScene().name);
    }

    public void NextGame(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    void Update()
    {
        if(showNextBtn && !SingleVar.StartGame)
        {
            StartCoroutine(ShowNext());

            GAManager.manager.LevelComplete(SceneManager.GetActiveScene().name);
            FBManager.manager.LevelComplete(SceneManager.GetActiveScene().name);

            showNextBtn = false;
        }
    }

    IEnumerator ShowNext()
    {
        yield return new WaitForSeconds(1f);

        panelNext.Play("Show");
    }
}
