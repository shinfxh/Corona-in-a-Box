using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestingButtonController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Button but = this.GetComponent<Button>();
        but.onClick.AddListener(testAll);
    }

    void testAll()
    {
        GameObject background = GameObject.Find("Background");
        GameController gameController = background.GetComponent<GameController>();
        int n = gameController.n;
        List<GameObject> persons = gameController.persons;
        for (int i=0; i<n; i++)
        {
            GameObject person = persons[i];
            PersonController personController = person.GetComponent<PersonController>();
            if (personController.infected == 1) personController.detected = 1;
        }
    }
}
