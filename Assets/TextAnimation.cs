using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextAnimation : MonoBehaviour
{
    public GameObject botones;

    public void ActivarBotones()
    {
        botones.SetActive(true);
    }
}
