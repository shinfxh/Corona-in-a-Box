using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class PersonController : MonoBehaviour
{
    private Vector3 vel;
    private float v0_max;
    private float v0_min;
    Rigidbody2D rb;
    private Vector2 dir;
    private float speed;
    private Vector2 lastVel;
    public int personIndex;
    public int active;
    public int infected;
    public int uninfected;
    public int recovered;
    public int quarantined;
    public int dead;
    public float infection_time;
    public int selected;
    public float death_limit_time;
    public float recovery_limit_time;
    public float p_death;
    public float p_recovery;
    public float incubation_time;
    private float randNum;
    public int detected;

    public Sprite healthy_img;
    public Sprite healthy_selected_img;
    public Sprite infected_img;
    public Sprite infected_selected_img;
    public Sprite recovered_img;
    public Sprite recovered_selected_img;

    private Sprite person_img;

    Button but;
    ColorBlock cb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        v0_min = 50.0f;
        v0_max = 100.0f;
        speed = Random.Range(v0_min, v0_max);
        rb.velocity = speed * new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
        but = this.GetComponent<Button>();
        selected = 0;
        but.onClick.AddListener(changeSelectedStatus);

        GameObject background = GameObject.Find("Background");
        GameController gameController = background.GetComponent<GameController>();
        death_limit_time = gameController.death_limit_time;
        recovery_limit_time = gameController.recovery_limit_time;
        p_death = gameController.p_death;
        p_recovery = gameController.p_recovery;
        incubation_time = gameController.incubation_time;
        infection_time = 0.0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        lastVel = rb.velocity;
        if (infected == 1)
        {
            infection_time += Time.deltaTime;
            if (infection_time > incubation_time) detected = 1;
            checkDeath();
            checkRecovery();
        }

        if (active == 1)
        {
            cb = but.colors;
            if (infected == 1 && detected==1)
            {
                if (selected == 1) but.GetComponent<Image>().sprite = infected_selected_img;
                else but.GetComponent<Image>().sprite = infected_img;
            }
            else if (recovered==0)
            {
                if (selected == 1) but.GetComponent<Image>().sprite = healthy_selected_img;
                else but.GetComponent<Image>().sprite = healthy_img;
            }
            else
            {
                if (selected == 1) but.GetComponent<Image>().sprite = recovered_selected_img;
                else but.GetComponent<Image>().sprite = recovered_img;
            }
        }
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            Vector2 wall_normal = other.contacts[0].normal;
            rb.velocity = Vector2.Reflect(lastVel, wall_normal);
        }
    }

    void changeSelectedStatus()
    {
        if (selected == 1)
        {
            selected = 0;
        }
        else
        {
            selected = 1;
        }
    }

    void checkDeath()
    {
        if (infection_time > death_limit_time)
        {
            randNum = Random.Range(0.0f, 1.0f);
            if (randNum < p_death)
            {
                Debug.Log("Person " + personIndex + " has died!");
                infected = 0;
                dead = 1;
                active = 0;
                transform.position = new Vector3(0, 0, 0);
                rb.velocity = new Vector2(0, 0);
            }
        }
    }
    void checkRecovery()
    {
        if (infection_time > recovery_limit_time)
        {
            randNum = Random.Range(0.0f, 1.0f);
            if (randNum < p_recovery)
            {
                infected = 0;
                recovered = 1;
                active = 1;
            }
        }
    }
}
