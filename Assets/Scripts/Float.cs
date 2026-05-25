using UnityEngine;

public class HoverAnimation : MonoBehaviour
{
    [Header("Hover Mechanics")]
    [Tooltip("How far up and down the object drifts from its base spot.")]
    public float hoverAmplitude = 0.12f;

    [Tooltip("How quickly the object bobs up and down.")]
    public float hoverSpeed = 2f;

    // Stores the baseline position so the object never drifts away
    private Vector3 baselinePosition;

    void Start()
    {
        // Lock in the exact map coordinate where you placed it in the editor
        baselinePosition = transform.position;
    }

    void Update()
    {
        // Use a mathematical Sine wave (cycles over time between -1 and 1) to calculate vertical offset
        float currentOffset = Mathf.Sin(Time.time * hoverSpeed) * hoverAmplitude;

        // Apply the offset strictly to the Y-axis while forcing X and Z to stay firmly in place
        transform.position = new Vector3(baselinePosition.x, baselinePosition.y + currentOffset, baselinePosition.z);
    }
}