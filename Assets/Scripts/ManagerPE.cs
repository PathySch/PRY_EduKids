﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ManagerPE : MonoBehaviour
{
    public Text puntos, fecha;
    public GameObject puntostab;
    bddislexia db = new bddislexia();
    public GameObject lettreOne, BoxOne, distrac1, distrac2, distrac3;
    public GameObject winText, btnsiguinte, btnir;

    public Text intentosj, totintentos;

    public GameObject intro;
    public GameObject mensaje, inicio;
    score sc = null;
    bool activa;
    public Canvas canvas;


    private string[] nombresPalabra1 = { "Abeja1", "Araña1", "Cocodrilo1", "Conejo1", "Gallina1", "Hipopotamo1", "Murcielago1", "Oveja1", "Perro1", "Tigre1" };

    private string[] nombresImagen1 = { "Abeja1", "Araña1", "Cocodrilo1", "Conejo1", "Gallina1", "Hipopotamo1", "Murcielago1", "Oveja1", "Perro1", "Tigre1" };



    private string[] nombresPalabraDis1 = { "Aveja", "Arana", "Cocobrilo", "Konejo", "Galina", "Hipopólamo", "Murcélago", "Obeja", "Pero", "Tegri" };
    private string[] nombresPalabraDis2 = { "Avega", "Aroña", "Cucodrilu", "Cunejo", "Gayina", "Hipupótamo", "Murcilagu", "Uveja", "Perruo", "Tigri" };
    private string[] nombresPalabraDis3 = { "Abija", "Eraña", "Cucudrilo", "Cunego", "Gahina", "Hiqopótamo", "Morciélagu", "Ovega", "Pirro", "Tygre" };


    Vector3 lettreOneIni, distrac1Ini, distrac2Ini, distrac3Ini;

    string str = "";
    public string word;

    private Image palabra1;

    private Image distracIma1;
    private Image distracIma2;
    private Image distracIma3;

    public Image imag1;
    public string errores;

    bool oneCorrect = false;

    Vector3 iniScaleLettreOne;

    public AudioSource source;
    public AudioClip correct;
    public AudioClip incorrect;
    public AudioClip gana;

    public AudioClip[] sonido1;
    //public AudioClip[] sonido2;


    int cont = 0;
    int intentos = 0;

    string txtinten = "INTENTOS:";
    System.Random rnd = new System.Random();


    void Start()
    {
        List<score> lscore = db.consultarScore(login.getlog().id, "PalabraEscondida");
        errores = "" + lscore.Count();
        if (lscore.Count() > 0)
        {
            if (lscore[lscore.Count() - 1].pausa == "PAUSA")
            {
                sc = lscore[lscore.Count() - 1];
                cont = sc.nivel;
                intentos = int.Parse(sc.puntos);
                intentosj.text = txtinten + intentos;
            }
        }
        puntostab.SetActive(false);


        palabra1 = lettreOne.GetComponent<Image>();
        palabra1.sprite = Resources.Load<Sprite>("Imagenes/palabras/" + nombresPalabra1[cont]);

        distracIma1 = distrac1.GetComponent<Image>();
        distracIma1.sprite = Resources.Load<Sprite>("Imagenes/distractor/" + nombresPalabraDis1[cont]);

        distracIma2 = distrac2.GetComponent<Image>();
        distracIma2.sprite = Resources.Load<Sprite>("Imagenes/distractor/" + nombresPalabraDis2[cont]);

        distracIma3 = distrac3.GetComponent<Image>();
        distracIma3.sprite = Resources.Load<Sprite>("Imagenes/distractor/" + nombresPalabraDis3[cont]);

        imag1.sprite = Resources.Load<Sprite>("Imagenes/Figuras/" + nombresPalabra1[cont]);
        winText.SetActive(false);
        btnsiguinte.SetActive(true);

        lettreOneIni = lettreOne.transform.position;

        distrac1Ini = distracIma1.transform.position;
        distrac2Ini = distracIma2.transform.position;
        distrac3Ini = distracIma3.transform.position;

        iniScaleLettreOne = lettreOne.transform.localScale;

    }


    public void DragLettreOne()
    {
        lettreOne.transform.position = Input.mousePosition;
    }


    public void Dragdistractor1()
    {
        distrac1.transform.position = Input.mousePosition;
    }


    public void Dragdistractor2()
    {
        distrac2.transform.position = Input.mousePosition;
    }

    public void Dragdistractor3()
    {
        distrac3.transform.position = Input.mousePosition;
    }


    public void DropDistrac1()
    {

        intentos++;

        intentosj.text = txtinten + intentos;
        distrac1.transform.position = distrac1Ini;
        source.clip = incorrect;
        source.Play();
    }

    public void DropDistrac2()
    {

        intentos++;

        intentosj.text = txtinten + intentos;
        distrac2.transform.position = distrac2Ini;
        source.clip = incorrect;
        source.Play();

    }


    public void DropDistrac3()
    {

        intentos++;

        intentosj.text = txtinten + intentos;
        distrac3.transform.position = distrac3Ini;
        source.clip = incorrect;
        source.Play();

    }

    public void DropLettreOne()
    {
        float Distance = Vector3.Distance(lettreOne.transform.position, BoxOne.transform.position);

        intentos++;

        intentosj.text = txtinten + intentos;
        if (Distance < 40 && oneCorrect == false)
        {
            lettreOne.transform.localScale = BoxOne.transform.localScale;
            lettreOne.transform.position = BoxOne.transform.position;
            oneCorrect = true;
            BoxOne.name = lettreOne.name;
            source.clip = sonido1[cont];
            source.Play();
        }

        else
        {
            lettreOne.transform.position = lettreOneIni;
            source.clip = incorrect;
            source.Play();
        }

    }


    public void Reload()
    {
        cont++;
        winText.SetActive(false);
        btnsiguinte.SetActive(false);
        palabra1.sprite = Resources.Load<Sprite>("Imagenes/palabras/" + nombresPalabra1[cont]);

        imag1.sprite = Resources.Load<Sprite>("Imagenes/Figuras/" + nombresPalabra1[cont]);


        distracIma1.sprite = Resources.Load<Sprite>("Imagenes/distractor/" + nombresPalabraDis1[cont]);
        distracIma2.sprite = Resources.Load<Sprite>("Imagenes/distractor/" + nombresPalabraDis2[cont]);
        distracIma3.sprite = Resources.Load<Sprite>("Imagenes/distractor/" + nombresPalabraDis3[cont]);
        str = "";


        oneCorrect = false;
        // twoCorrect = false;


        BoxOne.name = "1";

        lettreOne.transform.position = lettreOneIni;


        lettreOne.transform.localScale = iniScaleLettreOne;

    }

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {

            activa = !activa;
            canvas.enabled = activa;
            Time.timeScale = (activa) ? 0 : 1f;
        }

        if (oneCorrect == true)
        {

            if (cont >= 10)
            {

                totintentos.text = "SUS INTENTOS FUERON:" + intentos;
                winText.SetActive(true);

            }

            btnsiguinte.SetActive(true);

        }
    }


    public void btnbnext()
    {
        if (cont == 10)
        {
            source.clip = gana;
            source.Play();
        }
        if (cont >= 10)
        {
            if (sc != null)
            {

                sc.pausa = "COMPLETADO";
                sc.nivel = cont;
                sc.puntos = "" + intentos;

                db.Updatescore("PalabraEscondida", sc);
            }
            else
            {

                sc = new score(0, "" + intentos, "" + DateTime.Now, "completado", cont);


                db.GuardarScore(sc, "PalabraEscondida");
            }
            SceneManager.LoadScene("Principal");
        }
        else
        {
            Reload();

        }

    }

    public void verscore()
    {
        List<score> lscore = db.consultarScore(login.getlog().id, "PalabraEscondida");
        string pun = "", fech = "";
        puntostab.SetActive(true);

        foreach (score item in lscore)
        {
            int valor = int.Parse(item.puntos);

            valor = 122 - valor;
            if (valor < 0)
            {
                valor = 0;
            }

            if (valor > 100)
            {
                pun += "***% \n";
            }
            else
            {
                pun += valor + " % \n";
            }

            fech += item.fecha + "\n";
        }

        fecha.text = fech;
        puntos.text = pun;
    }

    public void salircore()
    {
        puntostab.SetActive(false);

    }

    public void ir()
    {
        mensaje.SetActive(false);
        inicio.SetActive(false);
    }


    public void Salir()
    {
        SceneManager.LoadScene("Principal");
    }

    public void salirPausa()
    {

        if (sc != null)
        {
            sc.pausa = "PAUSA";
            sc.nivel = cont;
            sc.puntos = "" + intentos;
            db.Updatescore("PalabraEscondida", sc);
        }
        else
        {
            sc = new score(0, "" + intentos, "" + DateTime.Now, "PAUSA", cont);
            db.GuardarScore(sc, "PalabraEscondida");
        }
        SceneManager.LoadScene("PalabraEscondida");
    }


}
