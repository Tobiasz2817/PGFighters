using UnityEngine;

public class PollerData : MonoBehaviour
{
    public PolledObject prefab;
    public int count = 1;
    public bool isNetwork = false;
}

public enum ObjectPollTypes
{
    BulletType1,
    BulletType2
}