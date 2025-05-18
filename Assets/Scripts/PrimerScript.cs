using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimerScript : MonoBehaviour
{
    // Variables
    public int numeroEntero = 5; //variable para numeros enteros
    private float numeroDecimal = 7.5f; // variable para numeros con decimales
    bool boleana = true; // variable verdadero o falso;
    string cadenaTexto = "Hola Mundo";

    private int[] numeros = {75, 1, 3};

    public int[] numeros2;
    private int[,] numeros3 = {{7, 8, 98}, {9, 22, 45}, {74, 5, 6}};

    List<int> listaDeNumeros = new List<int> {3, 5, 8, 9, 88, 12};

    // Start is called before the first frame update
    void Start()
    {
        foreach(int numero in listaDeNumeros)
        {
            Debug.Log(numero);
        }

        listaDeNumeros.Add(22);
        listaDeNumeros.Remove(5);
        listaDeNumeros.RemoveAt(0);
        //listaDeNumeros.RemoveAt(listaDeNumeros.Count - 1);
        //listaDeNumeros.Clear();
        listaDeNumeros.Sort();
        listaDeNumeros.Reverse();

        foreach(int numero in listaDeNumeros)
        {
            Debug.Log(numero);
        }

      

        /*foreach(int numero in numeros)
        {
            Debug.Log(numero);
        }*/

        /*for (int i = 0; i < numeros.Length; i++)
        {
            Debug.Log(numeros[i]);
        }*/

        /*int i = 75;
        while(i < numeros.Length)
        {
            Debug.Log(numeros[i]);
            i++;
        }*/

        /*int i = 75;
        do 
        {
            Debug.Log("asfafs");
        }
        while (i < numeros.Length);*/



        //Debug.Log(numeros[0]);
        //Debug.Log(numeros3[1,2]);

        //Calculos();

        
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
