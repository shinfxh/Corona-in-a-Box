using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject personPrefab;
    public GameObject canvas;
    public int n;
    public float w = 700.0f;
    public float h = 400.0f;
    private int choice;
    public List<GameObject> persons = new List<GameObject>();
    public List<Vector3> vel = new List<Vector3>();
    private Button but;
    private ColorBlock cb;
    private float v0_max = 200.0f;
    private float r;
    public float p_infection;
    public float p_death;
    public float p_recovery;
    public float r_infection = 50.0f;
    public int infected_count;
    public float death_limit_time;
    public float recovery_limit_time;
    public float incubation_time;
    private float randNum;


    // Start is called before the first frame update
    void Start()
    {
        p_infection = 0.01f;
        r_infection = 50.0f;
        infected_count = 0;
        death_limit_time = 5.0f;
        recovery_limit_time = 5.0f;
        p_death = 0.0001f;
        p_recovery = 0.0001f;
        incubation_time = 5.0f;

        choice = Random.Range(0, n);
        r = personPrefab.GetComponent<RectTransform>().rect.width;
        for (int i = 0; i < n; i++)
        {
            GameObject a = Instantiate(personPrefab) as GameObject;
            a.name = "Person" + i;
            a.transform.position = new Vector3(Random.Range(-(w/2-r), w/2-r), Random.Range(-(h/2-r), h/2-r), 0);
            a.transform.SetParent(canvas.transform, false);
            PersonController personController = a.GetComponent<PersonController>();
            personController.personIndex = i;
            personController.active = 1;
            personController.dead = 0;
            personController.recovered = 0;
            personController.quarantined = 0;
            personController.infection_time = 0;
            if (i == choice)
            {
                personController.infected = 1;
                personController.uninfected = 0;
                infected_count++;
            }
            else
            {
                personController.infected = 0;
                personController.uninfected = 1;
            }
            persons.Add(a);
            vel.Add(new Vector3(Random.Range(-v0_max, v0_max), Random.Range(-v0_max, v0_max), 0));
        }

        for (int i=0; i<n-1; i++)
        {
            for (int j=i+1; j<n; j++)
            {
                Physics2D.IgnoreCollision(persons[i].GetComponent<Collider2D>(), persons[j].GetComponent<Collider2D>());
            }
        }
    }

    void FixedUpdate()
    {
        CheckInfections();
    }

    void CheckInfections()
    {
        List<GameObject> personsSorted_x = persons;
        personsSorted_x.Sort((p1, p2) => p1.GetComponent<RectTransform>().transform.position.x.CompareTo(p2.GetComponent<RectTransform>().transform.position.x));
        for (int i = 0; i < n; i++)
        {
            PersonController personController = personsSorted_x[i].GetComponent<PersonController>();
            if (personController.infected == 1)
            {
                Vector3 pos = personsSorted_x[i].GetComponent<RectTransform>().transform.position;
                float x0 = pos.x;
                for (int index_l = i - 1; index_l >= 0; index_l--)
                {
                    GameObject next_person = personsSorted_x[index_l];
                    PersonController next_controller = next_person.GetComponent<PersonController>();
                    if (next_controller.uninfected != 1) continue;
                    Vector3 pos1 = next_person.GetComponent<RectTransform>().transform.position;
                    if (Mathf.Abs(pos.x - pos1.x) > r_infection) break;
                    if ((pos1 - pos).magnitude < r_infection)
                    {
                        randNum = Random.Range(0.0f, 1.0f);
                        if (randNum < p_infection)
                        {
                            next_controller.infected = 1;
                            next_controller.uninfected = 0;
                            infected_count++;
                        }
                    }
                }
                for (int index_r = i + 1; index_r < n; index_r++)
                {
                    GameObject next_person = personsSorted_x[index_r];
                    PersonController next_controller = next_person.GetComponent<PersonController>();
                    if (next_controller.uninfected != 1) continue;
                    Vector3 pos1 = next_person.GetComponent<RectTransform>().transform.position;
                    if (Mathf.Abs(pos.x - pos1.x) > r_infection) break;
                    if ((pos1 - pos).magnitude < r_infection)
                    {
                        randNum = Random.Range(0.0f, 1.0f);
                        if (randNum < p_infection)
                        {
                            next_controller.infected = 1;
                            next_controller.uninfected = 0;
                            infected_count++;
                        }
                    }
                }
            }
        }
    }
}