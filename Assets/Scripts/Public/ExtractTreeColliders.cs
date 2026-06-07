using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Terrain))]
public class ExtractTreeColliders : MonoBehaviour
{
    [SerializeField]
    private Terrain terrain;

    private void Reset()
    {
        terrain = GetComponent<Terrain>();
        Extract();
    }

    [ContextMenu("Extract")]
    public void Extract()
    {
        Transform[] allChildren = terrain.GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            if (child != null && child.name.Contains("_Generated_Obstacle"))
            {
                DestroyImmediate(child.gameObject);
            }
        }

        if (terrain.terrainData == null)
        {

            return;
        }

        for (int i = 0; i < terrain.terrainData.treePrototypes.Length; i++)
        {
            TreePrototype tree = terrain.terrainData.treePrototypes[i];
            if (tree.prefab == null) continue;


            CapsuleCollider prefabCollider = tree.prefab.GetComponentInChildren<CapsuleCollider>();

            if (!prefabCollider)
            {

                continue;
            }

            TreeInstance[] instances = terrain.terrainData.treeInstances.Where(x => x.prototypeIndex == i).ToArray();


            for (int j = 0; j < instances.Length; j++)
            {
                Vector3 worldPos = Vector3.Scale(instances[j].position, terrain.terrainData.size);
                worldPos += terrain.GetPosition();

                GameObject obj = new GameObject(tree.prefab.name + "_" + j + "_Generated_Obstacle");

                CapsuleCollider objCollider = obj.AddComponent<CapsuleCollider>();
                objCollider.center = prefabCollider.center;
                objCollider.height = prefabCollider.height;
                objCollider.radius = prefabCollider.radius;

                if (terrain.preserveTreePrototypeLayers)
                    obj.layer = tree.prefab.layer;
                else
                    obj.layer = terrain.gameObject.layer;

                obj.transform.position = worldPos;
                obj.transform.parent = terrain.transform;
            }
        }

    }
}