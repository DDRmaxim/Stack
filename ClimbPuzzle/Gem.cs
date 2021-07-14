using UnityEngine;

public class Gem : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    [SerializeField] private float radius = 1;
    [SerializeField] private LayerMask playerMask;
    [Space]
    [SerializeField] private ParticleSystem confetti;
    [SerializeField] private GameObject manDance;
    [SerializeField] private Vector3 target;

    Transform t;
    GameObject player;

    void Start()
    {
        t = transform;

        player = GameObject.Find("/Stickman");
    }

    void Update()
    {
        t.Rotate(0, speed * Time.deltaTime, 0);

        if (Physics.CheckSphere(t.position, radius, playerMask))
        {
            SingleVar.StartGame = false;

            Instantiate(confetti, t.position, Quaternion.identity);

            Instantiate(manDance, player.transform.position, player.transform.rotation).SendMessage("Target", target);

            Destroy(player);

            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(target, .3f);
    }
}
