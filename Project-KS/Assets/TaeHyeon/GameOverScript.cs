using UnityEngine;
using UnityEngine.UI;

public class GameOverScript : MonoBehaviour
{
    void Start()
    {
        // Button ������Ʈ�� �����ɴϴ�. �ؽ�Ʈ�� ��ư�� ������ ��쿡�� �ʿ��մϴ�.
        Button btn = GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(OnGameOverClick);
        }
    }

    // ���� ���� �޼���
    public void OnGameOverClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        Debug.Log("1");
#else
        Application.Quit();
#endif
    }
}
