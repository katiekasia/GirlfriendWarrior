using UnityEngine;

public class HoverAnimation : MonoBehaviour
{
    [Header("Hover Mechanics")]
    [Tooltip("How far up and down the object drifts from its base spot.")]
    public float hoverAmplitude = 0.12f;

    [Tooltip("How quickly the object bobs up and down.")]
    public float hoverSpeed = 2f;


    private Vector3 baselinePosition;

    void Start()
    {

        baselinePosition = transform.position;
    }

    void Update()
    {

        float currentOffset = Mathf.Sin(Time.time * hoverSpeed) * hoverAmplitude;

        transform.position = new Vector3(baselinePosition.x, baselinePosition.y + currentOffset, baselinePosition.z);
    }
}