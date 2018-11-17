using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PortalManager : MonoBehaviour {
    public GameObject mainCamera;
    public GameObject Sponza;
    public Material[] SponzaMaterials;
	// Use this for initialization
	void Start () {
        SponzaMaterials = Sponza.GetComponent<Renderer>().sharedMaterials;
	}
	
	// Update is called once per frame
	void OnTriggerStay (Collider collider) {
        Vector3 camPositionInPortalSpace = transform.InverseTransformPoint(mainCamera.transform.position);
        if (camPositionInPortalSpace.y < 0.5f)
        {
            for (int i = 0; i < SponzaMaterials.Length; ++i)
            {
                SponzaMaterials[i].SetInt("_StencilComp", (int)CompareFunction.Always);
            }
        }
        else {
            for (int i = 0; i < SponzaMaterials.Length; ++i)
            {
                SponzaMaterials[i].SetInt("_StencilComp", (int)CompareFunction.Equal);
            }

        }
    }
}
