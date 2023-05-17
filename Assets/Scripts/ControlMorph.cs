using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlMorph : MonoBehaviour
{
    public enum Forma
    {
        TuberiaA,
        TuberiaB,
        TuberiaC,
        TuberiaD,
        TuberiaE,
        TuberiaF,
        TuberiaG
    };

    public Animator myAnim;
    public int specificFrame;
    public Forma miForma;

    // Start is called before the first frame update
    void Start()
    {
        specificFrame = 5;
        miForma = Forma.TuberiaA;
        myAnim.Play("Change", 0, specificFrame / 35f);
    }

    public void CambiarFrame()
    {
        /*specificFrame += 5;
        miForma++;
        if (specificFrame > 35)
        {
            specificFrame = 5;
            miForma = Forma.TuberiaA;
        }
        myAnim.Play("Change", 0, specificFrame / 35f);*/
        StartCoroutine(Cambio());
    }

    IEnumerator Cambio()
    {
        for (int i = 0; i < 5; i++)
        {
            specificFrame++;
            yield return new WaitForSeconds(.1f);
            myAnim.Play("Change", 0, specificFrame / 35f);
        }
        miForma++;
        if (specificFrame > 35)
        {
            specificFrame = 5;
            miForma = Forma.TuberiaA;
            myAnim.Play("Change", 0, specificFrame / 35f);
        }
    }

    public void SelectFrame(string tamanoEntrada, string tamanoSalida)
    {
        StartCoroutine(Seleccion(tamanoEntrada, tamanoSalida));
    }

    IEnumerator Seleccion(string tamanoEntrada, string tamanoSalida)
    {
        switch (tamanoSalida)
        {
            case "57":
                specificFrame = 20;
                break;
            case "60,3":
                if (tamanoEntrada == "114,3")
                {
                    specificFrame = 5;
                }
                else if (tamanoEntrada == "133")
                {
                    specificFrame = 25;
                }
                break;
            case "76,1":
                if (tamanoEntrada == "114,3")
                {
                    specificFrame = 10;
                }
                else if (tamanoEntrada == "133")
                {
                    specificFrame = 30;
                }
                break;
            case "88,9":
                specificFrame = 15;
                break;
            default:
                break;
        }
        for (int i = 0; i < 5; i++)
        {
            specificFrame++;
            yield return new WaitForSeconds(.1f);
            myAnim.Play("Change", 0, specificFrame / 35f);
        }
    }
}
