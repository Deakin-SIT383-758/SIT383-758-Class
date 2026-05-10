using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject cubePrefab;
    public GameObject spherePrefab;
    public GameObject cylinderPrefab;
    public GameObject capsulePrefab;
    public GameObject portalPrefab;
    public GameObject orePrefab;

    [Header("Spawn Points")]
    public Transform cubeSpawnPoint;
    public Transform sphereSpawnPoint;
    public Transform cylinderSpawnPoint;
    public Transform capsuleSpawnPoint;
    public Transform portalSpawnPoint;
    public Transform oreSpawnPoint;

    [Header("Spawned Objects")]
    public GameObject currentCube;
    public GameObject currentSphere;
    public GameObject currentCylinder;
    public GameObject currentCapsule;
    public GameObject currentPortal;
    public GameObject currentOre;

    public float scaleAmount = 0.25f;
    public float minimumScale = 0.25f;
    public float maximumScale = 3f;

    


    //SPAWNING METHODS
    public void SpawnCube()
    {
        if (currentCube != null)
        {
            Destroy(currentCube);
        }

        currentCube = Instantiate(
            cubePrefab,
            cubeSpawnPoint.position,
            Quaternion.identity
        );
    }

    public void SpawnSphere()
    {
        if (currentSphere != null)
        {
            Destroy(currentSphere);
        }

        currentSphere = Instantiate(
            spherePrefab,
            sphereSpawnPoint.position,
            Quaternion.identity
        );
    }

    public void SpawnCylinder()
    {
        if (currentCylinder != null)
        {
            Destroy(currentCylinder);
        }

        currentCylinder = Instantiate(
            cylinderPrefab,
            cylinderSpawnPoint.position,
            Quaternion.identity
        );
    }

    public void SpawnCapsule()
    {
        if (currentCapsule != null)
        {
            Destroy(currentCapsule);
        }

        currentCapsule = Instantiate(
            capsulePrefab,
            capsuleSpawnPoint.position,
            Quaternion.identity
        );
    }

    public void SpawnPortal()
    {
        if (currentPortal != null)
        {
            Destroy(currentPortal);
        }

        currentPortal = Instantiate(
            portalPrefab,
            portalSpawnPoint.position,
            Quaternion.identity
        );
    }

    public void SpawnOre()
    {
        if (currentOre != null)
        {
            Destroy(currentOre);
        }
        currentOre = Instantiate(
            orePrefab,
            oreSpawnPoint.position,
            Quaternion.identity
        );
    }


    //BIGGER METHODS
    public void MakeCubeBigger()
    {
        if (currentCube != null)
        {
            Vector3 newScale =
                currentCube.transform.localScale +
                Vector3.one * scaleAmount;

            newScale.x = Mathf.Clamp(
                newScale.x,
                minimumScale,
                maximumScale);

            newScale.y = Mathf.Clamp(
                newScale.y,
                minimumScale,
                maximumScale);

            newScale.z = Mathf.Clamp(
                newScale.z,
                minimumScale,
                maximumScale);

            currentCube.transform.localScale =
                newScale;
        }
    }

    public void MakeSphereBigger()
    {
        if (currentSphere != null)
        {
            Vector3 newScale =
                currentSphere.transform.localScale +
                Vector3.one * scaleAmount;
            newScale.x = Mathf.Clamp(
                newScale.x,
                minimumScale,
                maximumScale);
            newScale.y = Mathf.Clamp(
                newScale.y,
                minimumScale,
                maximumScale);
            newScale.z = Mathf.Clamp(
                newScale.z,
                minimumScale,
                maximumScale);
            currentSphere.transform.localScale =
                newScale;
        }
    }

    public void MakeCylinderBigger()
    {
        if (currentCylinder != null)
        {
            Vector3 newScale =
                currentCylinder.transform.localScale +
                Vector3.one * scaleAmount;
            newScale.x = Mathf.Clamp(
                newScale.x,
                minimumScale,
                maximumScale);
            newScale.y = Mathf.Clamp(
                newScale.y,
                minimumScale,
                maximumScale);
            newScale.z = Mathf.Clamp(
                newScale.z,
                minimumScale,
                maximumScale);
            currentCylinder.transform.localScale =
                newScale;
        }
    }

    public void MakeCapsuleBigger()
    {
        if (currentCapsule != null)
        {
            Vector3 newScale =
                currentCapsule.transform.localScale +
                Vector3.one * scaleAmount;
            newScale.x = Mathf.Clamp(
                newScale.x,
                minimumScale,
                maximumScale);
            newScale.y = Mathf.Clamp(
                newScale.y,
                minimumScale,
                maximumScale);
            newScale.z = Mathf.Clamp(
                newScale.z,
                minimumScale,
                maximumScale);
            currentCapsule.transform.localScale =
                newScale;
        }
    }

    public void MakePortalBigger()
    {
        if (currentPortal != null)
        {
            Vector3 newScale =
                currentPortal.transform.localScale +
                Vector3.one * scaleAmount;
            newScale.x = Mathf.Clamp(
                newScale.x,
                minimumScale,
                maximumScale);
            newScale.y = Mathf.Clamp(
                newScale.y,
                minimumScale,
                maximumScale);
            newScale.z = Mathf.Clamp(
                newScale.z,
                minimumScale,
                maximumScale);
            currentPortal.transform.localScale =
                newScale;
        }
    }

    public void MakeOreBigger()
    {
        if (currentOre != null)
        {
            Vector3 newScale =
                currentOre.transform.localScale +
                Vector3.one * scaleAmount;
            newScale.x = Mathf.Clamp(
                newScale.x,
                minimumScale,
                maximumScale);
            newScale.y = Mathf.Clamp(
                newScale.y,
                minimumScale,
                maximumScale);
            newScale.z = Mathf.Clamp(
                newScale.z,
                minimumScale,
                maximumScale);
            currentOre.transform.localScale =
                newScale;
        }
    }


    //SMALLER METHODS 
    public void MakeCubeSmaller()
    {
        if (currentCube != null)
        {
            Vector3 newScale =
                currentCube.transform.localScale -
                Vector3.one * scaleAmount;

            newScale.x = Mathf.Clamp(
                newScale.x,
                minimumScale,
                maximumScale);

            newScale.y = Mathf.Clamp(
                newScale.y,
                minimumScale,
                maximumScale);

            newScale.z = Mathf.Clamp(
                newScale.z,
                minimumScale,
                maximumScale);

            currentCube.transform.localScale =
                newScale;
        }
    }

    public void MakeSphereSmaller()
    {
        if (currentSphere != null)
        {
            Vector3 newScale = currentSphere.transform.localScale - Vector3.one * scaleAmount;

            newScale.x = Mathf.Clamp(
                newScale.x,
                minimumScale,
                maximumScale);

            newScale.y = Mathf.Clamp(
                newScale.y,
                minimumScale,
                maximumScale);

            newScale.z = Mathf.Clamp(
                newScale.z,
                minimumScale,
                maximumScale);

            currentSphere.transform.localScale =
                newScale;
        }
    }

    public void MakeCylinderSmaller()
    {
        if (currentCylinder != null)
        {
            Vector3 newScale = currentCylinder.transform.localScale - Vector3.one * scaleAmount;
            newScale.x = Mathf.Clamp(
                newScale.x,
                minimumScale,
                maximumScale);
            newScale.y = Mathf.Clamp(
                newScale.y,
                minimumScale,
                maximumScale);
            newScale.z = Mathf.Clamp(
                newScale.z,
                minimumScale,
                maximumScale);
            currentCylinder.transform.localScale =
                newScale;
        }
    }

    public void MakeCapsuleSmaller()
    {
        if (currentCapsule != null)
        {
            Vector3 newScale = currentCapsule.transform.localScale - Vector3.one * scaleAmount;
            newScale.x = Mathf.Clamp(
                newScale.x,
                minimumScale,
                maximumScale);
            newScale.y = Mathf.Clamp(
                newScale.y,
                minimumScale,
                maximumScale);
            newScale.z = Mathf.Clamp(
                newScale.z,
                minimumScale,
                maximumScale);
            currentCapsule.transform.localScale =
                newScale;
        }
    }

    public void MakePortalSmaller()
    {
        if (currentPortal != null)
        {
            Vector3 newScale = currentPortal.transform.localScale - Vector3.one * scaleAmount;
            newScale.x = Mathf.Clamp(
                newScale.x,
                minimumScale,
                maximumScale);
            newScale.y = Mathf.Clamp(
                newScale.y,
                minimumScale,
                maximumScale);
            newScale.z = Mathf.Clamp(
                newScale.z,
                minimumScale,
                maximumScale);
            currentPortal.transform.localScale =
                newScale;
        }
    }

    public void MakeOreSmaller()
    {
        if (currentOre != null)
        {
            Vector3 newScale = currentOre.transform.localScale - Vector3.one * scaleAmount;
            newScale.x = Mathf.Clamp(
                newScale.x,
                minimumScale,
                maximumScale);
            newScale.y = Mathf.Clamp(
                newScale.y,
                minimumScale,
                maximumScale);
            newScale.z = Mathf.Clamp(
                newScale.z,
                minimumScale,
                maximumScale);
            currentOre.transform.localScale =
                newScale;
        }
    }
    void ChangeObjectColor(GameObject obj, Color color)
    {
        if (obj != null)
        {
            Renderer renderer =
                obj.GetComponent<Renderer>();

            renderer.material =
                new Material(renderer.material);

            renderer.material.color = color;
        }
    }

}
