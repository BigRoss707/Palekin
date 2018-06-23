using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour
{

    public float minGroundNormalY = .65f;
    public float gravityModifier = 1f;
    public float gravity = 5f;

    private const float minMoveDistance = 0.001f;
    private float maxVertMove;
    private const float shellRadius = 0.01f;

    private Rigidbody2D rigidBody;
    protected Vector2 velocity;
    protected Vector2 targetVelocity;
    private Vector2 groundNormal;
    protected bool grounded;
    private ContactFilter2D contactFilter; 

    void OnEnable()
    {
        rigidBody = this.GetComponent<Rigidbody2D>();
    }

    protected virtual void Start()
    {
        maxVertMove = gravity * gravityModifier * Physics2D.gravity.magnitude;
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;
    }

    protected virtual void Update()
    {
        targetVelocity = Vector2.zero;
        ComputeVelocity();
    }

    protected virtual void ComputeVelocity()
    {

    }

    protected virtual void FixedUpdate()
    {
        grounded = false;
        ApplyGravity();
        ApplyVelocity();
        Vector2 deltaPosition = velocity * Time.deltaTime;
        Vector2 moveX = deltaPosition.x * new Vector2(groundNormal.y, -groundNormal.x);
        Vector2 move = moveX;
        Move(move, false);
        move = Vector2.up * deltaPosition.y;
        Move(move, true);
    }

    void Move(Vector2 move, bool yMove)
    {
        float distance = move.magnitude;
        RaycastHit2D[] hits = new RaycastHit2D[16];
        List<RaycastHit2D> hitsList = new List<RaycastHit2D>();

        if(distance >  minMoveDistance)
        {
            int count = rigidBody.Cast(move, contactFilter, hits, distance + shellRadius);
            for(int i = 0; i < count; i++)
            {
                hitsList.Add(hits[i]);
            }

            for(int i = 0; i < hitsList.Count; i++)
            {
                Vector2 currentNormal = hitsList[i].normal;
                if(currentNormal.y > minGroundNormalY)
                {
                    grounded = true;
                    if(yMove)
                    {
                        groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }

                float projection = Vector2.Dot(velocity, currentNormal);
                if(projection < 0)
                {
                    velocity = velocity - projection * currentNormal;
                }

                float modifiedDistance = hitsList[i].distance - shellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }
        }

        rigidBody.position = rigidBody.position + move.normalized * distance;
    }

    void ApplyGravity()
    {
        velocity += gravity * gravityModifier* Physics2D.gravity * Time.deltaTime;
        if(-velocity.y > maxVertMove)
        {
            velocity.y = -maxVertMove;
        }
    }
    
    void ApplyVelocity()
    {
        velocity.x = targetVelocity.x;
    }

}