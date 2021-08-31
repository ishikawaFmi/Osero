using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class OseroContlorer : MonoBehaviour
{
    public const int vertical = 8, horizontal = 8;

    const float offset = 2.25f;

    [SerializeField]
    public GameObject cellObj;

    [SerializeField]
    private GameObject gridObj;

    [SerializeField]
    private GameObject gridList;

    [SerializeField]
    private GameObject cellList;

    public int[,] grid = new int[vertical, horizontal];

    public Cell[,] cell = new Cell[vertical, horizontal];

    public int turn = 0;
    [SerializeField]
    private OseroAi oseroAi;

    [SerializeField]
    public Hp playerHp;
    [SerializeField]
    public Hp enemyHp;
    [SerializeField]
    public Mp playerMp;
    [SerializeField]
    public Mp enemyMp;

    [SerializeField]
    private Game game;
    [SerializeField]
    private Text playerTurn;
    [SerializeField]
    private Text enemyTurn;
    public List<Cell> cellLists = new List<Cell>(64);

    Cell[] cells = new Cell[64];
    public List<Grid> gridLists = new List<Grid>(64);

    public Grid[,] fgrid = new Grid[vertical, horizontal];

    int[,] sumi = new int[,] { { 0, 0 }, { 0, 7 }, { 7, 0 }, { 7, 7 } };

    int[,] sumiyoko = new int[,] { { 0, 1 }, { 1, 0 }, { 0, 6 }, { 1, 7 }, { 6, 0 }, { 7, 1 }, { 7, 6 }, { 6, 7 } };

    int[,] z = new int[,] { { 1, 1 }, { 1, 6 }, { 6, 1 }, { 6, 6 } };

    int[,] z16 = new int[,] { { 2, 1 }, { 3, 1 }, { 4, 1 }, { 5, 1 }, { 1, 2 }, { 1, 3 }, { 1, 4 }, { 1, 5 }
    ,{2,6 },{3,6 },{4,6 },{5,6 },{6,2 },{6,3 },{6,4 },{6,5 }};

    int[,] z12 = new int[,] { { 2, 0 }, { 0, 2 }, { 2, 2 }, { 0, 5 }, { 2, 5 }, { 2, 7 }, { 5, 0 }, { 7, 2 }, { 5, 2 }, { 7, 5 }, { 5, 5 }, { 5, 7 } };

    bool a = false;

    public int drowCount = 0;
    void Start()
    {
        for (int v = 0; v < grid.GetLength(0); v++)//オセロのボードを生成
        {
            for (int h = 0; h < grid.GetLength(1); h++)
            {
                var instanceObj = Instantiate(gridObj, new Vector3(offset * v, offset * h, 0), Quaternion.identity);
                var grids = instanceObj.GetComponent<Grid>();
                grids.x = v;
                grids.y = h;
                fgrid[v, h] = grids;


                instanceObj.transform.parent = gridList.transform;

                if (v == 3 && h == 4 || v == 4 && h == 3)//初期セルの生成
                {
                    InstanceCell(v, h, State.MyOsero.green, SerectType.typeState);
                }
                else if (v == 4 && h == 4 || v == 3 && h == 3)
                {
                    InstanceCell(v, h, State.MyOsero.orenge, SerectType.typeState);
                }

            }
        }

        Score();
    }
    void Update()
    {
        int x = 0;
        if (Input.GetKeyDown(KeyCode.A))
        {
            playerHp.GaugeRedurction(10);
            playerHp.life -= 10;

        }
      

        if (Input.GetKeyDown(KeyCode.B) && turn == 1&&a == true)
       {
            Resources.UnloadUnusedAssets();
            oseroAi.MiniMax(gridLists,cell);
            Xxx(State.MyOsero.orenge, cell);       
            turn = 0;
            a = false;
            Debug.Log(turn);
        }
        else if (turn == 0&&a == false)
        {
            Xxx(State.MyOsero.green, cell);
            a = true;
        }
        if (drowCount == 2)
        {
            game.Judge();
        }
        for (int i = 0; i < cell.GetLength(0); i++)
        {
            for (int k = 0; k < cell.GetLength(1); k++)
            {
                if (cell[i,k] != null)
                {
                    x++;
                    if(x == 64)
                    {
                        game.Judge();
                    }
                }
            }
        }
        if (turn == 0)
        {
            playerTurn.enabled = true;
            enemyTurn.enabled = false;
        }
        else
        {
            playerTurn.enabled = false;
            enemyTurn.enabled = true;
        }
    }
    /// <summary>
    /// もし置けるなら置く
    /// </summary>
    /// <param name="v">置く場所のX軸</param>
    /// <param name="h">置く場所のY軸</param>
    /// <param name="myOsero">セルの色</param>
    public void Put(int v, int h, State.MyOsero myOsero, State.TypeState typeState)
    {
        if (Chek(v, h, myOsero, typeState).Item1)
        {
            InstanceCell(v, h, myOsero, typeState);
            if (typeState != State.TypeState.none)
            {
                if (turn == 0)
                {
                    playerMp.GaugeRedurction(10);
                    playerMp.mp -= 10;
                }
                else
                {
                    enemyMp.GaugeRedurction(10);
                    enemyMp.mp -= 10;
                }
            }
            if (turn == 0)
            {
                Resources.UnloadUnusedAssets();
                turn = 1;
                Xxx(State.MyOsero.orenge, cell);
                drowCount = 0;
                SerectType.typeState = State.TypeState.none;
            }
           
            Debug.Log(turn);
        }


    }
    /// <summary>
    /// セルの生成
    /// </summary>
    /// <param name="v">置く場所のX軸</param>
    /// <param name="h">置く場所のY軸</param>
    /// <param name="myOsero">セルの色</param>
    /// <param name="typeState">属性</param>
    void InstanceCell(int v, int h, State.MyOsero myOsero, State.TypeState typeState)
    {
        var instanceCell = Instantiate(cellObj, new Vector3(offset * v, offset * h, 0), Quaternion.identity);
        instanceCell.transform.parent = cellList.transform;
        cell[v, h] = instanceCell.GetComponent<Cell>();
        cell[v, h].oseroState = myOsero;
        cell[v, h].cellState = typeState;
    }
    /// <summary>
    /// おけるかどうかの判定
    /// </summary>
    /// <param name="v">置く場所のX軸</param>
    /// <param name="h">置く場所のY軸</param>
    /// <param name="myOsero">セルの色</param>
    /// <returns>おけるならtrueを返す</returns>
    public (bool, Cell[,]) Chek(int v, int h, State.MyOsero myOsero,State.TypeState typeState=State.TypeState.none, Cell[,] cella = null, bool not = false)
    {
        bool chek = false;
        var cellb = cell;
       

        int x = v;
        int y = h;
        bool revese = false;
        bool sheld = false;
        
        for (int i = -1; i <= 1; i++)
        {
            for (int r = -1; r <= 1; r++)
            {
                if (i==0&&r==0)
                {
                    continue;
                } 
                x += i;
                y += r;
                revese = false;
                int k = 0;
                for (int z = 0; z < cells.Length; z++)
                {
                    cells[z] = null;
                }
                if (x <= -1 || y <= -1 || x >= vertical || y >= horizontal)
                {
                    x = v;
                    y = h;
                    continue;
                }
                else if (cellb[x, y] == null)
                {
                    x = v;
                    y = h;
                    continue;
                }
                else
                {
                    while (cellb[x, y].oseroState != myOsero)
                    {
                        cells[k] = cellb[x, y];
                        revese = true;
                        if (x <= -1 || y <= -1 || x >= vertical || y >= horizontal)
                        {
                            break;
                        }
                        else
                        {
                            x += i;
                            y += r;
                        }
                        if (x <= -1 || y <= -1 || x >= vertical || y >= horizontal)
                        {
                            break;
                        }
                        else if (cellb[x, y] == null)
                        {
                            break;
                        }
                        else if (cellb[x, y].oseroState == myOsero)
                        {
                            for (int a = 0; a < cells.Length; a++)
                            {
                                if (cells[a] != null)
                                {
                                    cellLists.Add(cells[a]);
                                }

                            }
                           
                        }
                        k++;
                    }
                    if (x <= -1 || y <= -1 || x >= vertical || y >= horizontal)
                    {
                        break;
                    }
                    if (cellb[x, y] == null)
                    {
                        x = v;
                        y = h;
                        continue;
                    }
                    else if (cellb[x, y].oseroState == myOsero && revese)
                    {
                        x = v;
                        y = h;
                        chek = true;
                        continue;
                    }
                    x = v;
                    y = h;
                }


            }

        }
        if (not)
        {
            for (int i = 0; i < cellLists.Count; i++)
            {
                cellLists.RemoveAt(i);
            }
            return (chek, cellb);
        }
        else
        {
            //めくる処理
            for (int i = 0; i < cellLists.Count; i++)
            {
                if (cellLists[i] != null)
                {
                    if (cellLists[i].cellState == State.TypeState.shield)
                    {

                        sheld = true;

                    }
                }
            }
            Attak(cellLists, myOsero,typeState, sheld,turn);
            for (int i = 0; i < cellLists.Count; i++)
            {
                cellLists.RemoveAt(i);
            }
            return (chek, cellb);
        }


    }
    void Attak(List<Cell> cells, State.MyOsero myOsero, State.TypeState typeState, bool sheld,int turn)
    {
        Hp hp = playerHp;
        if (turn == 0)
        {
            hp = enemyHp;
        }

        for (int i = 0; i < cells.Count; i++)
        {
            if (cells[i] == null)
            {
                continue;
            }
                cells[i].oseroState = myOsero;
            if (typeState == State.TypeState.sword)
            {
                if (sheld)
                {
                    hp.GaugeRedurction(5);
                    hp.life -= 5;

                }
                else
                {
                    hp.GaugeRedurction(10);
                    hp.life -= 10;
                }
            }
            else
            {
                if (sheld)
                {
                    hp.GaugeRedurction(3);
                    hp.life -= 3;

                }
                else
                {
                    hp.GaugeRedurction(5);
                    hp.life -= 5;
                }

            }
            if (typeState == State.TypeState.heal)
            {
                if (turn == 0)
                {
                    playerHp.GaugeRedurction(-5);
                    playerHp.life += 5;
                }
                else
                {
                    enemyHp.GaugeRedurction(-5);
                    enemyHp.life += 5;
                }
               
            }
        }

       

      
        

    }
    public int Xxx(State.MyOsero myOsero, Cell[,] cells = null)
    {
        int x = 0;
        var cels = cell;

        for (int z = 0; z < gridLists.Count; z++)
        {
            gridLists.RemoveAt(z);
        }
        if (cells != null)
        {
            cels = cells;
        }


        for (int v = 0; v < grid.GetLength(0); v++)
        {
            for (int h = 0; h < grid.GetLength(1); h++)
            {
                if (cels[v, h] == null)
                {
                    if (Chek(v, h, myOsero, State.TypeState.none, null, true).Item1)
                    {
                            x++;                
                            fgrid[v, h].chek.SetActive(true);                                     
                    }
                    else
                    {
                        fgrid[v, h].chek.SetActive(false);                    
                    }
                }
                else 
                {
                    fgrid[v, h].chek.SetActive(false);
                }

            }
        }
        for (int q = 0; q < fgrid.GetLength(0); q++)
        {
            for (int w = 0; w < fgrid.GetLength(1); w++)
            {
                if (fgrid[q, w].chek.activeSelf == true)
                {
                    for (int i = 0; i < gridLists.Count; i++)
                    {
                        if (gridLists[i].x == q&& gridLists[i].y == w)
                        {
                            gridLists.Remove(fgrid[q, w]);
                        } 
                    }
                    gridLists.Add(fgrid[q, w]);
                }
                else
                {
                    gridLists.Remove(fgrid[q, w]);
                }
            }
        }
        return x;
    }
    public int Qqq(State.MyOsero myOsero, Cell[,] cells = null)
    {
        int x = 0;
        var cels = cell;
        if (cells != null)
        {
            cels = cells;
        }


        for (int v = 0; v < grid.GetLength(0); v++)
        {
            for (int h = 0; h < grid.GetLength(1); h++)
            {
                if (cels[v, h] == null)
                {
                    if (Chek(v, h, myOsero, State.TypeState.none, null, true).Item1)
                    {
                        x++;
                      
                    }
                   
                }
               

            }
        }

        return x;
    }
   
    void Score()
    {


        for (int i = 0; i <= 3; i++)
        {
            for (int k = 0; k <= 1; k++)
            {
                int a = sumi[i, 0];
                if (k == 1)
                {
                    int b = sumi[i, k];

                    fgrid[a, b].score = 30;

                }
            }
        }
        for (int i = 0; i <= 7; i++)
        {
            for (int k = 0; k <= 1; k++)
            {
                int a = sumiyoko[i, 0];
                if (k == 1)
                {
                    int b = sumiyoko[i, k];

                    fgrid[a, b].score = -12;

                }
            }
        }
        for (int i = 0; i <= 3; i++)
        {
            for (int k = 0; k <= 1; k++)
            {
                int a = z[i, 0];
                if (k == 1)
                {
                    int b = z[i, k];

                    fgrid[a, b].score = -15;

                }
            }
        }
        for (int i = 0; i <= 15; i++)
        {
            for (int k = 0; k <= 1; k++)
            {
                int a = z16[i, 0];
                if (k == 1)
                {
                    int b = z16[i, k];

                    fgrid[a, b].score = -3;

                }
            }
        } 
        for (int i = 0; i <= 11; i++)
        {
            for (int k = 0; k <= 1; k++)
            {
                int a = z12[i, 0];
                if (k == 1)
                {
                    int b = z12[i, k];

                    fgrid[a, b].score = 0;

                }
            }
        }

 
    }
   
}





