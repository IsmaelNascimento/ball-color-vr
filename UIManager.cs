using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private bool m_IsGaze = false;

    public void GazeEnterButtonPlay()
    {
        m_IsGaze = true;
        StartCoroutine(GazeEnterButtonPlay_Coroutine());
    }

    public void GazeExitButtonPlay()
    {
        m_IsGaze = false;
    }

    private IEnumerator GazeEnterButtonPlay_Coroutine()
    {
        yield return new WaitForSeconds(2f);

        if (m_IsGaze)
        {
            SceneManager.LoadScene("Gameplay");
        }
    }
}
