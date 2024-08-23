using UnityEngine;
using UnityEngine.UI;

public class GameOverScript : MonoBehaviour
{
    void Start()
    {
        // Button 컴포넌트를 가져옵니다. 텍스트가 버튼을 포함한 경우에만 필요합니다.
        Button btn = GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(OnGameOverClick);
        }
    }

    // 게임 종료 메서드
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
