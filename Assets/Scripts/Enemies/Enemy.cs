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

    private Quaternion _targetRotation;

    private bool _updateRotation = true;

    private bool _doMove = false;

    private bool _isFrozen = false;
    private int _frozenCount = 0;
    private GameObject _iceBlock;
    public GameObject iceBlockPrefab;

    protected GameObject player;

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

        UpdateRotation();
    }


    protected virtual void Attack()
    {
        //TODO: 
        //Player.TakeDamage().. 
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
        agent.SetDestination(pos);
    }

    protected void RotateToFacePlayer(Vector3 euler)
    {
        if (GameManager.Instance.player)
        {
            SetTargetPosition(GameManager.Instance.player.transform.position);
        }
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
            if(_iceBlock != null)
            {
                Destroy(_iceBlock);
            }
        }
        
    }
}
