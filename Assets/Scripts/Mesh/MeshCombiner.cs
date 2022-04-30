using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WEditor
{
    public class MeshCombiner : MonoBehaviour
    {
        [SerializeField] GameObject targetCombiner;
        public static MeshCombiner instance;
        List<GameObject> multipleTargetsCombiner = new List<GameObject>();
        private void Start()
        {
            instance = this;
        }
        public void DisableTargetCombiner()
        {
            targetCombiner.SetActive(false);
            foreach (var item in multipleTargetsCombiner)
            {
                print(item.name);
                Destroy(item);
            }
            multipleTargetsCombiner = new List<GameObject>();
        }
        public void CombineMultipleMeshes(Dictionary<string, List<GameObject>> meshFilters)
        {
            foreach (var item in meshFilters)
            {
                CombineInstance[] combine = new CombineInstance[item.Value.Count];
                int i = 0;
                var material = item.Value[0].GetComponent<MeshRenderer>().material;
                while (i < item.Value.Count)
                {
                    combine[i].mesh = item.Value[i].GetComponent<MeshFilter>().sharedMesh;
                    combine[i].transform = item.Value[i].transform.localToWorldMatrix;
                    item.Value[i].gameObject.SetActive(false);
                    i++;
                }
                GameObject multipleMesh = new GameObject("mmesh");
                multipleMesh.layer = 6;
                multipleMesh.tag = "Ground";
                MeshRenderer mMesh = multipleMesh.AddComponent<MeshRenderer>();
                mMesh.material = material;
                MeshFilter mFilter = multipleMesh.AddComponent<MeshFilter>();
                mFilter.mesh = new Mesh();
                mFilter.mesh.CombineMeshes(combine);
                multipleMesh.AddComponent<MeshCollider>();
                multipleTargetsCombiner.Add(multipleMesh);
                multipleMesh.transform.position = Vector3.zero;
            }
        }
        public void CombineMeshes(List<MeshFilter> meshFilters)
        {
            CombineInstance[] combine = new CombineInstance[meshFilters.Count];

            int i = 0;
            while (i < meshFilters.Count)
            {
                combine[i].mesh = meshFilters[i].sharedMesh;
                combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
                meshFilters[i].gameObject.SetActive(false);
                i++;
            }
            MeshFilter targetMeshFilter = targetCombiner.GetComponent<MeshFilter>();
            targetMeshFilter.mesh = new Mesh();
            targetMeshFilter.mesh.CombineMeshes(combine);
            targetMeshFilter.transform.position = Vector3.zero;
            targetMeshFilter.gameObject.AddComponent<MeshCollider>();
            targetCombiner.gameObject.SetActive(true);
        }
    }
}
