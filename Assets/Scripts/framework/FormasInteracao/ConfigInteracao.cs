using System;
using UnityEngine;
using UnityEngine.UI;
using static Ludus.SDK.Framework.Configuracao;

public class ConfigInteracao : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Button ArrastaESolta, PegaESolta, CliqueSimples;
    void Start()
    {
        if (PlayerPrefs.HasKey("formainteracao"))
        {
            int escolha = PlayerPrefs.GetInt("formainteracao");
            switch (escolha)
            {
                case 0:
                    ArrastaESolta.GetComponent<Image>().color = Color.green;
                    break;
                case 1:
                    CliqueSimples.GetComponent<Image>().color = Color.green;
                    break;
                case 2:
                    PegaESolta.GetComponent<Image>().color = Color.green;
                    break;
                default:
                    break;
            }
        }
        //parei aqui
        ArrastaESolta.onClick.AddListener(() => SelecionarFormaInteracao(FormasdeInteracao.DragAndDrop));
        CliqueSimples.onClick.AddListener(() => SelecionarFormaInteracao(FormasdeInteracao.DragAndDrop));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SelecionarFormaInteracao(FormasdeInteracao forma) 
    {
        PlayerPrefs.SetInt("formainteracao", Convert.ToInt32(forma));
    }
}
