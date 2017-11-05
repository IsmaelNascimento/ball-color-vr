using UnityEngine;
using DG.Tweening;

public class BallGaze : MonoBehaviour
{
    VRBallManager vRBallManager;

    private void Start()
    {
        vRBallManager = GameObject.Find("goManager").GetComponent<VRBallManager>();
    }

    private void Update()
    {
        gameObject.transform.DOMoveY(3.5f, 5).OnComplete(() => { Destroy(gameObject); });
    }

    public void GazeEnter()
    {
        vRBallManager.m_IsGazedBall = true;

        StartCoroutine(vRBallManager.GazeEnter_Coroutine(.1f, gameObject));
    }

    public void GazeExit()
    {
        vRBallManager.m_IsGazedBall = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //print("collision.gameObject.tag:: " + collision.gameObject.tag);

        if (collision.gameObject.tag.Equals("destroyerBall"))
        {
            Destroy(gameObject);
        }
    }
}
