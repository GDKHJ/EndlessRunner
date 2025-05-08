using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    //��Ʈ�� ��ư�� ���ؼ� �������� �޴�
    public GameObject ControlKeyMenu;
    //���� �ȳ��� �޴�
    public GameObject ExitMenu;

    public void OnStartButtonEnter()
    {
        SceneManager.LoadScene("GameScene");
        //GameScene ������ �ε��մϴ�.
    }

 
    //���� �ִٸ� ����
    public void OnControlKeyButtonEnter()
    {
        //activeSelf�� ������ ���ӿ�����Ʈ�� Ȱ�� ���������� ���θ� Ȯ���� �� �ִ� ������Ƽ
        if(ControlKeyMenu.activeSelf == true)
        {
            ControlKeyMenu.SetActive(false);
        }
        else
        {
            ControlKeyMenu.SetActive(true);
        }        
    }

    //������ �� ȯ�濡���� �����
    //���� �� ȯ�濡���� ���Ḧ ��Ȳ�� ���� ó���մϴ�.
    public void OnExitButtonEnter()
    {
        if (ControlKeyMenu.activeSelf == true)
        {
            ExitMenu.SetActive(false);
        }
        else
        {
            ExitMenu.SetActive(true);

        }   
    }

    public void Exit()
    {
#if UNITY_EDITOR // ����Ƽ ������ �ʿ����� �۾�
        UnityEditor.EditorApplication.isPlaying = false;
        //������ �ٷ� ������ ���(�����, �����)
#else
        Application.Quit(); //���� ��Ȱ��ȭ�Ǵ� �ڵ尡 �ٷ� ����
#endif
    }
}
