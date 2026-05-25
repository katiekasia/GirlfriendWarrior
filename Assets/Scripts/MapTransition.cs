using UnityEngine;
using Unity.Cinemachine;

public class MapTransition : MonoBehaviour
{
    [SerializeField] PolygonCollider2D mapBoundry;
    CinemachineConfiner2D confiner;

    // It's often cleaner to declare the enum at the top of the class
    enum Direction { Up, Down, Left, Right }
    [SerializeField] Direction direction;

    private void Awake()
    {
        confiner = FindFirstObjectByType<CinemachineConfiner2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            confiner.BoundingShape2D = mapBoundry;
            confiner.InvalidateBoundingShapeCache();
            UpdatePlayerPosition(collision.gameObject);
        }
    }

    private void UpdatePlayerPosition(GameObject player)
    {
        // Fixed: Changed '-' to '='
        Vector3 newPos = player.transform.position;

        switch (direction)
        {
            case Direction.Up:
                newPos.y += 5;
                break;
            case Direction.Down:
                newPos.y -= 5;
                break;
            case Direction.Left:
                newPos.x -= 5;
                break;
            case Direction.Right:
                newPos.x += 5;
                break;
        }

        player.transform.position = newPos;
    }
}