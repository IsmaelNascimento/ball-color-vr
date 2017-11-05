using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VRBallManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] m_PointsInstantiates;
    [SerializeField]
    private GameObject[] m_BallsColors;
    [SerializeField]
    private float m_TimerMaxGameplay = 25;
    [SerializeField]
    private float m_TimerBallColor = 0;
    [SerializeField]
    private float m_TimeMaxBallColor = 0;
    [SerializeField]
    private float m_TimerMaxInstatiateBall = 0;
    [SerializeField]
    private float m_TimeMaxInstatiateBall = 0;
    private int m_PointsPlayer = 0;
    [SerializeField]
    private int m_LifePlayer = 3;
    private int m_BallColorIndexCurrent;
    private int m_BallIndexCurrent;
    [SerializeField]
    private Text m_TextTimer;
    [SerializeField]
    private Text m_TextPointsPlayer;
    [SerializeField]
    private Text m_TextLifePlayer;
    [SerializeField]
    private Image m_ImageLookBall;
    [HideInInspector]
    public bool m_IsGazedBall = false;
    private string tagCorrect = "";
    [SerializeField]
    private GameObject m_CanvasBallColor;
    [SerializeField]
    private GameObject m_ButtonRestart;
    [SerializeField]
    private GameObject m_CanvasTimer;
    [SerializeField]
    private GameObject m_ParticleHit;
    [SerializeField]
    private GameObject m_ParticleMissed;

    void Start()
    {
        RandomColorBall();

        m_TextLifePlayer.text = "Life: " + m_LifePlayer.ToString();
        m_TextPointsPlayer.text = "Points:\n" + m_PointsPlayer.ToString();
    }

    void Update()
    {
        if (m_LifePlayer != 0)
        {
            TimeChangeBall();
            TimeInstatiateBall();
        }

        TimeGameplay();
    }

    private void TimeInstatiateBall()
    {
        m_TimerMaxInstatiateBall += Time.deltaTime;

        if (m_TimerMaxInstatiateBall >= m_TimeMaxInstatiateBall)
        {
            InstatiateBalls();
            m_TimerMaxInstatiateBall = 0;
        }
    }

    private void TimeGameplay()
    {
        m_TimerMaxGameplay -= Time.deltaTime;
        m_TextTimer.text = m_TimerMaxGameplay.ToString("00") + "\nseconds";

        if (m_TimerMaxGameplay <= 0 || m_LifePlayer < 1)
        {
            GameOver();
        }
    }

    private void TimeChangeBall()
    {
        m_TimerBallColor += Time.deltaTime;

        if (m_TimerBallColor >= m_TimeMaxBallColor)
        {
            m_BallColorIndexCurrent = Random.Range(0, m_PointsInstantiates.Length);
            
            RandomColorBall();
            m_TimerBallColor = 0;
        }
    }

    private void RandomColorBall()
    {
        switch (m_BallColorIndexCurrent)
        {
            case 0:
                m_ImageLookBall.color = Color.red;
                tagCorrect = "ballRed";
                //print("Color ball is RED");
                break;
            case 1:
                m_ImageLookBall.color = Color.yellow;
                tagCorrect = "ballYellow";
                //print("Color ball is YELLOW");
                break;
            case 2:
                m_ImageLookBall.color = Color.blue;
                tagCorrect = "ballBlue";
                //print("Color ball is BLUE");
                break;
            case 3:
                m_ImageLookBall.color = Color.green;
                tagCorrect = "ballGreen";
                //print("Color ball is GREEN");
                break;
        }
    }

    public IEnumerator GazeEnter_Coroutine(float timeGazed, GameObject ballCurrent)
    {
        yield return new WaitForSeconds(timeGazed);

        if (m_IsGazedBall)
        {
            //print("Name gameobject:: " + ballCurrent.name);

            if(ballCurrent.tag == tagCorrect)
            {
                HitBall(ballCurrent);
            }
            else
            {
                MissedBall(ballCurrent);
            }
        }
    }

    private void HitBall(GameObject ballCurrent)
    {
        StartCoroutine(ParticleHit_Coroutine(ballCurrent));

        print("Hit Gaze");
        m_PointsPlayer += 10;
        m_TextPointsPlayer.text = "Points:\n" + m_PointsPlayer.ToString();
        m_TimerMaxGameplay += 5;
    }

    private void MissedBall(GameObject ballCurrent)
    {
        StartCoroutine(ParticleMissed_Coroutine(ballCurrent));

        print("Missed Gaze");
        if (m_PointsPlayer > 5)
        {
            m_PointsPlayer -= 5;
            m_TextPointsPlayer.text = "Points:\n" + m_PointsPlayer.ToString();
        }

        m_LifePlayer -= 1;
        m_TextLifePlayer.text = "Life: " + m_LifePlayer.ToString();
    }

    private void GameOver()
    {
        print("GameOver");  
        m_TimerMaxGameplay = 0;
        m_LifePlayer = 0;
        m_TextLifePlayer.text = "Life: " + m_LifePlayer.ToString();
        m_TextTimer.text = m_TimerMaxGameplay.ToString("00") + " seconds";
        m_CanvasBallColor.SetActive(false);
        m_CanvasTimer.AddComponent<GvrPointerGraphicRaycaster>();
        m_ButtonRestart.SetActive(true);
    }

    private void InstatiateBalls()
    {
        Instantiate(m_BallsColors[m_BallColorIndexCurrent], m_PointsInstantiates[m_BallColorIndexCurrent].transform.position, Quaternion.identity);
    }

    public void ButtonRestart()
    {
        StartCoroutine(GazeEnterRestart_Coroutine(.5f));
    }

    public IEnumerator GazeEnterRestart_Coroutine(float timeGazed)
    {
        yield return new WaitForSeconds(timeGazed);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator ParticleHit_Coroutine(GameObject ballCurrent)
    {
        m_ParticleHit.transform.position = ballCurrent.transform.position;
        Destroy(ballCurrent);
        m_ParticleHit.SetActive(true);
        yield return new WaitForSeconds(m_ParticleHit.GetComponent<ParticleSystem>().main.duration);
        m_ParticleHit.SetActive(false);
    }

    private IEnumerator ParticleMissed_Coroutine(GameObject ballCurrent)
    {
        m_ParticleMissed.transform.position = ballCurrent.transform.position;
        Destroy(ballCurrent);
        m_ParticleMissed.SetActive(true);
        yield return new WaitForSeconds(m_ParticleMissed.GetComponent<ParticleSystem>().main.duration);
        m_ParticleMissed.SetActive(false);
    }
}