using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuarantineButtonController : MonoBehaviour
{
    public GameObject canvas;
    // Start is called before the first frame update
    void Start()
    {
        Button but = this.GetComponent<Button>();
        but.onClick.AddListener(quarantineSelected);
    }

    void quarantineSelected()
    {
        GameObject background = GameObject.Find("Background");
        GameController gameController = background.GetComponent<GameController>();
        int n = gameController.n;
        List<GameObject> persons = gameController.persons;
        for (int i=0; i<n; i++)
        {
            GameObject person = persons[i];
            if (person.GetComponent<PersonController>().selected == 1)
            {
                person.transform.position = new Vector3(100.0f, 200.0f, 0.0f);
                //person.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                person.GetComponent<PersonController>().selected = 0;
            }
        }
    }
}
