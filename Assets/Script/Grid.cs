using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public int x = 0;
    public int y = 0;

    private OseroContlorer oseroContlorer;

    public GameObject chek = null;

    public int score = -1;
    void Start()
    {
        oseroContlorer = GameObject.Find("GameManeger").GetComponent<OseroContlorer>();
        chek.SetActive(false);
    }
    public void OnClick()
    {
        if (oseroContlorer.turn == 0)
        {
            oseroContlorer.Put(x, y,State.MyOsero.green,SerectType.typeState);
        }
      
        
    }
}
