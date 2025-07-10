using Ludus.SDK.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Draggeable : MonoBehaviour, IPointerClickHandler
{
    private bool isFollowing = false;
    private RectTransform rectTransform;
    private Canvas canvas;
    private Vector2 cursorHotspot;
    private Vector3 startPosition;
    private Vector2 dragOffset;

    [SerializeField] public RectTransform targetArea;
    [SerializeField] public Texture2D CursorSolto;
    [SerializeField] public Texture2D CursorClicado;

    private RectTransform rt;
    private CanvasGroup grupo;
   
    private new AudioSource audio;
    private GameObject sombras;

   

    public static bool colouCerto;

    // legenda
    public string legenda;
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        startPosition = transform.position;
        rt = GetComponent<RectTransform>();
        grupo = GetComponent<CanvasGroup>();
        audio = GetComponentInChildren<AudioSource>();
        colouCerto = false;
        
    }

    void Start()
    {
        

        cursorHotspot = new Vector2(CursorSolto.width / 2, CursorSolto.height / 2);
        Cursor.SetCursor(CursorSolto, cursorHotspot, CursorMode.Auto);

        sombras = GameObject.Find("PainelSombra"); // certifique-se de que o objeto exista
       
    }

    void Update()
    {
        
        if (isFollowing)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            RectTransform parentRect = rectTransform.parent as RectTransform;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parentRect,
                mousePos,
                canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
                out Vector2 localPoint
            );

            rectTransform.anchoredPosition = localPoint - dragOffset;
        }
       
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        isFollowing = !isFollowing;

        if (isFollowing)
        {
            // Início do arrasto
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rectTransform.parent as RectTransform,
                Mouse.current.position.ReadValue(),
                canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
                out Vector2 localPoint
            );

            dragOffset = localPoint - rectTransform.anchoredPosition;

            if (CursorClicado != null)
                Cursor.SetCursor(CursorClicado, cursorHotspot, CursorMode.Auto);
        }
        else
        {
            // Fim do arrasto
            Vector2 posicaoClique = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D[] coliders = Physics2D.OverlapPointAll(posicaoClique);

            // ✅ CORRIGIDO: garantir retorno à posição inicial se nenhuma sombra for atingida
            if (coliders.Length == 0)
            {
               
                transform.position = startPosition;
                isFollowing = false;
                Cursor.SetCursor(CursorSolto, cursorHotspot, CursorMode.Auto);
                return; // impede execução desnecessária
            }

            if (CursorSolto != null)
                Cursor.SetCursor(CursorSolto, cursorHotspot, CursorMode.Auto);

            if (audio != null && audio.clip != null)
                audio.Play();

            // legenda
            if (Controle.configuracao.temLegendaObjeto)
            {
                try
                {
                    if (!string.IsNullOrEmpty(this.legenda) && Controle.configuracao.txtLegenda != null)
                        Controle.configuracao.txtLegenda.text = this.legenda;
                }
                catch (System.Exception ex)
                {
                    Debug.LogError("Erro ao carregar a legenda.");
                    Debug.LogError(ex.GetBaseException());
                }
            }

            // ✅ Lógica de pareamento com a sombra
            try
            {
                Image imgObjeto = gameObject.GetComponent<Image>();
                List<Image> imagens = sombras.GetComponentsInChildren<Image>().ToList();

                string numeroSpriteObj = imgObjeto.sprite.name.Split('_')[1];

                foreach (var imgSombra in imagens)
                {
                    if (imgSombra.sprite.name.Contains("sombra_"))
                    {
                        string numeroSpriteSombra = imgSombra.sprite.name.Split('_')[1];

                        if (numeroSpriteObj.Equals(numeroSpriteSombra))
                        {
                            if (Controle.configuracao.substituirObjetoAoParear)
                            {
                                try
                                {
                                    List<Image> imgs = gameObject.GetComponentsInChildren<Image>(true).ToList();
                                    imgSombra.sprite = imgs[1].sprite;
                                }
                                catch (System.Exception ex)
                                {
                                    Debug.LogError("[+LUDUS] Erro ao substituir sprite.");
                                    Debug.LogError(ex.InnerException);
                                }
                            }
                            else if (Controle.configuracao.trocarImagemSombraAoClicar)
                            {
                                imgSombra.sprite = imgObjeto.sprite;
                            }

                            Controle.configuracao.AtualizarAcerto();
                            Controle.configuracao.TocarSom('A');
                            colouCerto = true;
                        }
                        else
                        {
                            Controle.configuracao.AtualizarErro();
                            Controle.configuracao.TocarSom('E');
                            colouCerto = false;
                        }

                        break;
                    }
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError("[+LUDUS] Erro ao acessar imagens para pareamento.");
                Debug.LogException(ex);
            }

            Debug.Log("Objeto clicado: " + gameObject.name);
        }
    }
}
