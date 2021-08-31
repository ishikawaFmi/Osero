using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


public class OseroAi : MonoBehaviour
{
    [SerializeField]
    private OseroContlorer oseroContlorer;
    [SerializeField]
    private Hp enemyHp;
    public const int vertical = 8, horizontal = 8;


    public List<Cell> cellLists2 = new List<Cell>(64);

    public List<Grid> gridLists = new List<Grid>(64);
    public List<Grid> gridLists2 = new List<Grid>(64);
    List<Grid> grids = new List<Grid>(64);
    Cell[] cells = new Cell[64];


    public void MiniMax(List<Grid> grids, Cell[,] cells)
    {

        Cell[,] cell = new Cell[vertical, horizontal];
        for (int i = 0; i < cell.GetLength(0); i++)
        {
            for (int k = 0; k < cell.GetLength(1); k++)
            {
                if (cells[i, k] != null)
                {
                    InstanceCellx(i, k, cells[i, k].oseroState, cells[i, k].cellState, cell);
                }
            }
        }

        int z = 0;
        int score = -100;
        Grid grid;
        bool canPut = true;
        int x = 0;
        for (int i = 0; i < grids.Count; i++)
        {
            if (grids == null)
            {
                canPut = false;
                break;
            }
            cell = CheckA(grids[i].x, grids[i].y, State.MyOsero.orenge, cell);
            gridLists = Sarch(State.MyOsero.green, cell);
            grid = Min(gridLists);
            if (grid == null)
            {
                gridLists2 = Sarch(State.MyOsero.orenge, cell);
            }
            else
            {
                var cell2 = CheckA(grid.x, grid.y, State.MyOsero.green, cell);
                gridLists2 = Sarch(State.MyOsero.orenge, cell2);
            }
            grid = Max(gridLists2);

            if (grid == null)
            {
                x++;
                if (x == grids.Count)
                {
                    canPut = false;
                }
                continue;
            }
            if (grid.score >= score)
            {
                score = grid.score;
                z = i;
            }

        }
        if (canPut)
        {
            if (enemyHp.life <=  enemyHp.Maxlife / 2&&oseroContlorer.enemyMp.mp >= 10)
            {
                oseroContlorer.Put(oseroContlorer.gridLists[z].x, oseroContlorer.gridLists[z].y, State.MyOsero.orenge, State.TypeState.heal);
            }
            else if (Lot(20) && oseroContlorer.enemyMp.mp >= 10)
            {
                oseroContlorer.Put(oseroContlorer.gridLists[z].x, oseroContlorer.gridLists[z].y, State.MyOsero.orenge, State.TypeState.shield);
            }
            else if (Lot(30) && oseroContlorer.enemyMp.mp >= 10)
            {
                oseroContlorer.Put(oseroContlorer.gridLists[z].x, oseroContlorer.gridLists[z].y, State.MyOsero.orenge, State.TypeState.sword);
            }
            else
            {
                oseroContlorer.Put(oseroContlorer.gridLists[z].x, oseroContlorer.gridLists[z].y, State.MyOsero.orenge, State.TypeState.none);
            }
            oseroContlorer.drowCount = 0;
        }
        else if (grids != null)
        {
            for (int i = 0; i < grids.Count; i++)
            {
                if (grids[i].score >= score)
                {
                    score = grids[i].score;
                    z = i;
                }
            }
            if (enemyHp.life >= enemyHp.life / enemyHp.Maxlife)
            {
                oseroContlorer.Put(oseroContlorer.gridLists[z].x, oseroContlorer.gridLists[z].y, State.MyOsero.orenge, State.TypeState.heal);
            }
            else if (Lot(20))
            {
                oseroContlorer.Put(oseroContlorer.gridLists[z].x, oseroContlorer.gridLists[z].y, State.MyOsero.orenge, State.TypeState.shield);
            }
            else if (Lot(30))
            {
                oseroContlorer.Put(oseroContlorer.gridLists[z].x, oseroContlorer.gridLists[z].y, State.MyOsero.orenge, State.TypeState.sword);
            }
            else
            {
                oseroContlorer.Put(oseroContlorer.gridLists[z].x, oseroContlorer.gridLists[z].y, State.MyOsero.orenge, State.TypeState.none);
            }
            oseroContlorer.drowCount = 0;
        }
        else
        {
            Debug.Log("pas");
            oseroContlorer.drowCount++;
        }

    }
    bool Lot(int a)
    {
        int number;
        
        // １から１００までの中からランダムに数字を選択する。
        number = UnityEngine.Random.Range(1, 100);
  
        if (number <= a)
        {
            bool Lot = true;
            return Lot;
        }
        else
        {
            bool Lot = false;
            return Lot;
        }
        
    }
    Grid Min(List<Grid> grids)
    {
        Grid grid = null;
        if (grids.Count <= 1)
        {
            return grid;
        }
        grid = grids[0];
        for (int i = 1; i < grids.Count; i++)
        {
            if (oseroContlorer.fgrid[grids[i].x, grids[i].y].score <= grid.score)
            {
                grid = grids[i];
            }

        }
        return grid;

    }
    Grid Max(List<Grid> grids)
    {
        Grid grid = null;
        if (grids.Count <= 1)
        {
            return grid;
        }
        grid = grids[0];
        for (int i = 1; i < grids.Count; i++)
        {
            if (oseroContlorer.fgrid[grids[i].x, grids[i].y].score >= grid.score)
            {
                grid = grids[i];
            }

        }
        return grid;

    }

    //int ScoreAI(Cell[,] cells)
    //{
    //    a = oseroContlorer.Qqq(State.MyOsero.green, cells) - oseroContlorer.Qqq(State.MyOsero.orenge, cells);
    //    for (int i = 0; i < oseroContlorer.cell.GetLength(0); i++)
    //    {
    //        for (int k = 0; k < oseroContlorer.cell.GetLength(1); k++)
    //        {
    //            if (cells[i, k] != null)
    //            {
    //                if (cells[i, k].oseroState == State.MyOsero.orenge)
    //                {
    //                    orengeB += oseroContlorer.fgrid[i, k].score;
    //                }
    //                else
    //                {
    //                    greenB += oseroContlorer.fgrid[i, k].score;
    //                }

    //            }
    //        }
    //    }
    //    b = greenB - orengeB;
    //    int j = 3;
    //    return aiScore = (int)(a + Mathf.Pow(b, j));
    //}
    public Cell[,] CheckA(int v, int h, State.MyOsero myOsero, Cell[,] cella = null)
    {

        int x = v;
        int y = h;
        bool revese = false;
        for (int i = -1; i <= 1; i++)
        {
            for (int r = -1; r <= 1; r++)
            {
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
                else if (cella[x, y] == null)
                {
                    x = v;
                    y = h;
                    continue;
                }
                else
                {
                    while (cella[x, y].oseroState != myOsero && i != 0 && r != 0)
                    {

                        cells[k] = cella[x, y];
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
                        else if (cella[x, y] == null)
                        {
                            break;
                        }
                        else if (cella[x, y].oseroState == myOsero)
                        {
                            for (int a = 0; a < cells.Length; a++)
                            {
                                if (cells[a] != null)
                                {
                                    cellLists2.Add(cells[a]);
                                }

                            }

                        }
                        k++;
                    }
                    if (x <= -1 || y <= -1 || x >= vertical || y >= horizontal)
                    {
                        break;
                    }
                    if (cella[x, y] == null)
                    {
                        x = v;
                        y = h;
                        continue;
                    }
                    else if (cella[x, y].oseroState == myOsero && revese)
                    {
                        x = v;
                        y = h;

                        continue;
                    }
                    x = v;
                    y = h;
                }


            }

        }

        //めくる処理
        for (int i = 0; i < cellLists2.Count; i++)
        {
            if (cellLists2[i] != null)
            {
                cellLists2[i].oseroState = myOsero;
            }

        }


        // cellLists.ForEach(a => cellLists.Remove(a));
        for (int i = 0; i < cellLists2.Count; i++)
        {
            cellLists2.RemoveAt(i);
        }


        InstanceCellx(v, h, myOsero, State.TypeState.none, cella);

        return cella;



    }
    public List<Grid> Sarch(State.MyOsero myOsero, Cell[,] cells)
    {

        var cels = cells;


        for (int i = 0; i < grids.Count; i++)
        {
            grids.RemoveAt(i);
        }
        for (int v = 0; v < oseroContlorer.grid.GetLength(0); v++)
        {
            for (int h = 0; h < oseroContlorer.grid.GetLength(1); h++)
            {
                if (cels[v, h] == null)
                {
                    if (oseroContlorer.Chek(v, h, myOsero,  State.TypeState.none,cells, true).Item1)
                    {

                        grids.Add(oseroContlorer.fgrid[v, h]);

                    }

                }

            }
        }

        return grids;
    }

    void InstanceCellx(int v, int h, State.MyOsero myOsero, State.TypeState typeState, Cell[,] cells)
    {
        var instanceCell = Instantiate(oseroContlorer.cellObj);

        cells[v, h] = instanceCell.GetComponent<Cell>();
        cells[v, h].oseroState = myOsero;
        cells[v, h].cellState = typeState;
        Destroy(instanceCell);
    }

}
