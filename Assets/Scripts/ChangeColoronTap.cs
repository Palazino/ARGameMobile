using UnityEngine;

public class ChangeColorOnTap : MonoBehaviour
{
    private Renderer objectRenderer;
    private Color[] colors = { Color.red, Color.green, Color.blue, Color.yellow, Color.magenta, Color.cyan };

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform == transform)
                    {
                        ChangeColor();
                    }
                }
            }
        }
    }

    void ChangeColor()
    {
        Color newColor = colors[Random.Range(0, colors.Length)];
        objectRenderer.material.color = newColor;
    }
}
