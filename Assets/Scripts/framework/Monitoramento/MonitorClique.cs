using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



namespace Ludus.SDK.Framework
{

    public class MonitorClique :
        MonoBehaviour,
        IPointerClickHandler, IDropHandler, IDragHandler
    {
        // private LudusLog log = new LudusLog("monitor-exec", "Execu��o do m�dulo monitor.");
        void Start()
        {
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            string side = "undefined"; // String auxiliar utilizada para facilitar o log.
            Debug.Log("clicou");
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                side = "esquerdo";          // D� o set identificando qual lado do mouse foi clicado.
            }

            else if (eventData.button == PointerEventData.InputButton.Middle)
            {

                side = "do meio";
            }

            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                side = "direito";
            }

            JsonLog.log.addCol(new LudusLogCol("[+LUDUS-mouse-click]:", "Bot�o " + side + " ." + eventData.pointerClick.gameObject.name));
            //  UnityEngine.Debug.Log("[+LUDUS-mouse-click]: O bot�o " + side + "."); // Mostra o log ao usu�rio.
        }

        public void OnDrag(PointerEventData eventData)
        {

            JsonLog.log.addCol(new LudusLogCol("[+LUDUS-drag]:", eventData.pointerDrag.gameObject.name + " " + this.imagemSprite(eventData.pointerDrag.gameObject)));

        }

        public void OnDrop(PointerEventData eventData)
        {
            JsonLog.log.addCol(new LudusLogCol("[+LUDUS-drop]:", eventData.pointerDrag.gameObject.name + " " + this.imagemSprite(eventData.pointerDrag.gameObject)));
        }

        private string imagemSprite(GameObject go)
        {

            Image img = go.GetComponent<Image>();
            if (img != null)
            {
                return img.sprite.ToString();
            }
            else
            {
                return string.Empty;
            }

        }

    }
}
