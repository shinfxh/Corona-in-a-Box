using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockdownButtonController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Button but = this.GetComponent<Button>();
        but.onClick.AddListener(lockdownAll);
    }

    void lockdownAll()
    {
        GameObject background = GameObject.Find("Background");
        GameController gameController = background.GetComponent<GameController>();
        int n = gameController.n;
        List<GameObject> persons = gameController.persons;
        for (int i = 0; i < n; i++)
        {
            persons[i].GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        }
    }
}
