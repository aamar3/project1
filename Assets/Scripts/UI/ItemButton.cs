using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject parent = this.transform.parent.gameObject;
        Image image = parent.GetComponent<Image>();

        Image btnImage = this.gameObject.GetComponent<Image>();

        btnImage.sprite = image.sprite;
    }
}
