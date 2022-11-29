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
    private List<Material> _materialList = new List<Material>();
    private List<string> _texturePathList = new List<string>();
    private Material _firstMaterial;
    private string _firstTexturePath;

    [HideInInspector]
    void Start()
    {
        LoadMaterials();
        SpawnPictureMesh(4, 5, StartPosition, _offset, false);
        MovePicture(4, 5, StartPosition, _offset);

    }

    private void LoadMaterials()
    {
        var materialFilePath = GameSettings.Instance.GetMaterialDirectoryName();
        var textureFilePath = GameSettings.Instance.GetPuzzleCategoryTextureDirectoryName();
        var pairNumber = (int)GameSettings.Instance.GetPairNumber();
        const string matBaseName = "Pic";
        var firstMaterialName = "Back";
        for (var index = 1; index < pairNumber; index++)
        {
            var currentFilePath = materialFilePath + matBaseName + index;
            Material mat = Resources.Load(currentFilePath, typeof(Material)) as Material;
            _materialList.Add(mat);

            var currentTextureFilePath = textureFilePath + matBaseName + index;
            _texturePathList.Add(currentTextureFilePath);
        }
        _firstTexturePath = textureFilePath + firstMaterialName;
        _firstMaterial = Resources.Load(materialFilePath + firstMaterialName, typeof(Material)) as Material;
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
                var tempPicture = (Picture)Instantiate(PicturePrefabs, PicSpawnPosition.position, PicturePrefabs.transform.rotation);
                tempPicture.name = tempPicture.name + 'c' + col + 'r' + row;
                PictureList.Add(tempPicture);
            }
        }
        ApplyTextures();
    }
    public void ApplyTextures()
    {
        var rndMatIndex = Random.Range(0, _materialList.Count);
        var AppliedTimes = new int[_materialList.Count];

        for (int i = 0; i < _materialList.Count; i++)
        {
            AppliedTimes[i] = 0;
        }
        foreach (var o in PictureList)
        {
            var randPrevious = rndMatIndex;
            var counter = 0;
            var forceMat = false;

            while (AppliedTimes[rndMatIndex] >= 2 || ((randPrevious == rndMatIndex && !forceMat)))
            {
                rndMatIndex = Random.Range(0, _materialList.Count);
                counter++;
                if (counter > 100)
                {
                    for (var j = 0; j < _materialList.Count; j++)
                    {
                        if (AppliedTimes[j] < 2)
                        {
                            rndMatIndex = j;
                            forceMat = true;
                        }
                    }
                    if (forceMat == false)
                        return;
                }
            }
            o.SetFirstMaterial(_firstMaterial, _firstTexturePath);
            o.ApplyFirstMaterial();
            o.SetSecondMaterial(_materialList[rndMatIndex], _texturePathList[rndMatIndex]);
            AppliedTimes[rndMatIndex] += 1;
            forceMat = false;
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
