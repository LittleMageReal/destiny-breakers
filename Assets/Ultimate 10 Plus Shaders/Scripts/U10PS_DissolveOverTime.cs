using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class U10PS_DissolveOverTime : MonoBehaviour
{
    private bool isDissolving = false;

    private MeshRenderer meshRenderer;

    public float speed = .5f;

    [SerializeField] private float dissolveDelay; // Add this line
    private float Cooldown = 3f;
    private float lastUse;


    private void Start(){
        meshRenderer = this.GetComponent<MeshRenderer>();
    }

    private float t = 0.0f;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && Time.time - lastUse >= Cooldown)
        {
            lastUse = Time.time;
            isDissolving = !isDissolving;

            //Stop visual dissolving after 3 seconds
            StartCoroutine(StopDissolvingAfterDelay(dissolveDelay));
        }

        if (isDissolving)
        {
            Material[] mats = meshRenderer.materials;

            mats[0].SetFloat("_Cutoff", Mathf.Sin(t * speed));
            t += Time.deltaTime;

            // Unity does not allow meshRenderer.materials[0]...
            meshRenderer.materials = mats;
        }

        IEnumerator StopDissolvingAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            isDissolving = false;
        }
    }
}
