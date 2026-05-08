using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element_controler : MonoBehaviour
{
    public GameObject Fire;
    public GameObject Earth;
    public GameObject Ice;
    
    public GameObject Blackness;

    public GameObject A1;
    public GameObject A2;
    public GameObject A3;

    private Renderer Fire_renderer;
    private Renderer Earth_renderer;
    private Renderer Ice_renderer;
    private Renderer Dark_renderer;

    void Start()
    {
        Fire.SetActive(false);
        Fire_renderer = A1.GetComponent<Renderer>();

        Earth.SetActive(false);
        Earth_renderer = A2.GetComponent<Renderer>();

        Ice.SetActive(false);
        Ice_renderer = A3.GetComponent<Renderer>();

        Blackness.SetActive(false);
    }
    void Update()
    {
        if(Fire_renderer.isVisible)
        {
            Fire.SetActive(true);
            Fire_renderer = Fire.GetComponent<Renderer>();
        }

        if(Earth_renderer.isVisible)
        {
            Earth.SetActive(true);
            Earth_renderer = Earth.GetComponent<Renderer>();
        }

        if(Ice_renderer.isVisible)
        {
            Ice.SetActive(true);
            Ice_renderer = Ice.GetComponent<Renderer>();
        }

        if(Ice_renderer.isVisible && Earth_renderer.isVisible && Fire_renderer.isVisible)
        {
            Blackness.SetActive(true);
            Ice.SetActive(false);
            Fire.SetActive(false);
            Earth.SetActive(false); 
        }
    }
}
