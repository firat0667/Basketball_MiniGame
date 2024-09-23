using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketDedector : MonoBehaviour
{
    public delegate void BasketScored();
    public static event BasketScored OnBasketScored;
    private bool _istop;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Basketball"))  
        {
            if (other.transform.position.y > transform.position.y)
            {
                if (OnBasketScored != null)
                {
                    _istop = true;
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.position.y < transform.position.y)
        {
            if (_istop)
            {
                 OnBasketScored();
                _istop = false;
            }
        }
    }
}
