using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenerationTile : MonoBehaviour
{
    [Header("Scene Parameters")]
    public int Maze = 3;
    public int Cliff = 1;
    public int WightMaze = 1;
    public Transform es;

    [Header("Tiles")]
    public GameObject platforma;
    public GameObject platformaIn;
    public GameObject platformaOut;
    public float sizeHPlatform = 5;
    public GameObject separator;
    public SpriteRenderer tile;
    public Sprite[] tileset;
    public SpriteRenderer tree;
    public Sprite[] trees;
    public float sizeWTree = 10;

    [Header("Redis")]
    public GameObject coin;
    public GameObject drop;

    [Header("Campfire")]
    public GameObject camp;

    [Header("Player")]
    public Transform player;
    public Text scores;
    public Text coinTxt;
    public Text lvl;
    private int Scores = 0;
    private float pointLevel = 0; 

    void Start()
    {
        startGame();
    }

    void Update()
    {
        if (player.position.y < pointLevel) {
            SinglVar.level++;
            lvl.text = "LvL " + SinglVar.level.ToString();

            GenerateTiles(Maze * SinglVar.level, Cliff, Maze * (SinglVar.level - 1));

            SinglVar.scores += 53;
        }

        if (SinglVar.scores != Scores) {
            if (SinglVar.scores > Scores)
                Scores++;
            else
                Scores = 0;

            scores.text = Scores.ToString();
        }

        if (coinTxt.text != SinglVar.coin.ToString())
            coinTxt.text = SinglVar.coin.ToString();
    }
    /*
    IEnumerator FPS()
    {
        yield return new WaitForSeconds(1);

        int fps = Mathf.FloorToInt(1.0f / Time.deltaTime);

        log.text = "LVL " + level +
            "\nFPS " + fps;

        if (viewLog) {
            StartCoroutine(FPS());
        }
    }
    */
    void startGame()
    {
        DeleteScene();

        GenerateTiles(Maze, Cliff, 0);

        SinglVar.scores = 100;
        SinglVar.level = 1;

        lvl.text = "LvL " + SinglVar.level.ToString();

        Scores = 0;
        scores.text = Scores.ToString();
    }

    void DeleteScene()
    {
        foreach (Transform t in transform) {
            Destroy(t.gameObject);
        }

        foreach (Transform t in es) {
            Destroy(t.gameObject);
        }
    }

    void GenerateTiles(int maze, int cliff, int start)
    {
        pointLevel = -maze * sizeHPlatform + Maze * 2;

        for (int i = start; i < maze; i++) {
            // Отверстие
            int _cliff = Random.Range(-WightMaze + cliff / 2, WightMaze - cliff / 2);
            int treeFront = (int)sizeWTree;
            int treeBack = (int)(sizeWTree / 1.5f);

            bool addCoin = true;
            bool addDrop = true;

            bool addCamp = true;
            
            for (int s = -WightMaze; s <= WightMaze; s++) {
                if (s == _cliff && i < maze) {
                    treeFront = (int)sizeWTree + 1;
                    treeBack = (int)(sizeWTree / 1.5f) + 1;

                    Instantiate(platformaOut, new Vector3(s, -i * sizeHPlatform, 0), Quaternion.identity, transform);
                    tile.sprite = tileset[8];
                    Instantiate(tile, new Vector3(s, -i * sizeHPlatform - 1, 0), Quaternion.identity, transform).name = "tile_" + (i * (int)sizeHPlatform) + "_" + (s + WightMaze);
                    /*
                    for (int v = 0; v < cliff; v++) {
                        // Блок ТОП
                        tile.sprite = tileset[1];
                        Instantiate(tile, new Vector3(s + v, -i * sizeHPlatform, 0), Quaternion.identity, transform).name = "tile_" + (i * (int)sizeHPlatform + 1) + "_" + (s + WightMaze);
                        // Блок НИЗ
                        tile.sprite = tileset[7];
                        Instantiate(tile, new Vector3(s + v, -i * sizeHPlatform - 1, 0), Quaternion.identity, transform).name = "tile_" + (i * (int)sizeHPlatform) + "_" + (s + WightMaze);
                    }
                    */
                    s += cliff - 1;
                    if (s <= WightMaze) {
                        Instantiate(platformaIn, new Vector3(s, -i * sizeHPlatform, 0), Quaternion.identity, transform);
                        tile.sprite = tileset[6];
                        Instantiate(tile, new Vector3(s, -i * sizeHPlatform - 1, 0), Quaternion.identity, transform).name = "tile_" + (i * (int)sizeHPlatform) + "_" + (s + WightMaze);
                    }

                    //   GameObject _respawnBot = Instantiate(RespawnBot, new Vector3(Random.Range(-WightMaze + cliff / 2, WightMaze - cliff / 2), -i * sizeHPlatform + 0.2f, 0), Quaternion.identity) as GameObject;
                    //    _respawnBot.SendMessage("setmaze", WightMaze);
                } else {
                    // Травка
                    Instantiate(platforma, new Vector3(s, -i * sizeHPlatform, 0), Quaternion.identity, transform);
                    tile.sprite = tileset[7];
                    Instantiate(tile, new Vector3(s, -i * sizeHPlatform - 1, 0), Quaternion.identity, transform).name = "tile_" + (i * (int)sizeHPlatform) + "_" + (s + WightMaze);

                    /*
                    // Блок ТОП
                    tile.sprite = tileset[1];
                    Instantiate(tile, new Vector3(s, i * sizeHPlatform, 0), Quaternion.identity).name = "tile_" + (i * (int)sizeHPlatform + 1) + "_" + (s + maze * 4);
                    martix[i * (int)sizeHPlatform + 1][s + maze * 4] = 1;
                    */

                    // Блок НИЗ
                    tile.sprite = tileset[7];
                    Instantiate(tile, new Vector3(s, -i * sizeHPlatform - 1, 0), Quaternion.identity, transform).name = "tile_" + (i * (int)sizeHPlatform) + "_" + (s + WightMaze);
                    //martix[i * (int)sizeHPlatform][s + maze * 4] = 7;

                    if (addCoin)
                        if (Random.Range(0, 10) > 7 && s > 2 && s < WightMaze - 2) {
                            Instantiate(coin, new Vector3(s, -i * sizeHPlatform + 0.3f, 0), Quaternion.identity, es).name = "coin_" + (i * (int)sizeHPlatform) + "_" + (s + WightMaze);
                            addCoin = false;
                        }

                    if (addDrop)
                        if (Random.Range(0, 10) > 5 && s > 2) {
                            Instantiate(drop, new Vector3(s, -i * sizeHPlatform - 0.65f, 0), Quaternion.identity, es).name = "drop_" + (i * (int)sizeHPlatform) + "_" + (s + WightMaze);
                            addDrop = false;
                        }
                    

                    if (addCamp)
                        if (Random.Range(0, 10) > 6 && s > 2) {
                            Instantiate(camp, new Vector3(s, -i * sizeHPlatform - 0.4f, 0), Quaternion.identity, es).name = "camp_" + (i * (int)sizeHPlatform) + "_" + (s + WightMaze);
                            addCamp = false;
                        }

                    // Деревья
                    treeFront--; treeBack--;

                    if (treeBack <= 0) {
                        tree.sprite = trees[Random.Range(0, trees.Length)];
                        tree.transform.localScale = new Vector3(0.75f, 0.75f, 1);
                        tree.color = new Color(0, 0, 0, 0.75f);
                        tree.sortingOrder = -15;
                        Instantiate(tree, new Vector3(s - sizeWTree / 4, -i * sizeHPlatform - 0.4f, 0), new Quaternion(0, Random.value > 0.5f ? 0 : 180, 0, 0), transform);
                        treeBack = (int)(sizeWTree / 1.5f);
                    }

                    if (treeFront <= 0) {
                        tree.sprite = trees[Random.Range(0, trees.Length)];
                        tree.transform.localScale = new Vector3(1, 1, 1);
                        tree.color = Color.white;
                        tree.sortingOrder = -10;
                        Instantiate(tree, new Vector3(s - sizeWTree / Random.Range(2, 3), -i * sizeHPlatform - 0.4f, 0), new Quaternion(0, Random.value > 0.5f ? 0 : 180, 0, 0), transform);
                        treeFront = (int)sizeWTree;
                    }
                }
            }

            if (i % 2 == 0) {
                Instantiate(separator, new Vector3(WightMaze - 1.7f, -i * sizeHPlatform + 5, 0), Quaternion.identity, transform);
                Instantiate(separator, new Vector3(-WightMaze + 1.7f, -i * sizeHPlatform + 5, 0), new Quaternion(0, 180, 0, 0), transform);
            }

            /*
            // Оружие
            gun.sprite = guns[0];
            Vector3 v3m = new Vector3(_cliff - 1, -i * sizeHPlatform, 0);
            SpriteRenderer go_gun = Instantiate(gun, v3m, Quaternion.identity) as SpriteRenderer;
            //go_gun.SendMessage("RadiusForce", gunsForce[0], SendMessageOptions.DontRequireReceiver);
            go_gun.name = "gun_" + i;

            // Тайлы оружия

            tile.sprite = tileset[3];
            v3m.x--;
            Instantiate(tile, v3m, Quaternion.identity);

            tile.sprite = tileset[4];
            v3m.x++;
            Instantiate(tile, v3m, Quaternion.identity);

            tile.sprite = tileset[5];
            v3m.x++;
            Instantiate(tile, v3m, Quaternion.identity);

            tile.sprite = tileset[2];
            v3m.y++;
            Instantiate(tile, v3m, Quaternion.identity);

            tile.sprite = tileset[1];
            v3m.x--;
            Instantiate(tile, v3m, Quaternion.identity);

            tile.sprite = tileset[0];
            v3m.x--;
            Instantiate(tile, v3m, Quaternion.identity);
            */
        }
    }
}
