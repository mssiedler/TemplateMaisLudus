using UnityEngine;

public class Posicionamento : MonoBehaviour
{
    void Start()
    {
        RectTransform[] filhos = new RectTransform[4];
        Debug.Log(filhos.Length);
        // Pega os primeiros 4 filhos como RectTransform
        for (int i = 0; i < 4 && i < transform.childCount; i++)
        {
            filhos[i] = transform.GetChild(i).GetComponent<RectTransform>();
        }

        // Define os 4 cantos
        SetAnchorAndPivot(filhos[0], new Vector2(0, 1), new Vector2(0, 1)); // Upper Left
        SetAnchorAndPivot(filhos[1], new Vector2(1, 1), new Vector2(1, 1)); // Upper Right
        SetAnchorAndPivot(filhos[2], new Vector2(0, 0), new Vector2(0, 0)); // Lower Left
        SetAnchorAndPivot(filhos[3], new Vector2(1, 0), new Vector2(1, 0)); // Lower Right
    }

    void SetAnchorAndPivot(RectTransform rt, Vector2 anchor, Vector2 pivot)
    {
        if (rt == null) return;

        rt.anchorMin = anchor;
        rt.anchorMax = anchor;
        rt.pivot = pivot;
        rt.anchoredPosition = Vector2.zero;
    }
}