using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI ;
using System;
using UnityEngine.SceneManagement;
using System.Linq;

public class InterfazUsuario : MonoBehaviour
{
    public Text puntos, fecha;
    public GameObject puntostab;
    public GameObject menu, inicio;
    public GameObject menuGanador;
	public Text textoMenuGanador;
	public Slider sliderDif;
	public Text textoDificultad;
    public GameObject btnsalir;
    bddislexia db = new bddislexia();
    score sc = null;
    bool activa;
    public Canvas canvas;
    public bool menuMostrado;
	public bool menuMostradoGanador;
    public int dificultad = -1, cont = 1;

    public int SegundosCronometro;
	public Text cronometro;
	private TimeSpan tiempo;
    private int segundosTot=0;

	void Start()
    {

        List<score> lscore = db.consultarScore(login.getlog().id, "parejas");


      
        if (lscore.Count() > 0)
        {
            if (lscore[lscore.Count() - 1].pausa == "PAUSA")
            {



                sc = lscore[lscore.Count() - 1];

                string[] values = sc.puntos.Split(':');

                TimeSpan ts = new TimeSpan(int.Parse(values[0]),int.Parse( values[1]),int.Parse(values[2]));
                segundosTot =(int) ts.TotalSeconds;
                dificultad = sc.nivel-2;
                 


            }

        }
        puntostab.SetActive(false);
        btnsalir.SetActive(false);
        loaded();
	}

	public void MostrarMenu(){
		menu.SetActive (true);
		menuMostrado = true;
	}

	public void EsconderMenu(){
		menu.SetActive (false);
		menuMostrado = false;
	}

	public void MostrarMenuGanador(){
		menuGanador.SetActive (true);
		menuMostradoGanador = true;
		textoMenuGanador.text = "HAS GANADO!" + '\n' + "Has encontrado todas las parejas en " + '\n' + tiempo;
        segundosTot += SegundosCronometro;
    }

	public void EsconderMenuGanador(){
		menuGanador.SetActive (false);
		menuMostradoGanador = false;
	}


	public void CambiarDificultad()
    {
        dificultad = dificultad + cont;
    }


	public void ActivarCronometro(){
		ActualizarCronometro ();
	}

	public void ReiniciarCronometro(){
        

		SegundosCronometro = 0;
		CancelInvoke ("ActualizarCronometro");

	}

	public void PausarCronometro(){

	}

	public void ActualizarCronometro(){
		SegundosCronometro++;
		tiempo = new TimeSpan(0,0,  SegundosCronometro);
		cronometro.text = tiempo.ToString();
		Invoke ("ActualizarCronometro", 1.0f);
	}
   
    public void ir()
    {
        btnsalir.SetActive(false);
        inicio.SetActive(false);
    }
    public void loaded()
    {
        if (dificultad < 3)
        {

            CambiarDificultad();

            Debug.Log("disf antes"+dificultad);

            if (dificultad == 2)
            {
                if (sc==null) {
                    TimeSpan temp = new TimeSpan(0, 0, segundosTot);

                    Debug.Log("priemra" + dificultad);
                    sc = new score(0, "" + temp, "" + DateTime.Now, "PAUSA", dificultad);
                    db.GuardarScore(sc, "parejas");
                }
            }
            else if (dificultad > 2)
            {
                Debug.Log("segunda" + dificultad);
                TimeSpan temp = new TimeSpan(0, 0, segundosTot);
                sc.puntos = "" + temp;
                sc.nivel = dificultad;
                sc.pausa = "PAUSA";
                db.Updatescore("parejas", sc);
            }
        }
        else
        {
            Debug.Log("tercera" + dificultad);
            TimeSpan temp = new TimeSpan(0, 0, segundosTot);
            sc.puntos = "" + temp;
            sc.nivel = dificultad;
            sc.pausa = "COMPLETO";
            db.Updatescore( "parejas",sc);

            btnsalir.SetActive(true);
        }
    }

    public void verscore()
    {
        bddislexia db = new bddislexia();
        List<score> lscore = db.consultarScore(login.getlog().id, "parejas");
        string pun = "", fech = "";
        puntostab.SetActive(true);

        foreach (score item in lscore)
        {
            pun += item.puntos + "\n";

            fech += item.fecha + "\n";


        }

        fecha.text = fech;
        puntos.text = pun;



    }

    public void salircore()
    {
        puntostab.SetActive(false);

    }

    public void Salir()
    {
        SceneManager.LoadScene("Principal");
        
    }

    public void salirPausa()
    {
        SceneManager.LoadScene("Principal");
    }

    public void salirescena()
    {
        SceneManager.LoadScene("Principal");
    }

    private void Update()
    {
        if (Input.GetKeyDown("space"))
        {

            activa = !activa;
            canvas.enabled = activa;
            Time.timeScale = (activa) ? 0 : 1f;



        }
    }
}
