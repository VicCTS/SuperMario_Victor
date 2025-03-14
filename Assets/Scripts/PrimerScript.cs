using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PrimerScript : MonoBehaviour
{
    // Variables
    public int numeroEntero = 5; //variable para numeros enteros
    private float numeroDecimal = 7.5f; // variable para numeros con decimales
    bool boleana = true; // variable verdadero o falso;
    string cadenaTexto = "Hola Mundo";



    // Start is called before the first frame update
    void Start()
    {
        Calculos();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Calculos()
    {
        Debug.Log(numeroEntero);
        numeroEntero = 17;
        Debug.Log(numeroEntero);

        numeroEntero = 7 + 5;

        numeroEntero++;

        numeroEntero += 2;
    }
}
