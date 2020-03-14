using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float maxhp, damage, movespeed = 1.2f;
    public float hp;
    [SerializeField]private bool following = false, attacking = false, delay = false;
    private GameObject player;
    public List<GameObject> drop;
    public string playersCommentary = "Одним меньше";
    protected void Start()
    {
        hp = maxhp;
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    protected void FixedUpdate()
    {
        if (following)
        {
            Follow();
        }

        if (hp <= 0)
        {
            if (drop.Count != 0)
            {
                int i = 0;
                foreach (var item in drop)
                {
                    ++i;
                    Instantiate(item, new Vector3(this.transform.position.x + RandomNonNullGenerator() * Mathf.Sqrt(i) * Mathf.Sign((float)i % 2 - 0.5f), this.transform.position.y + RandomNonNullGenerator() * Mathf.Sqrt(i) * Mathf.Sign((float)i % 2 - 0.5f)/10f, 15f), Quaternion.Euler(0f, 0f, 0f));
                }
            }
            Death();
        }
    }

    protected void Update()
    {
        
        if (Vector3.Magnitude(new Vector3(player.transform.position.x - this.transform.position.x,player.transform.position.y-this.transform.position.y,0f)) < 0.6f)
        {
            player.GetComponent<Rigidbody2D>().AddForce(-300f * new Vector2((transform.position.x - player.transform.position.x),(transform.position.y - player.transform.position.y)).normalized);
            player.transform.GetComponentInChildren<Condition>().hp -= damage * 0.01f * (100f - player.transform.GetComponentInChildren<Condition>().armor);
        }
    }

    protected void Follow()
    { 
        // поворот + transform.Translate(new Vector3(0f,speed),Space.Self);
        if (Vector3.Magnitude(player.transform.position - transform.position) > GetComponent<CircleCollider2D>().radius * 5f)
        {
            transform.Translate((player.transform.position - transform.position).normalized / (2000f/movespeed), Space.World);
            attacking = false;
        }
        else
        {
            attacking = true;
            transform.position = Vector3.Lerp(this.transform.position, player.transform.position, 0.003f);
        }
    }

    protected void Death()
    {
        player.GetComponentInChildren<Condition>().exp += 15;
        player.GetComponentInChildren<Reaction>().SetReplics(playersCommentary);
        Destroy(gameObject);
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        // обнаружение
        if (collision.tag == "Player")
        {
            following = true;
            Destroy(GetComponent<BoxCollider2D>());
        }
    }

    protected void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (attacking && !delay)
            {
                delay = true;
                StartCoroutine("Delay");
                collision.transform.GetComponentInChildren<Condition>().hp -= damage * 0.01f * (100f-collision.transform.GetComponentInChildren<Condition>().armor);
                collision.GetComponent<Rigidbody2D>().AddForce(new Vector2(-400f * (transform.position.x - collision.transform.position.x), -400f * (transform.position.y - collision.transform.position.y)));
            }
        }
    }

    protected IEnumerator Delay()
    {
        yield return new WaitForSeconds(1f);
        delay = false;
    }

    // метод для подсчитывания угла по всей тангенциальной окружности. Vector3.Angle возвращает лишь в промежутке от 0 до Пи
    protected float AngleBetwVector3by360degrees(Vector3 v1, Vector3 v2)
    {
        Vector3 resultVector = v2 - v1;
        if(resultVector.x>0 && resultVector.y > 0)
        {
            return Mathf.Rad2Deg * (Mathf.Atan2(resultVector.y, resultVector.x) - Mathf.PI/2);
        }
        else if(resultVector.x<0 && resultVector.y > 0)
        {
            return Mathf.Rad2Deg * (Mathf.Atan2(resultVector.y,resultVector.x) + Mathf.PI * 1.5f);
        }
        else if(resultVector.x<0 && resultVector.y < 0)
        {
            return Mathf.Rad2Deg * (Mathf.Atan2(resultVector.y, resultVector.x) + Mathf.PI * 1.5f);
        }
        else
        {
            return Mathf.Rad2Deg * (Mathf.Atan2(resultVector.y, resultVector.x) +  3 * Mathf.PI / 2);
        }
    }

    protected void OnGUI()
    {
        if (this.GetComponent<BoxCollider2D>()==null)
        {
            var onScreenPoint = Camera.main.WorldToScreenPoint(transform.position);
            GUI.color = Color.red;
            int percent = (int)(100 * hp / maxhp);
            GUI.Box(new Rect(onScreenPoint.x * 1.03f, Screen.height - onScreenPoint.y, percent+10f, 20f), hp.ToString());
        }
    }

    public float RandomNonNullGenerator()
    {
        System.Random rand = new System.Random();
        return (float)rand.Next()%2;
    }
}
