using Ludus.SDK.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Clique : MonoBehaviour
{
    private RectTransform rt;
    private CanvasGroup grupo;
    private Vector2 posicaoOriginal;
    private new AudioSource audio;
    private Canvas canvas;
    private GameObject sombras;

    public static bool colouCerto;


    //legenda
    public string legenda;

    void Start()
    {
        sombras = GameObject.Find("PainelSombra");
        AjustarCollider();
    }

    void AjustarCollider()
    {
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider == null)
        {
            boxCollider = gameObject.AddComponent<BoxCollider2D>();
        }
        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector2 tamanho = rectTransform.rect.size;
        // Define o tamanho do collider com base nas variáveis largura/altura
        boxCollider.size = tamanho = rectTransform.rect.size;
        boxCollider.offset = rectTransform.rect.center;
    }

    void OnMouseDown()
    {
        
        if (audio.clip != null)
            audio.Play();
        //verifica se tem legenda
        if (Controle.configuracao.temLegendaObjeto)
        {
            try
            {
                if (this.legenda != null && Controle.configuracao.txtLegenda != null)
                {
                    Controle.configuracao.txtLegenda.text = this.legenda;
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Erro ao carregar o objeto de legenda. Está nulo, inexistente ou mal definido na cena.");
                Debug.LogError(ex.GetBaseException());
            }


        }

            Image imgObjeto, imgSombra;
            try
            {
                imgObjeto = gameObject.GetComponent<Image>();
                List<Image> imagens = sombras.GetComponentsInChildren<Image>().ToList<Image>();
                string numeroSpriteObj = imgObjeto.sprite.ToString().Split('_')[1];
                string numeroSpriteSombra;
                for (int i = 0; i < imagens.Count; i++)
                {
                    string nomeSprite = imagens[i].sprite.ToString();
                    if (nomeSprite.Contains("sombra_"))
                    { 
                        numeroSpriteSombra = nomeSprite.Split('_')[1];
                        imgSombra = imagens[i];
                    //se o sprite clicado tem correspondente na sombra
                    if (numeroSpriteObj.Equals(numeroSpriteSombra))
                    {
                        if (Controle.configuracao.substituirObjetoAoParear)
                        {
                            try
                            {
                                List<Image> imgs = gameObject.GetComponentsInChildren<Image>(true).ToList<Image>();
                                imgSombra.sprite = imgs[1].sprite;
                            }
                            catch (System.Exception ex)
                            {

                                Debug.LogError("[+LUDUS] Não achou a imagem para substituir ao parear");
                                Debug.LogError(ex.InnerException);
                            }

                        }
                        else
                        {
                            if (Controle.configuracao.trocarImagemSombraAoClicar)
                            {
                                imgSombra.sprite = imgObjeto.sprite;
                            }
                        }


                        Controle.configuracao.AtualizarAcerto();
                        Controle.configuracao.TocarSom('A');   //caso tenha na pasta de sons, tocar som de acerto
                    }
                    else 
                    {
                        Controle.configuracao.AtualizarErro();
                        Controle.configuracao.TocarSom('E');
                    }
                    break;
                    }
                }
               



            }
            catch (System.Exception ex)
            {

                Debug.LogError("[+LUDUS] Não possível acessar as imagens do objeto pareado e/ou de sua sombra");
                Debug.LogException(ex);
                return;
            }


        Debug.Log("Objeto clicado: " + gameObject.name);
        // Aqui você pode fazer o que quiser: pegar, mover, mudar cor etc.
    }

   

    public void Awake()
    {
        rt = GetComponent<RectTransform>();
        grupo = GetComponent<CanvasGroup>();
        audio = this.GetComponentInChildren<AudioSource>();


        canvas = Controle.configuracao.painelGeral.GetComponent<Canvas>();


        colouCerto = false;
    }
    


    private void Update()
    {
            
    }
}
