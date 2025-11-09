using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class DragElementFollow : MonoBehaviour
{
    public RectTransform RectTransform => rectTransform;
    private RectTransform rectTransform;

    [Header("Components")]
    [SerializeField, Required] private DragElement linkedElement;

    [Header("Settings")]
    [Tooltip("How quickly it accelerates toward the target.")]
    [SerializeField] private float motionStiffness = 200f;
    [Tooltip("How much resistance to motion.")]
    [SerializeField] private float motionDamping = 20f;
    [Space]
    [SerializeField] public Vector3 offset = Vector3.zero;
    [SerializeField, MinValue(1)] public float maxVelocity = 1000f;

    [Header("Info")]
    [SerializeField, ReadOnly] public Vector3 targetPos;
    [SerializeField, ReadOnly] public Vector3 trajectory;
    [SerializeField, ReadOnly] public float velocitySpeed;
    [field: SerializeField, ReadOnly] public Vector3 Velocity { get; private set; }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    private void Update()
    {
        SyncWithLinkedElement();
        FollowCard();
    }

    private void SyncWithLinkedElement()
    {
        //Force same parent
        if (transform.parent != linkedElement.RectTransform.parent)
        {
            transform.SetParent(linkedElement.RectTransform.parent, true);
        }
        //Force to be next in parent order
        if (transform.GetSiblingIndex() != linkedElement.RectTransform.GetSiblingIndex() + 1)
        {
            transform.SetSiblingIndex(linkedElement.RectTransform.GetSiblingIndex() + 1);
        }
        //Force name
        if (transform.name != $"{linkedElement.name}-Visual")
        {
            transform.name = $"{linkedElement.name}-Visual";
        }
    }
    private void FollowCard()
    {
        targetPos = linkedElement.RectTransform.position + offset;
        trajectory = RectTransform.position - targetPos;

        if (trajectory.sqrMagnitude < .1f && Velocity.sqrMagnitude < .1f)
        {
            Velocity = Vector3.zero;
            RectTransform.position = targetPos;
        }
        else
        {
            Vector3 springForce = -trajectory * motionStiffness;
            Vector3 dampingForce = -Velocity * motionDamping;
            Vector3 acceleration = springForce + dampingForce;
            Velocity += acceleration * Time.deltaTime;

            if (Velocity.magnitude > maxVelocity) Velocity = Velocity.normalized * maxVelocity;
            RectTransform.position += Velocity * Time.deltaTime;
        }

        velocitySpeed = Velocity.magnitude;
    }
}