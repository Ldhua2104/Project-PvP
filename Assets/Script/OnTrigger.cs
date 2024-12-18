using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTrigger : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Renderer>().enabled = false;
    }
}
