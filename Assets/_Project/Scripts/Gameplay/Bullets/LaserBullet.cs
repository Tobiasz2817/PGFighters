using UnityEngine;

public class LaserBullet : Bullet
{
    private Vector3 stopDistance;
    protected override void MoveTo() {
        transform.position += direction * (speed * Time.deltaTime);
        
        if(Vector3.Distance(transform.position,stopDistance) < 0.3f)
            NetworkPoller.Instance.PollObject(ownerId,Type,objectId);
    }
    
    public void SetStopDistance(Vector3 stopDistance) {
        this.stopDistance = stopDistance;
    }
}
