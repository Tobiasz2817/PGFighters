using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PollerPlaceHolder : MonoBehaviour
{
    public List<PolledObject> objects = new List<PolledObject>();
    
    public PollerData pollerData;
    
    public Transform resetTo;
    public float syncTime = 5f;
    public float reverseDestroyTime = 10f;
    
    private void Awake() {
        LoadingSceneManager.Instance.OnClientsConnected += (ids) => GameManager.Instance.GameIsOver += (id) => StopAllCoroutines();
    }

    public void InitPoller(PollerData pollerData) {
        this.pollerData = pollerData;
        objects = pollerData.CreatePolledObjects(transform);
    }
    
    public void InitTransform(Transform resetTo) {
        this.resetTo = resetTo;

        if(pollerData.syncWithOwner)
            StartCoroutine(SyncObject(syncTime));
    }

    private IEnumerator SyncObject(float time) {
        while (GameManager.Instance.CurrentState != GameManager.GameState.End) {
            foreach (var polledObject in objects) {
                if (!polledObject.gameObject.activeInHierarchy) {
                    SetPosition(polledObject);
                    ResetRigidbody(polledObject);
                }
            }
            yield return new WaitForSeconds(time);
        }
    }

    private void SetPosition(PolledObject polledObject) {
        polledObject.transform.position = resetTo.transform.position;
    }
    private void ResetParent(PolledObject polledObject) {
        polledObject.transform.parent = transform;
        polledObject.transform.SetSiblingIndex(polledObject.objectId);
    }
    private void ResetRigidbody(PolledObject polledObject) {
        if (polledObject.rb != null) {
            polledObject.rb.velocity = Vector3.zero;
            polledObject.rb.angularVelocity = Vector3.zero;
        }
    }

    private void ActiveObject(PolledObject polledObject,bool state) {
        polledObject.gameObject.SetActive(state);
    }

    private void SetIdObject(PolledObject polledObject,int index) {
        polledObject.objectId = index;
    }

    public PolledObject GetUnActiveObject() {
        foreach (var object_ in objects) {
            if (!object_.gameObject.activeInHierarchy)
                return object_;
        }

        return null;
    }
    public PolledObject GetUnActiveObject(int index) {
        foreach (var object_ in objects) {
            if (!object_.gameObject.activeInHierarchy)
                if(object_.objectId == index)
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
    public PolledObject GetUnActiveObject(Type type) {
        foreach (var object_ in objects) 
            if (!object_.gameObject.activeInHierarchy) 
                if (type == object_.GetType())
                    return object_;

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
    
    public List<int> GetObjectsIndexesDifferentTypes(int countPerType) {
        List<int> objects_ = new List<int>();
        Type previousType = null;
        Type currentType = null;
        int counterEntry = 0;
        
        foreach (var object_ in objects) {
            if (!object_.gameObject.activeInHierarchy) {
                currentType = object_.GetType();
                
                if (counterEntry >= countPerType && currentType == previousType) {
                    continue;
                }
                
                if (currentType != previousType && counterEntry != 0) {
                    counterEntry = 0;
                }

                objects_.Add(object_.objectId);
                counterEntry++;

                previousType = currentType;
            }
        }

        return objects_;
    }

    public int GetUnActiveIndex() {
        foreach (var object_ in objects) 
            if (!object_.gameObject.activeInHierarchy) 
                return object_.objectId;

        return -1;
    }
    
    public PolledObject GetObject(int index) {
        foreach (var object_ in objects) {
            if (!object_.gameObject.activeInHierarchy)
                if (object_.objectId == index)
                    return object_;
        }

        return null;
    }
    public PolledObject GetActiveObject(int index) {
        foreach (var object_ in objects) {
            if (object_.objectId == index)
                    return object_;
        }

        return null;
    }
    public PolledObject[] GetUnActiveObjects(Type type, int count) {
        var objects_ = new PolledObject[count];
        int indexer = 0;
        
        foreach (var object_ in objects) {
            if (!object_.gameObject.activeInHierarchy) {
                
                if (type != object_.GetType()) {
                    if (indexer > 0)
                        break;
                    
                    continue;
                }  
                
                if (indexer >= count)
                    break;

                objects_[indexer] = object_;
                indexer++;
            }
        }

        return objects_;
    }
    public List<PolledObject> GetUnActiveListObjects(Type type, int count) {
        var objects_ = new List<PolledObject>();
        int indexer = 0;
        
        foreach (var object_ in objects) {
            if (!object_.gameObject.activeInHierarchy) {
                if (type == object_.GetType()) {
                    if (indexer >= count)
                        break;
                    
                    indexer++;
                    objects_.Add(object_);
                }
                
                if (indexer > 0 && type != object_.GetType())
                    return objects_;
            }
        }

        return objects_;
    }
    public int GetIndexObject(Type type) {
        foreach (var object_ in objects) {
            if (!object_.gameObject.activeInHierarchy)
                if (object_.GetType() == type)
                    return object_.objectId;
        }

        return -1;
    }

    public int[] GetIndexesObjects(int count) {
        var objects_ = new int[count];
        int indexer = 0;
        
        return objects_;
    }
    
    
    public int GetIndexPrefab(Type type) {
        return pollerData.GetIndexObjectsByType(type);
    }
    
    public void PollActiveObject(int index) {
        if (index >= 0 && index < objects.Count) {
            var polledObj = objects[index];
            polledObj.gameObject.SetActive(false);
            ResetParent(polledObj);
            SetPosition(polledObj);
            ResetRigidbody(polledObj);    
        }
    }

    #region Reversing

    public void ReverseOnNewObject(int index) {
        StopCoroutine(nameof(SyncObject));
        
        ReChangeReferencesList(index);
        
        StartCoroutine(DestroyReversedObjects(reverseDestroyTime));
        
        
        if(pollerData.syncWithOwner)
            StartCoroutine(SyncObject(syncTime));
    }
    
    public void ReverseOnNewObject(Type type) {
        StopCoroutine(nameof(SyncObject));
        
        ReChangeReferencesList(GetIndexPrefab(type));
        
        StartCoroutine(DestroyReversedObjects(reverseDestroyTime));
        
        
        if(pollerData.syncWithOwner)
            StartCoroutine(SyncObject(syncTime));
    }

    private void ReChangeReferencesList(int newObjectIndex) {
        var currentLength = objects.Count;
        var lastLength = currentLength + pollerData.count;
        for (int i = objects.Count; i < lastLength; i++) {
            var spawnedObject = Instantiate(pollerData.GetPolledObject(newObjectIndex), transform);
            SetPosition(spawnedObject);
            ResetRigidbody(spawnedObject);
            ActiveObject(spawnedObject,false);
            objects.Add(spawnedObject);

            var first = objects[i - currentLength];
            var currentNew = objects[i];
            var tmp_first = first;

            objects[i - currentLength] = currentNew;
            SetIdObject(objects[i - currentLength],i - currentLength);
            objects[i] = tmp_first;
            SetIdObject(objects[i],i);
        }
    }
    
    private IEnumerator DestroyReversedObjects(float time = 0f) {
        yield return new WaitForSeconds(time);
        
        DestroyObjects(pollerData.count, pollerData.count * 2);
        objects.RemoveRange(pollerData.count, pollerData.count);
        MoveRightObjectsIds(pollerData.count,objects.Count);
    }

    private void MoveRightObjectsIds(int startRange, int endRange) {
        for (int i = startRange; i < endRange; i++)
            SetIdObject(objects[i],i);
    }
    private void DestroyObjects(int startRange, int endRange) {
        for (int i = startRange; i < endRange; i++)
            Destroy(objects[i].gameObject);
    }

    #endregion
}
