using System.Collections;
using UnityEngine;

public class CursorAnimacao : MonoBehaviour
{
    
    public float intervalo = 5f; // tempo em segundos

    void Start()
    {
        StartCoroutine(TrocarEstado());
    }

    IEnumerator TrocarEstado()
    {
        while (true)
        {
            // inverte o estado atual (ativo/inativo)
            this.gameObject.SetActive(!this.gameObject.activeSelf);

            // espera o intervalo definido
            yield return new WaitForSeconds(intervalo);
        }
    }
}
