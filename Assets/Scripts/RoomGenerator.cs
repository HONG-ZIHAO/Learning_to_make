using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomGenerator : MonoBehaviour
{
    public enum Direction { up, down, left, right };
    public Direction direction;
    [Header("房间信息")]
    public GameObject roomPrefab;
    public int roomNumber;
    public Color startColor, endColor;
    private GameObject endRoom;

    [Header("位置控制")]
    public Transform generatorPoint;
    public float xOffset;
    public float yOffset;
    public LayerMask roomLayer;

    public int maxStep;
    //将Room的代码在这个代码里面实例化
    public List<Room> rooms = new List<Room>();
    //记录最远房间的列表
    List<GameObject> farRooms = new List<GameObject>();
    //记录次远房间的列表
    List<GameObject> lessFarRooms = new List<GameObject>();
    //记录只有一个门的房间的列表
    List<GameObject> oneWayRooms = new List<GameObject>();

    public WallType WallType;
    void Start()
    {
        for (int i = 0; i < roomNumber; i++)
        {
            rooms.Add(Instantiate(roomPrefab, generatorPoint.position, Quaternion.identity).GetComponent<Room>());
            //改变point位置
            ChangePointPos();
        }
        rooms[0].GetComponent<SpriteRenderer>().color = startColor;

        endRoom = rooms[0].gameObject;
        //找到最后的房间
        foreach (var room in rooms)
        {

            SetupRoom(room, room.transform.position);
        }
        FindEndRoom();

        endRoom.GetComponent<SpriteRenderer>().color = endColor;

    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.anyKey)
        //{
        //    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //}
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
        } while (Physics2D.OverlapCircle(generatorPoint.position, 0.2f, roomLayer));
    }
    public void SetupRoom(Room newRoom, Vector3 roomPosition)
    {
        newRoom.roomUp = Physics2D.OverlapCircle(roomPosition + new Vector3(0, yOffset, 0), 0.2f, roomLayer);
        newRoom.roomDown = Physics2D.OverlapCircle(roomPosition + new Vector3(0, -yOffset, 0), 0.2f, roomLayer);
        newRoom.roomLeft = Physics2D.OverlapCircle(roomPosition + new Vector3(-xOffset, 0, 0), 0.2f, roomLayer);
        newRoom.roomRight = Physics2D.OverlapCircle(roomPosition + new Vector3(xOffset, 0, 0), 0.2f, roomLayer);
        newRoom.UpdateRoom(xOffset, yOffset);
        //通过开口数量和开口方向判断使用何种墙壁
        switch (newRoom.doorNumber)
        {
            case 1:
                if (newRoom.roomUp)
                    Instantiate(WallType.singleUp, roomPosition, Quaternion.identity);
                if (newRoom.roomDown)
                    Instantiate(WallType.singleBottom, roomPosition, Quaternion.identity);
                if (newRoom.roomLeft)
                    Instantiate(WallType.singleLeft, roomPosition, Quaternion.identity);
                if (newRoom.roomRight)
                    Instantiate(WallType.singleRight, roomPosition, Quaternion.identity);
                break;
            case 2:
                if (newRoom.roomUp && newRoom.roomDown)
                    Instantiate(WallType.doubleUB, roomPosition, Quaternion.identity);
                if (newRoom.roomUp && newRoom.roomLeft)
                    Instantiate(WallType.doubleLU, roomPosition, Quaternion.identity);
                if (newRoom.roomUp && newRoom.roomRight)
                    Instantiate(WallType.doubleUR, roomPosition, Quaternion.identity);
                if (newRoom.roomDown && newRoom.roomLeft)
                    Instantiate(WallType.doubleLB, roomPosition, Quaternion.identity);
                if (newRoom.roomDown && newRoom.roomRight)
                    Instantiate(WallType.doubleRB, roomPosition, Quaternion.identity);
                if (newRoom.roomLeft && newRoom.roomRight)
                    Instantiate(WallType.doubleLR, roomPosition, Quaternion.identity);
                break;
            case 3:
                if (newRoom.roomUp && newRoom.roomDown && newRoom.roomLeft)
                    Instantiate(WallType.tripleLUB, roomPosition, Quaternion.identity);
                if (newRoom.roomUp && newRoom.roomDown && newRoom.roomRight)
                    Instantiate(WallType.tripleURB, roomPosition, Quaternion.identity);
                if (newRoom.roomDown && newRoom.roomLeft && newRoom.roomRight)
                    Instantiate(WallType.tripleLRB, roomPosition, Quaternion.identity);
                if (newRoom.roomUp && newRoom.roomLeft && newRoom.roomRight)
                    Instantiate(WallType.tripleLUR, roomPosition, Quaternion.identity);
                break;
            case 4:
                if (newRoom.roomUp && newRoom.roomDown && newRoom.roomLeft && newRoom.roomRight)
                    Instantiate(WallType.fourDoors, roomPosition, Quaternion.identity);
                break;

        }
    }
    public void FindEndRoom()
    {
        //获得最大数值的标准
        for (int i = 0; i < rooms.Count; i++)
        {
            if (rooms[i].stepToStart > maxStep)
                maxStep = rooms[i].stepToStart;

        }
        //获得最大房间和第二大的房间
        foreach (var room in rooms)
        {
            if (room.stepToStart == maxStep)
                farRooms.Add(room.gameObject);
            if (room.stepToStart == maxStep - 1)
                lessFarRooms.Add(room.gameObject);
        }
        for (int i = 0; i < farRooms.Count; i++)
        {
            if (farRooms[i].GetComponent<Room>().doorNumber == 1)
                oneWayRooms.Add(farRooms[i]);
        }
        for (int i = 0; i < lessFarRooms.Count; i++)
        {
            if (lessFarRooms[i].GetComponent<Room>().doorNumber == 1)
                oneWayRooms.Add(lessFarRooms[i]);
        }
        //在之前获取的最远房间和次远房间列表只有一个门的里面随机选择一个房间做最终房间，如果没有只有一个门的，则随机从最远房间里面选一个
        if (oneWayRooms.Count != 0)
        {
            endRoom = oneWayRooms[Random.Range(0, oneWayRooms.Count)];

        }
        else
        {
            endRoom = farRooms[Random.Range(0, farRooms.Count)];

        }
    }
}

//通过WallType类来收集所有的墙壁类型，并且通过System.Serializable来将类在unity中显示出来
[System.Serializable]
public class WallType
{
    public GameObject
        singleLeft, singleRight, singleUp, singleBottom,

        doubleLU, doubleLR, doubleLB, doubleUR, doubleUB, doubleRB,

        tripleLUR, tripleLUB, tripleURB, tripleLRB,

        fourDoors;

}