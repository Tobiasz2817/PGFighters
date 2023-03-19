using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PollerPlaceHolder : MonoBehaviour
{
    public List<PolledObject> objects = new List<PolledObject>();

    public Transform resetTo;
    public float syncTime = 5f;

    private void Awake() {
        GameManager.Instance.GameIsOver += (id) => StopAllCoroutines();
    }

    public void InitPoller(int count, PolledObject prefab) {
        for (int i = 0; i < count; i++) {
            var spawnedObject = Instantiate(prefab, transform);
            spawnedObject.gameObject.SetActive(false);
            spawnedObject.objectId = i;
            objects.Add(spawnedObject);
        }
    }
    
    public void InitTransform(Transform resetTo) {
        this.resetTo = resetTo;

        StartCoroutine(SyncObject(syncTime));
    }

    private IEnumerator SyncObject(float time) {
        while (GameManager.Instance.CurrentState != GameManager.GameState.End) {
            foreach (var polledObject in objects) {
                if (!polledObject.gameObject.activeInHierarchy) {
                    polledObject.transform.position = resetTo.transform.position;

                    if (polledObject.rb != null) {
                        polledObject.rb.velocity = Vector3.zero;
                        polledObject.rb.angularVelocity = Vector3.zero;
                    }
                }
            }
            yield return new WaitForSeconds(time);
        }
    }
    
    
    public PolledObject GetUnActiveObject() {
        foreach (var object_ in objects) {
            if (!object_.gameObject.activeInHierarchy)
                return object_;
        }

        return null;
    }
    
    public PolledObject[] GetUnActiveObjects(int count) {
        var objects_ = new PolledObject[count];
        int indexer = 0;
        foreach (var object_ in objects) {
            if (!object_.gameObject.activeInHierarchy) {

                if (indexer >= count) 
                    return objects_;

                objects_[indexer] = object_;
                indexer++;
            }
        }

        return null;
    }
    
    public int[] GetUnActiveIndexes(int count) {
        var objects_ = new int[count];
        int indexer = 0;
        foreach (var object_ in objects) {
            if (!object_.gameObject.activeInHierarchy) {

                if (indexer >= count) 
                    return objects_;

                objects_[indexer] = object_.objectId;
                indexer++;
            }
        }

        return null;
    }

    public PolledObject GetObject(int index) {
        foreach (var object_ in objects) {
            if (object_.objectId == index)
                return object_;
        }

        return null;
    }
    
    public void PollActiveObject(int index) {
        var polledObj = objects[index];
        polledObj.gameObject.SetActive(false);
        if (polledObj.rb != null) {
            polledObj.rb.velocity = Vector3.zero;
            polledObj.rb.angularVelocity = Vector3.zero;
        }
        polledObj.transform.position = resetTo.position;
    }
}

