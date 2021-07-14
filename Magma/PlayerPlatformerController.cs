using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatformerController : PhysicsObject
{
    [Header("Player parameters")]
    public float maxSpeed = 2;
    public float jumpTake = 12;
    public GameObject rip;
    public GameObject[] selfi;
    public GameObject menu;

    [Header("Player hit triger collider")]
    public Vector2 sizeBox;
    public Vector3 offsetBox;
    public LayerMask maskHit;

    public AudioClip s_run;
    public AudioClip s_stone;
    private AudioSource source;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private Vector2 move;
    private Vector2 target;
    private Vector3 targetResp;
    private int damage = 0;

    private int[] simple = { 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 49, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97, 0 };

    private List<GameObject> listActive;

    [SerializeField] bool godfire = false; 

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        move = Vector2.right;

        targetResp = transform.position;

        listActive = new List<GameObject>();

        source = GetComponent<AudioSource>();
        source.playOnAwake = false;
        source.mute = false;
        source.loop = false;
    }

    void Start()
    {
        RIP();
    }

    protected override void ComputeVelocity()
    {
        if (SinglVar.life > 0 || godfire) {

            TrigerCollider();

            if (Input.GetButtonDown("Fire1")) {
                if (move.x > 0.01f) {
                    move.x = -1;
                    spriteRenderer.flipX = true;
                } else {
                    move.x = 1;
                    spriteRenderer.flipX = false;
                }
            }

            /*
            else if (Input.GetButtonDown("Fire2") && grounded && damage == 0) {
                if (move.x > 0.01f) {
                    move.x = -1;
                    spriteRenderer.flipX = true;
                } else {
                    move.x = 1;
                    spriteRenderer.flipX = false;
                }
                velocity.y = jumpTake;
                animator.SetTrigger("jump");
            } else if (Input.GetButtonUp("Fire2")) {
                if (velocity.y > 0) {
                    velocity.y = velocity.y * 0.5f;
                }
            }
            */

            if (damage == 0) {
                animator.SetBool("grounded", grounded);

                animator.SetFloat("velocityX", Mathf.Abs(move.x) / maxSpeed);

                targetVelocity = move * maxSpeed;

                if (!source.isPlaying && SinglVar.sound == 1)
                    source.PlayOneShot(s_run, 1);

                if (!grounded)
                    source.Stop();

            } else if (damage == 1) {
                animator.SetTrigger("damage");
                damage = 2;
                source.Stop();
            }

        } else RIP();

        //if (Input.GetKeyDown(KeyCode.L)) Live();
    }

    void TrigerCollider()
    {
        target = transform.position + offsetBox;
        
        Collider2D[] hitObjects = Physics2D.OverlapBoxAll(target, sizeBox, 0, maskHit);

        foreach (Collider2D hit in hitObjects) {

            if (hit.tag == "Coin") {
                hit.SendMessage("takeCoin", SendMessageOptions.DontRequireReceiver);
                SinglVar.scores += simple[Random.Range(0, simple.Length - SinglVar.level < 13 ? SinglVar.level : 13)];
                SinglVar.coin++;

                PlayerPrefs.SetInt("coin", SinglVar.coin);

                if (SinglVar.sound == 1)
                    AudioSource.PlayClipAtPoint(s_stone, transform.position);
            }

            if (hit.tag == "Camp") {
                hit.SendMessage("takeFire", SendMessageOptions.DontRequireReceiver);
                SinglVar.life += 5;
                SinglVar.scores += simple[Random.Range(0, 10)];

                if (SinglVar.sound == 1)
                    AudioSource.PlayClipAtPoint(s_stone, transform.position);
            }

            listActive.Add(hit.gameObject);
        }
    }

    void RIP()
    {
        if (damage != 2) {
            animator.SetBool("die", true);

            foreach (var item in selfi)
                item.SetActive(false);

            damage = 2;
        }

        if (SinglVar.live) {
            Instantiate(rip, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);

            SinglVar.live = false;

            menu.SendMessage("Pause", SendMessageOptions.DontRequireReceiver);
        }

        source.Stop();
    }

    void Live()
    {
        transform.position = targetResp;

        animator.SetBool("die", false);

        SinglVar.life = 100;
        SinglVar.live = true;

        foreach (var item in selfi)
            item.SetActive(true);

        damage = 0;

        foreach (var item in listActive) {
            item.SendMessage("reStart", SendMessageOptions.DontRequireReceiver);
        }

        listActive.Clear();
    }

    public void LowDamage()
    {
        damage = 0;
    }

    public void Damage()
    {
        damage = 1;
    }

    public void GOF()
    {
        SinglVar.life++;
        SinglVar.scores += simple[Random.Range(0, simple.Length)];
    }

    void OnDrawGizmosSelected()
    {
        target = transform.position + offsetBox;

        Gizmos.DrawWireCube(target, sizeBox);
    }
}