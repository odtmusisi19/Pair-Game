using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PictureManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Picture PicturePrefabs;
    public Transform PicSpawnPosition;
    public Vector2 StartPosition = new Vector2(-2.15f, 3.62f);
    public List<Picture> PictureList;
    private Vector2 _offset = new Vector2(1.5f, 1.52f);

    [HideInInspector]
    void Start()
    {
        SpawnPictureMesh(4, 5, StartPosition, _offset, false);
        MovePicture(4, 5, StartPosition, _offset);

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void SpawnPictureMesh(int rows, int columns, Vector2 Pos, Vector2 offset, bool scaleDown)
    {
        for (int col = 0; col < columns; col++)
        {
            for (int row = 0; row < rows; row++)
            {
                var tempPicture = (Picture)Instantiate(PicturePrefabs, PicSpawnPosition.position, PicSpawnPosition.transform.rotation);
                tempPicture.name = tempPicture.name + 'c' + col + 'r' + row;
                PictureList.Add(tempPicture);
            }
        }
    }
    private void MovePicture(int rows, int columns, Vector2 Pos, Vector2 offset)
    {
        var index = 0;
        for (int col = 0; col < columns; col++)
        {
            for (int row = 0; row < rows; row++)
            {
                var targetPosition = new Vector3((Pos.x + (offset.x * row)), (Pos.y - (offset.y * col)), 0.0f);
                StartCoroutine(MoveToPosition(targetPosition, PictureList[index]));
                index++;
            }
        }
    }
    private IEnumerator MoveToPosition(Vector3 target, Picture obj)
    {
        var randomDis = 7;
        while (obj.transform.position != target)
        {
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, target, randomDis * Time.deltaTime);
            yield return 0;
        }
    }
}
