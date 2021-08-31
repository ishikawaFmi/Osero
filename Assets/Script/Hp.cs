using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class Hp : MonoBehaviour
{
    public float Maxlife = 100;
    [SerializeField]
    public float life = 100;

    [SerializeField]
    private Image GreenHp;

    [SerializeField]
    private Image RedHp;

    private Tween redGauge;
   public void GaugeRedurction(float redurctionValue,float time = 1f)
    {
        var valueFrom = life / Maxlife;
        var valueTo = (life - redurctionValue) / Maxlife;

        GreenHp.fillAmount = valueTo;
        if (redGauge != null)
        {
            redGauge.Kill();
        }
        redGauge = DOTween.To(
            () => valueFrom,
            x => { 
                RedHp.fillAmount = x;
            },
            valueTo,
            time
            );

      
    }
}
