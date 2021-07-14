using UnityEngine;

public class MoveStick : MonoBehaviour
{
    [SerializeField] private RectTransform touchLeft, touchRight, uiLeft, uiRight;
    [SerializeField] private Transform stickLeft, stickRight;
    [SerializeField] private float radiusCheckHitBox, sizeHitBox, limitTouchDistance;
    [SerializeField] private Vector3 offsetHitBox;
    [SerializeField] private LayerMask maskHitBox;
    [SerializeField] private bool bound;

    private Camera cam;

    private Transform body;

    private Vector3 offsetBody, moveLeft, moveRight, offsetTouch;

    private Stick stick;
    private enum Stick { Null, Left, Right }

    private float zPos, yPos;

    void Start()
    {
        cam = Camera.main;

        body = transform;

        zPos = body.position.z;
        yPos = -10.5f;

        moveLeft = stickLeft.position;
        moveRight = stickRight.position;

        touchLeft.position = RectTransformUtility.WorldToScreenPoint(cam, moveLeft);
        touchRight.position = RectTransformUtility.WorldToScreenPoint(cam, moveRight);

        uiLeft.position = RectTransformUtility.WorldToScreenPoint(cam, stickLeft.position);
        uiRight.position = RectTransformUtility.WorldToScreenPoint(cam, stickRight.position);
    }

    void Update()
    {
        AlignBody();

        ControlStickArea();
    }

    void ControlStickArea()
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(touchLeft.position);
        if (stick == Stick.Left && Physics.Raycast(ray, out hit, 100))
        {
            offsetTouch = hit.point;
            offsetTouch.z = moveLeft.z;

            moveLeft = CheckTrack(offsetTouch, stickLeft.position, Stick.Left);
        }

        ray = cam.ScreenPointToRay(touchRight.position);
        if (stick == Stick.Right && Physics.Raycast(ray, out hit, 100))
        {
            offsetTouch = hit.point;
            offsetTouch.z = moveRight.z;

            moveRight = CheckTrack(offsetTouch, stickRight.position, Stick.Right);
        }

        if (Vector3.Distance(moveLeft, moveRight) > limitTouchDistance + .01f)
        {
            moveLeft = stickLeft.position;
            moveRight = stickRight.position;
        }
        else if (Vector3.Distance(moveLeft, moveRight) < limitTouchDistance)
        {
            if (moveLeft.x - moveRight.x < -1.5f)
            {
                moveLeft = CheckTrack(moveLeft, stickLeft.position, Stick.Left);
                moveRight = CheckTrack(moveRight, stickRight.position, Stick.Right);

                stickLeft.position = moveLeft;
                stickRight.position = moveRight;
            }
            else if (moveLeft.x - moveRight.x > -1.51f)
            {
                moveLeft = stickLeft.position;
                moveRight = stickRight.position;
            }
        }

        if (Input.GetButtonUp("Fire1"))
        {
            if (Vector3.Distance(moveLeft, moveRight) > limitTouchDistance)
            {
                moveLeft = stickLeft.position;
                moveRight = stickRight.position;
            }

            touchLeft.position = RectTransformUtility.WorldToScreenPoint(cam, moveLeft);
            touchRight.position = RectTransformUtility.WorldToScreenPoint(cam, moveRight);
        }

        uiLeft.position = RectTransformUtility.WorldToScreenPoint(cam, stickLeft.position);
        uiRight.position = RectTransformUtility.WorldToScreenPoint(cam, stickRight.position);
    }

    Vector3 CheckTrack(Vector3 moved, Vector3 stickPosition, Stick stick)
    {
        if (!Physics.CheckSphere(moved + offsetHitBox + new Vector3(0, sizeHitBox), radiusCheckHitBox, maskHitBox) ||
            !Physics.CheckSphere(moved + offsetHitBox - new Vector3(0, sizeHitBox), radiusCheckHitBox, maskHitBox))
            moved.y = stickPosition.y;

        if (!Physics.CheckSphere(moved + offsetHitBox + new Vector3(sizeHitBox, 0), radiusCheckHitBox, maskHitBox) ||
            !Physics.CheckSphere(moved + offsetHitBox - new Vector3(sizeHitBox, 0), radiusCheckHitBox, maskHitBox))
            moved.x = stickPosition.x;

        var move = moved;

        Collider[] checkSphere = Physics.OverlapSphere(move + offsetHitBox, radiusCheckHitBox, maskHitBox);

        if (checkSphere.Length > 0)
        {
            if (checkSphere[0].tag == "ver_135")
            {
                float step = stickPosition.x - move.x;
                move.y = stickPosition.y + step;

                if (stick != this.stick)
                {
                    move.x = Mathf.Lerp(move.x, move.x + 1, Time.deltaTime * 10);
                    move.y = Mathf.Lerp(move.y, move.y - 1, Time.deltaTime * 10);
                }
            }
            else if (checkSphere[0].tag == "ver_45")
            {
                float step = stickPosition.x - move.x;
                move.y = stickPosition.y - step;

                if (stick != this.stick)
                {
                    move.x = Mathf.Lerp(move.x, move.x - 1, Time.deltaTime * 10);
                    move.y = Mathf.Lerp(move.y, move.y - 1, Time.deltaTime * 10);
                }
            }
            else if (checkSphere[0].tag == "ver")
            {
                move = Vector3.Project(move, Vector3.up);
                move.x = checkSphere[0].transform.position.x;

                if (stick != this.stick) move.y = Mathf.Lerp(move.y, move.y - 1, Time.deltaTime * 10);
            }
            else if (checkSphere[0].tag == "hor")
            {
                move = Vector3.Project(move, Vector3.right);
                move.y = checkSphere[0].transform.position.y;
            }
        }
        else
        {
            return stickPosition;
        }

        move.z = moved.z;

        if (bound && Vector3.Distance(move, stickPosition) > 2)
        {
            return stickPosition;
        }

        return move;
    }

    void AlignBody()
    {
        var distance = Vector3.Distance(stickLeft.position, stickRight.position);
        var direction = stickLeft.position;

        direction.x += distance / 2f;
        direction.y = stickLeft.position.y > stickRight.position.y ? stickLeft.position.y : stickRight.position.y;
        direction.z = zPos;

        if (Mathf.Abs(stickLeft.position.y - stickRight.position.y) < .25f)
            yPos = Vector3.Distance(moveLeft, moveRight) / 2.5f - 11.3f;
        else
            yPos = -10.0f;

        offsetBody.y = yPos;

        body.position = direction + offsetBody;
    }

    public void SelectStick(string selectStick = "")
    {
        if (selectStick == "left") stick = Stick.Left;
        else if (selectStick == "right") stick = Stick.Right;
        else stick = Stick.Null;
    }
}