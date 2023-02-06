using UnityEngine;

public class NormalBullet : Bullet 
{
    protected override void MoveTo() {
        transform.position += direction * (speed * Time.deltaTime);
        Debug.Log("bullet is Moving");
    }
}
