using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TarodevController;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InGameUIController : MonoBehaviour
{
    [SerializeField] private string nextLevel;
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private Canvas levelComplete;
    [SerializeField] private Button levelCompleteFirstButton;
    [SerializeField] private Canvas pause;
    [SerializeField] private Button pauseFirstButton;
    [SerializeField] private Canvas saveIndicator;
    [SerializeField] private Image saveIcon;
    [SerializeField] private Image arrow;
    [SerializeField] private float saveIndicatorOffset;

    public Vector3 savePoint;
    public bool hasSave = false;

    private Camera mainCamera;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        mainCamera = FindObjectOfType<Camera>();
        levelComplete.enabled = false;
        pause.enabled = false;
        saveIndicator.enabled = false;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (hasSave) {
            float height = mainCamera.orthographicSize * 2;
            float width = height * mainCamera.aspect;
            float x = mainCamera.transform.position.x;
            float y = mainCamera.transform.position.y;
            Rect cameraBounds = new Rect(x - width / 2, y - height / 2, width, height);
            if (!cameraBounds.Contains(savePoint)) {
                // Get bounds
                saveIndicator.enabled = true;
                Rect canvasBounds = saveIndicator.GetComponent<RectTransform>().rect;
                Vector2 center = new Vector2(canvasBounds.width/2, canvasBounds.height/2);
                Rect indicatorBounds = new Rect(0, 0, canvasBounds.width - 2*saveIndicatorOffset, canvasBounds.height - 2*saveIndicatorOffset);

                // Calculate indictor angle
                Vector2 indicatorDirection = savePoint - mainCamera.transform.position;
                indicatorDirection = indicatorDirection / indicatorDirection.magnitude;

                // Position indicator
                Vector2 indicatorPosition = PointOnBounds(indicatorBounds, indicatorDirection) + center;
                saveIcon.rectTransform.position = indicatorPosition;
                arrow.rectTransform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, indicatorDirection));
            } else {
                saveIndicator.enabled = false;
            }
        }
    }

    public void ShowLevelComplete() {
        levelComplete.enabled = true;
        levelCompleteFirstButton.Select();
    }

    public void TogglePause() {
        if (pause.enabled)
            Unpause();
        else
            Pause();
    }

    public void Restart() {
        Unpause();
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
        eventSystem.SetSelectedGameObject(null);
    }

    public void Menu() {
        Unpause();
        SceneManager.LoadScene("MainMenu");
    }

    public void NextLevel() {
        Unpause();
        SceneManager.LoadScene(nextLevel);
    }

    public void Continue() {
        Unpause();
        eventSystem.SetSelectedGameObject(null);
    }

    private void Pause() {
        pause.enabled = true;
        Time.timeScale = 0;
        FindObjectOfType<PlayerController>().Deactivate();
        pauseFirstButton.Select();
    }

    private void Unpause() {
        pause.enabled = false;
        Time.timeScale = 1;
        FindObjectOfType<PlayerController>().Activate();
        eventSystem.SetSelectedGameObject(null);
    }

    public static Vector2 PointOnBounds(Rect bounds, Vector2 aDirection) {
        aDirection.Normalize();
        var xSign = Mathf.Sign(aDirection.x);
        var ySign = Mathf.Sign(aDirection.y);
        var e = new Vector2(bounds.width/2, bounds.height/2);
        var v = new Vector2(Mathf.Abs(aDirection.x), Mathf.Abs(aDirection.y));
        float y = e.x * v.y / v.x;
        if (Mathf.Abs(y) < e.y)
            return new Vector2(xSign * e.x, ySign * y);
        return new Vector2(xSign * e.y * v.x / v.y, ySign * e.y);
    }
}
