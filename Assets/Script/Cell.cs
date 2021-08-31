using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
[Serializable]
public class Cell : MonoBehaviour
{
    public State.TypeState cellState = State.TypeState.none;

    public State.MyOsero oseroState = State.MyOsero.none;

    [SerializeField]
    GameObject greenSword;

    [SerializeField]
    GameObject greenShield;

    [SerializeField]
    GameObject greenHeal;

    [SerializeField]
    GameObject orengeSword;

    [SerializeField]
    GameObject orengeShield;

    [SerializeField]
    GameObject orengeHeal;

    [SerializeField]
    GameObject green;

    [SerializeField]
    GameObject orenge;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        switch (cellState)
        {
            case State.TypeState.none:
                greenSword.SetActive(false);
                greenShield.SetActive(false);
                greenHeal.SetActive(false);
                orengeSword.SetActive(false);
                orengeShield.SetActive(false);
                orengeHeal.SetActive(false);
                break;

            case State.TypeState.sword:
                greenSword.SetActive(true);
                greenShield.SetActive(false);
                greenHeal.SetActive(false);
                orengeSword.SetActive(true);
                orengeShield.SetActive(false);
                orengeHeal.SetActive(false);
                break;

            case State.TypeState.shield:
                greenSword.SetActive(false);
                greenShield.SetActive(true);
                greenHeal.SetActive(false);
                orengeSword.SetActive(false);
                orengeShield.SetActive(true);
                orengeHeal.SetActive(false);
                break;

            case State.TypeState.heal:
                greenSword.SetActive(false);
                greenShield.SetActive(false);
                greenHeal.SetActive(true);
                orengeSword.SetActive(false);
                orengeShield.SetActive(false);
                orengeHeal.SetActive(true);
                break;

            default:
                break;
        }
        switch (oseroState)
        {
            case State.MyOsero.none:
                break;
            case State.MyOsero.green:
                green.SetActive(true);
                orenge.SetActive(false);
                break;
            case State.MyOsero.orenge:
                green.SetActive(false);
                orenge.SetActive(true);
                break;
            default:
                break;
        }
    }
}
