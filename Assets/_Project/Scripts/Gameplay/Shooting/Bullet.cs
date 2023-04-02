using Unity.Netcode;
using UnityEngine;

public abstract class Bullet : PolledObject
{
    [field: SerializeField] public float Damage { private set; get; }
    public ulong senderId { private set; get; }
    
    protected Vector3 direction;
    protected float speed;
    
    private bool canMove;

    public void SetSpeed(float speed_) {
        this.speed = speed_;
    }
    public void SetSenderId(ulong id) {
        this.senderId = id;
    }
    public void MoveTowards(Vector3 direction_) {
        this.direction = direction_;

        canMove = true;
    }

    protected abstract void MoveTo();
    
    
    #region Unity function

    private void FixedUpdate() { if (!canMove) return; MoveTo(); }

    private void OnTriggerEnter(Collider other) {
        if (!other.gameObject.CompareTag("Wall")) return;
        if (NetworkManager.Singleton == null || !NetworkManager.Singleton.IsServer) return;
        
        NetworkPoller.Instance.PollObjectServerRpc(senderId,Type,objectId);
    }

    #endregion
    
}