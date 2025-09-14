using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class effect : MonoBehaviour
{
    // Start is called before the first frame update
    public void OnDestroy()
    {
        Destroy(gameObject);
    }
}
