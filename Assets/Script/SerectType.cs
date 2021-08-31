using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerectType : MonoBehaviour
{
    public static State.TypeState typeState = State.TypeState.none;

    [SerializeField]
    private OseroContlorer oseroContlorer;

    [SerializeField]
    private GameObject sword;

    [SerializeField]
    private GameObject shield;

    [SerializeField]
    private GameObject heal;

    [SerializeField]
    private GameObject none;

    private void Update()
    {
        switch (typeState)
        {
            case State.TypeState.none:
                none.SetActive(true);
                sword.SetActive(false);
                shield.SetActive(false);
                heal.SetActive(false);
                break;
            case State.TypeState.sword:
                none.SetActive(false);
                sword.SetActive(true);
                shield.SetActive(false);
                heal.SetActive(false);
                break;
            case State.TypeState.shield:
                none.SetActive(false);
                sword.SetActive(false);
                shield.SetActive(true);
                heal.SetActive(false);
                break;
            case State.TypeState.heal:
                none.SetActive(false);
                sword.SetActive(false);
                shield.SetActive(false);
                heal.SetActive(true);
                break;
            default:
                break;
        }
    }
    public void serectSword()
    {
        if (oseroContlorer.playerMp.mp >= 10)
        {
            typeState = State.TypeState.sword;
            Debug.Log(typeState);
        }

    }
    public void serectShield()
    {
        if (oseroContlorer.playerMp.mp >= 10)
        {
            typeState = State.TypeState.shield;
            Debug.Log(typeState);
        }
    }
    public void serectHeal()
    {
        if (oseroContlorer.playerMp.mp >= 10)
        {
            typeState = State.TypeState.heal;
            Debug.Log(typeState);
        }
    }
    public void serectNone()
    {
        typeState = State.TypeState.none;
        Debug.Log(typeState);
    }
    public void serectSkip()
    {
        oseroContlorer.turn = 1;
        oseroContlorer.Xxx(State.MyOsero.orenge, oseroContlorer.cell);
        oseroContlorer.drowCount++;
        Debug.Log("serectSkip");
    }
}
