using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent agent;
    public int health;
    public float moveSpeed;
    public float rotationSpeed;
    public float freezeTime = 5f;
    public int damage = 2;

    private Quaternion _targetRotation;

    private bool _updateRotation = true;

    private bool _doMove = false;

    protected bool _isFrozen = false;
    private int _frozenCount = 0;
    private GameObject _iceBlock;
    public GameObject iceBlockPrefab;

    protected GameObject player;

    protected bool isChasingPlayer = false;
    protected Vector3 targetPos;

    private bool hasTriggeredArrivedAtTargetPos = false;

    private Vector3 oldPos;
    private Vector3 oldVel;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        _targetRotation = transform.rotation;
        SetRotationTowardsTarget(new Vector3(0, 20, 0), true);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (IsMoving())
        {
            RotateToFaceMovingDirection();
        }
        else if (HasReachedTargetLocation() && !hasTriggeredArrivedAtTargetPos)
        {
            OnArrivingAtTargetPosition();
        }


        UpdateRotation();
    }

    private void LateUpdate()
    {
        oldPos = transform.position;
        //oldVel = 
    }

    protected bool IsMoving()
    {

        return agent.velocity.magnitude > 0;
    }

    protected virtual void Attack()
    {
        player.GetComponent<Player>().TakeDamage(damage);
       
    }

    private void UpdateRotation()
    {
        if (_updateRotation)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    protected void SetTargetPosition(Vector3 pos)
    {
        targetPos = pos;
        agent.SetDestination(pos);
        hasTriggeredArrivedAtTargetPos = false;
    }


    protected void RotateToFacePlayer()
    {
        if (GameManager.Instance.player)
        {
            SetRotationTowardsTarget(GameManager.Instance.player.transform.position);
        }
    }
    protected void RotateToFaceMovingDirection()
    {
        //Vector3 dir = (transform.position - oldPos);
        //if (dir.magnitude > 0)
        //{
        //    SetRotationTowardsTarget(transform.position + dir);
        //}

        if (agent.desiredVelocity.magnitude > 0)
        {
            SetRotationTowardsTarget(transform.position + agent.desiredVelocity);
        }
        

    }

    protected virtual void OnArrivingAtTargetPosition()
    {
        //... 
        //override for functionality

        hasTriggeredArrivedAtTargetPos = true;
    }

    protected void SetRotationTowardsTarget(Vector3 target, bool onlyYAxis = true, bool force = false)
    {

        Vector3 dir = target - transform.position;

        if (onlyYAxis)
        {
            _targetRotation = Quaternion.LookRotation(dir);
            
        }
        else
        {
            //This doesnt work atm xdd but it might not be needed
            _targetRotation = Quaternion.FromToRotation(transform.forward, dir);
        }

        if(force)
        {
            transform.rotation = _targetRotation;
        }
        

    }

    public void TakeDamage(int dmg)
    {
        health -= dmg;
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void Freeze()
    {
        if(!_isFrozen) 
        {
            Collider collider = GetComponent<Collider>();
            _iceBlock = Instantiate(iceBlockPrefab, collider.bounds.center, Quaternion.identity, transform);
            _iceBlock.transform.localScale = collider.bounds.size * 1.2f;
            agent.isStopped = true;
        }
        _frozenCount++;

        StartCoroutine(FreezeTimer());

    }

    IEnumerator FreezeTimer()
    {
        int currentFrozenCount = _frozenCount;
        yield return new WaitForSeconds(freezeTime);
        if(currentFrozenCount == _frozenCount)
        {
            _isFrozen = false;
            agent.isStopped = false;
            if(_iceBlock != null)
            {
                Destroy(_iceBlock);
            }
        }
        
    }

    protected bool HasReachedTargetLocation()
    {
       

        Vector2 currentPos2d = new Vector2(transform.position.x, transform.position.z);
        Vector2 targetPos2d = new Vector2(targetPos.x, targetPos.z);

        if (Vector3.Distance(currentPos2d, targetPos2d) <= agent.stoppingDistance)
        {
            return true;
        }
        return false;
    }
}
