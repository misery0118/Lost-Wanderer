using UnityEngine;
using System.Collections;

public class CubeColorChanger : MonoBehaviour
{
    private Renderer cubeRenderer;
    public int cubeIndex; // The index of this cube in the sequence
    private Color originalColor; // Store the original color of the cube

    void Start()
    {
        cubeRenderer = GetComponent<Renderer>();
        originalColor = cubeRenderer.material.color; // Save the original color
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Notify the GameManager that this cube was triggered
            GameManager.Instance.CubeTriggered(this);
        }
    }

    public void SetColor(Color color)
    {
        cubeRenderer.material.color = color;
    }

    public void ResetColor()
    {
        cubeRenderer.material.color = originalColor; // Reset to original color
    }

    public void RevertToOriginalColor(float delay)
    {
        StartCoroutine(RevertColorCoroutine(delay));
    }

    private IEnumerator RevertColorCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        ResetColor();
    }
}
