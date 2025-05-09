using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


namespace Ludus.SDK.Framework
{
    public class Fase : MonoBehaviour
    {
        // Start is called before the first frame update

        private GameObject objeto;
        private GameObject sombra;
        private List<Image> imgsObjeto;
        private List<Image> imgsSombra;
        private List<AudioSource> audiosSourceObjetos;
        private List<Sprite> spritesObjeto;
        private List<Sprite> spritesSombra;
        private List<AudioClip> audiosObjeto;
        public string pasta;

        void Start()
        {
            try
            {
                objeto = GameObject.Find("PainelObjeto");
                sombra = GameObject.Find("PainelSombra");
            }
            catch (System.Exception ex)
            {

                Debug.LogError("[+LUDUS] Erro ao instanciar as vari�veis, verifique se os pain�is objeto e sombra est�o colocados adequadamente");
                Debug.LogException(ex);
                return;
            }
            try
            {

                imgsObjeto = objeto.GetComponentsInChildren<Image>().ToList<Image>();
                imgsSombra = sombra.GetComponentsInChildren<Image>().ToList<Image>();

                //lista de elementos de �udio
                audiosSourceObjetos = objeto.GetComponentsInChildren<AudioSource>().ToList<AudioSource>();




            }
            catch (System.Exception ex)
            {

                Debug.LogError("[+LUDUS] Erro ao buscar as imagens dentro dos pain�is 'sombra e/ou objeto', verifique se as imagens est�o corretamente dispostas na hierarquia de elemetos");
                Debug.LogException(ex);
                return;
            }



            if (string.IsNullOrEmpty(pasta))
            {

                Debug.LogError("[+LUDUS] Vari�vel pasta n�o defindida, verifique as vari�veis do script e adicione a pasta corresponde aos sprites. Dica: procure na pasta Resources/fases e busque O NOME DA FASE DESEJADA");
                return;
            }

            try
            {
                //Sprite[] 
                spritesObjeto = Resources.LoadAll<Sprite>("fases/" + pasta + "/objeto").ToList<Sprite>();
                spritesSombra = Resources.LoadAll<Sprite>("fases/" + pasta + "/sombra").ToList<Sprite>();

            }
            catch (System.Exception ex)
            {

                Debug.LogError("[+LUDUS] Sprites n�o encontrados. Verifique se a pasta fases/" + pasta + "/objeto e fases/" + pasta + "/sombra existem dentro da pasta Resources, e se os os sprites est�o corretamente divididos (multiple)");
                Debug.LogException(ex);
                return;
            }
            //carregar audios do objeto
            try
            {
                audiosObjeto = Resources.LoadAll<AudioClip>("fases/" + pasta + "/objetoSons").ToList<AudioClip>();
            }
            catch (System.Exception ex)
            {

                Debug.LogError("[+LUDUS] �udios n�o encontrados. Verifique se a pasta fases/" + pasta + "/objetoSons existe dentro da pasta Resources, e se os os audios est�o corretamente colocados");
                Debug.LogException(ex);
            }

            if (spritesObjeto.IsUnityNull())
            {

                Debug.LogError("[+LUDUS] spritesObjeto n�o definido");
                return;
            }
            if (spritesSombra.IsUnityNull())
            {

                Debug.LogError("[+LUDUS] spritesSombra n�o definido");
                return;
            }

            //cria a lista dos sprites que foram selecionado para 
            //depois usar na sombra
            List<int> indiceSelecionado = new List<int>();


            for (int i = 0; imgsObjeto.Count > i; i++)
            {

                int selecionado = this.indiceNovo(indiceSelecionado);
                imgsObjeto[i].sprite = spritesObjeto[selecionado];

                try
                {
                    audiosSourceObjetos[i].GetComponent<AudioSource>().clip = audiosObjeto[selecionado];
                }
                catch (System.Exception ex)
                {
                    Debug.LogError("[+LUDUS] erro ao carregar o �udio correspondente ao sprite. Verifique os arquivos na pasta(quantidade e nome, sugerece que os audios sejam dispostos com o _n�mero correto. Por exemplo audio_0, audio_1,...)");
                    Debug.LogException(ex);
                }



            }

            this.embaralharLista(indiceSelecionado);
            for (int i = 0; imgsSombra.Count > i; i++)
            {
                imgsSombra[i].sprite = spritesSombra[indiceSelecionado[i]];
            }






        }

        // Update is called once per frame
        void Update()
        {

        }

        private int indiceNovo(List<int> indiceSelecionado)
        {
            int novoindice;
            novoindice = Random.Range(0, spritesObjeto.Count - 1);
            while (indiceSelecionado.Contains(novoindice))
            {
                novoindice = Random.Range(0, spritesObjeto.Count - 1);
            }
            indiceSelecionado.Add(novoindice);
            return novoindice;

        }

        private void embaralharLista<T>(List<T> lista)
        {


            int n = lista.Count;
            for (int i = n - 1; i > 0; i--)
            {
                // Gera um �ndice aleat�rio no intervalo [0, i]
                int indiceAleatorio = Random.Range(0, i + 1);

                // Troca os elementos nas posi��es i e indiceAleatorio
                T temp = lista[i];
                lista[i] = lista[indiceAleatorio];
                lista[indiceAleatorio] = temp;
            }
        }
    }

}
