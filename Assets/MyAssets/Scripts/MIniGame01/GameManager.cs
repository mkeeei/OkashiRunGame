using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public MiniGamePlayerController miniGamePlayerController;

    enum State
    {
        Play,
        GameOver
    }

    State state;
    [SerializeField] public MiniGamePlayerController sheepController;
    [SerializeField] public GameObject playerPrefab;
    [SerializeField] public Animator animator;
    [SerializeField] private bool isGameOver = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject player = Instantiate(playerPrefab);
        player.transform.position = new Vector3(-5, 8, 0);
        state = State.Play;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        switch (state)
        {

            case State.Play:
                if (sheepController.IsDead())
                {
                    Debug.Log("IsDead‚ªŒÄ‚Î‚ê‚ÄGameOver‚ðŒÄ‚Ñ‚Ü‚µ‚½");
                    GameOver();
                }
                break;

            case State.GameOver:
                if (Input.GetButtonDown("Fire1"))
                {
                    Reload();
                }
                break;
        }
    }


    private void GameOver()
    {
        state = State.GameOver;
        isGameOver = true;
        Time.timeScale = 0;
        animator.speed = 0;
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }

    private void Reload()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}
