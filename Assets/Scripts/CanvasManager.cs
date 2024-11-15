using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI messageText;

    public void MessageText (string message)
    {
        messageText.text = message;
        StartCoroutine(HideMessage());
    }

    IEnumerator HideMessage()
    {
        yield return new WaitForSeconds(1.2f);
        messageText.text = "";
    }

    public void RestartButton()
    {
        SceneManager.LoadScene(0);
    }
}
