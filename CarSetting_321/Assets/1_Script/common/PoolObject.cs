using UnityEngine;
using System.Collections.Generic;

public class PoolObject : MonoBehaviour {
	
	public virtual void OnObjectReuse() {

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }

	}

    protected void Destroy()
    {
        gameObject.SetActive(false);
    }
}