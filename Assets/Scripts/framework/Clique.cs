using UnityEngine;

public class Clique : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
         
         for(int i = 0;i<10;i=i+2)
             Debug.Log ("i:"+i+"-"+Random.Range(i,i+2));
    }

    void OnMouseDown()
    {
        Debug.Log("Objeto clicado: " + gameObject.name);
        // Aqui você pode fazer o que quiser: pegar, mover, mudar cor etc.
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
