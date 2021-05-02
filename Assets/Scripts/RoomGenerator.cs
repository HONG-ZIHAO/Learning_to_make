using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomGenerator : MonoBehaviour
{
    public enum Direction { up,down,left,right};
    public Direction direction;
    [Header("������Ϣ")]
    public GameObject roomPrefab;
    public int roomNumber;
    public Color startColor, endColor;
    private GameObject endRoom;

    [Header("λ�ÿ���")]
    public Transform generatorPoint;
    public float xOffset;
    public float yOffset;
    public LayerMask roomLayer;

    public List<GameObject> rooms = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < roomNumber; i++)
        {
            rooms.Add(Instantiate(roomPrefab, generatorPoint.position, Quaternion.identity));
            //�ı�pointλ��
            ChangePointPos();
        }
        rooms[0].GetComponent<SpriteRenderer>().color = startColor;
        
        endRoom = rooms[0];
        foreach (var room in rooms)
        {
            if (room.transform.position.sqrMagnitude>endRoom.transform.position.sqrMagnitude)
            {
                endRoom = room;
            }
        }
        endRoom.GetComponent<SpriteRenderer>().color = endColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void ChangePointPos()
    {
        do
        {
            direction = (Direction)Random.Range(0, 4);
            switch (direction)
            {
                case Direction.up:
                    generatorPoint.position += new Vector3(0, yOffset, 0);
                    break;
                case Direction.down:
                    generatorPoint.position += new Vector3(0, -yOffset, 0);
                    break;
                case Direction.left:
                    generatorPoint.position += new Vector3(-xOffset, 0, 0);
                    break;
                case Direction.right:
                    generatorPoint.position += new Vector3(xOffset, 0, 0);
                    break;
            }
        } while (Physics2D.OverlapCircle(generatorPoint.position,0.2f,roomLayer));
    }
}