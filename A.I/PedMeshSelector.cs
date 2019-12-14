using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedMeshSelector : MonoBehaviour
{

    public List<GameObject> PedMeshes = new List<GameObject>();
    public bool randomisePed;
    public int mesh;
    public Mesh gizmoMesh;
    public enum m_SelectPedMesh{

    }
    // Start is called before the first frame update
    void Start()
    {
        GetMeshes();
        PEDMesh_Random();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GetMeshes()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.CompareTag("PEDMESH"))
            {
                PedMeshes.Add(child.transform.gameObject);
            }
        }
    }

    void PEDMesh_Random()
    {
        if (randomisePed)
        {
            mesh = Random.Range(0, PedMeshes.Count);
            PedMeshes[mesh].SetActive(true);
        }
        else
        {
            PedMeshes[mesh].SetActive(true);
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawMesh(gizmoMesh,transform.position,transform.rotation,new Vector3(15,15,15));
    }
}
