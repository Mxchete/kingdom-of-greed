using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
#endif
using UnityEngine;
using Cinemachine;

public class MapTransition : MonoBehaviour
{
    // Start is called before the first frame update
    CinemachineConfiner confiner;
    [SerializeField] PolygonCollider2D mapBoundry;
    [SerializeField] Direction direction;
    [SerializeField] float additivePos = 2f;

    enum Direction { Up,Down,Left, Right };

    private void Awake()
    {
        
        confiner = FindObjectOfType<CinemachineConfiner>();
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            confiner.m_BoundingShape2D = mapBoundry;
            UpdatePlayerPosition(collision.gameObject);
        } 
    }
    private void UpdatePlayerPosition(GameObject player)
    {
        Vector3 newPos = player.transform.position;

        switch (direction)
        {
            case Direction.Up:
                newPos.y += additivePos;
                break;

            case Direction.Down:
                newPos.y -= additivePos;
                break;

            case Direction.Left:
                newPos.x -= additivePos;
                break;

            case Direction.Right:
                newPos.x += additivePos;
                break;

        }
    }
}
