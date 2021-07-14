using UnityEngine;

public class DanceMan : MonoBehaviour
{
    [SerializeField] private float time = 1;

    Transform t;

    private Vector3 target;

    void Target(Vector3 target) => this.target = target;

    void Start()
    {
        t = transform;
    }

    void Update()
    {
        t.position = Vector3.MoveTowards(t.position, target, Time.deltaTime * time);
    }
}
