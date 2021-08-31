using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField]
    private Hp playerHp;

    [SerializeField]
    private Hp enemyHp;
    [SerializeField]
    private Scene scene;

    
    void Update()
    {
        if (playerHp.life <= 0)
        {
            scene.GameOver();
        }
        if (enemyHp.life <= 0)
        {
            scene.Clear();
        }
        
    }
    public void Judge()
    {
        if (playerHp.life < enemyHp.life)
        {
            scene.GameOver();
        }
        else
        {
            scene.Clear();
        }
    }
}
