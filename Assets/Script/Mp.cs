using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mp : MonoBehaviour
{
    public float maxMp = 100;
    [SerializeField]
    public float mp = 100;

    [SerializeField]
    private Image blueMp;

    [SerializeField]
    private Image orengeMp;

    private Tween redGauge;
    public void GaugeRedurction(float redurctionValue, float time = 1f)
    {
        var valueFrom = mp / maxMp;
        var valueTo = (mp - redurctionValue) / maxMp;

        blueMp.fillAmount = valueTo;
        if (redGauge != null)
        {
            redGauge.Kill();
        }
        redGauge = DOTween.To(
            () => valueFrom,
            x => {
                orengeMp.fillAmount = x;
            },
            valueTo,
            time
            );


    }
}
