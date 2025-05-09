using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TMP_Text countdownText;
    public GameObject scoreUI; // ← ScoreUI 오브젝트 연결

    private float timer = 0f;
    private bool isCounting = true;

    void Start()
    {
        countdownText.gameObject.SetActive(true);
        scoreUI.SetActive(false); // 시작 시 비활성화
    }

    void Update()
    {
        if (!isCounting) return;

        timer += Time.deltaTime;

        if (timer < 1f)
        {
            countdownText.text = "3";
        }
        else if (timer < 2f)
        {
            countdownText.text = "2";
        }
        else if (timer < 3f)
        {
            countdownText.text = "1";
        }
        else if (timer < 4f)
        {
            countdownText.text = "Start!";
        }
        else
        {
            countdownText.gameObject.SetActive(false);
            scoreUI.SetActive(true);     // 4초 후 ScoreUI 활성화
            isCounting = false;
        }
    }
}
