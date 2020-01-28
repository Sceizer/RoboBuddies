using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabObject : MonoBehaviour
{

    Transform currHoldingObject = null;
    //Position where the object will be when it is being held
    public Transform holdPosition;

    //Position where the object can be picked up from
    public Transform pickUpPosition;
    public float pickUpRange = 0.2f;

    //On what layer will the objects be
    public LayerMask pickUpableObjectLayer;

    public void GrabOrDrop()
    {
        if (currHoldingObject == null)
        {
            Transform newObjectToHold = CheckWhatObjectCanBeGrabbed();
            if (newObjectToHold != null)
            {
                HoldObject(newObjectToHold);
            }
        }
        else
        {
            DropObject();
        }
    }

    Transform CheckWhatObjectCanBeGrabbed()
    {
        Collider[] pickupableObjects = Physics.OverlapSphere(pickUpPosition.position, pickUpRange * 0.5f, pickUpableObjectLayer);
        if (pickupableObjects.Length > 0)
        {
            return pickupableObjects[0].transform;
        }
        return null;
    }

    void HoldObject(Transform objectToHold)
    {
        if(currHoldingObject != null)
        {
            Debug.LogError(name + " is already holding a item!");
            return;
        }
        currHoldingObject = objectToHold;
        objectToHold.transform.position = holdPosition.position;
        objectToHold.SetParent(holdPosition);

        Rigidbody objToHoldRb = objectToHold.GetComponent<Rigidbody>();
        if (objToHoldRb != null)
        {
            objToHoldRb.isKinematic = true;
        }

        Physics.IgnoreCollision(objectToHold.GetComponent<Collider>(), transform.GetComponent<Collider>(), true);
    }

    void DropObject()
    {
        if(currHoldingObject == null)
        {
            Debug.LogError(name + " is not holding a item!");
            return;
        }
        currHoldingObject.transform.position = pickUpPosition.position;
        currHoldingObject.SetParent(null);

        Rigidbody objToHoldRb = currHoldingObject.GetComponent<Rigidbody>();
        if (objToHoldRb != null)
        {
            objToHoldRb.isKinematic = false;
        }
        Physics.IgnoreCollision(currHoldingObject.GetComponent<Collider>(), transform.GetComponent<Collider>(), false);

        currHoldingObject = null;
    }
}
