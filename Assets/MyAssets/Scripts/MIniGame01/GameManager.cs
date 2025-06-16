using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    enum State
    {
        Play,
        GameOver,
        GameClear,
        Ready
    }

    State state;
    [SerializeField] private MiniGamePlayerController sheepControllerPrefab;
    [SerializeField] public Animator animator;
    [SerializeField] private bool isGameOver = false;
    [SerializeField] private float timer = 0;
    [SerializeField] public TextMeshProUGUI stateText;
    public Object Prefab_Transition;

    MiniGamePlayerController sheepController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.timeScale = 1;
        sheepController = Instantiate(sheepControllerPrefab);
        sheepController.transform.position = new Vector3(-4, 8, 0);
        state = State.Ready;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        timer += Time.deltaTime;

        if (timer >= 30.0f)
        {
            stateText.text = "GO Run Way !!";
            state = State.GameClear;
        }
        else
        {
            switch (state)
            {
                case State.Ready:
                    stateText.text = "Hide !!";
                    DOVirtual.DelayedCall(3f, () =>
                    state = State.Play);
                    break;

                case State.Play:
                    stateText.text = "";
                    if (sheepController.IsDead)
                    {
                        GameOver();
                    }
                    break;

                case State.GameOver:

                    stateText.text = "Go Retry";
                    Time.timeScale = 0;
                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        Reload();
                    }
                    break;
            }
        }
    }


    private void GameOver()
    {
        state = State.GameOver;
        isGameOver = true;
        animator.speed = 0;
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }

    private void Reload()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName);
    }


}
