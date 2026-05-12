using Unity.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PointClouder : MonoBehaviour
{
    [Header("Debug")]
    [Tooltip("Optional output string for debugging.")]
    public TextMesh logger;

    [Header("Voxel Setup")]
    [Tooltip("Shape to place around each target point.")]
    public GameObject nodeShape;

    [Tooltip("An object to collect all voxel game objects under.")]
    public GameObject voxelParent;

    [Header("UI")]
    [Tooltip("The label on the capture button - changes to enable/disable.")]
    public TMP_Text captureMessage;

    [Header("AR Foundation References")]
    [Tooltip("The AR camera, source of colours for the marker points.")]
    public Camera arCamera;

    [Tooltip("AR Camera Manager on the AR camera.")]
    public ARCameraManager cameraManager;

    [Tooltip("Switch off view of the background to see the octree more easily.")]
    public ARCameraBackground background;

    [Tooltip("AR Point Cloud Manager on the XR Origin.")]
    public ARPointCloudManager pointCloudManager;

    private OctTree tree;
    private bool addVoxels = false;

    void Start()
    {
        tree = new OctTree(nodeShape);

        if (captureMessage != null)
        {
            captureMessage.text = "Enable capture";
        }

        if (logger != null)
        {
            logger.text = "Ready";
        }
    }

    public void toggleCapture()
    {
        addVoxels = !addVoxels;

        if (background != null)
        {
            background.enabled = true;
        }

        if (captureMessage != null)
        {
            captureMessage.text = addVoxels ? "Disable capture" : "Enable capture";
        }
    }

    void Update()
    {
        if (pointCloudManager == null)
        {
            SetLogger("Missing ARPointCloudManager");
            return;
        }

        if (arCamera == null)
        {
            SetLogger("Missing AR Camera");
            return;
        }

        bool foundAnyPoints = false;

        foreach (ARPointCloud pointCloud in pointCloudManager.trackables)
        {
            if (!pointCloud.positions.HasValue)
                continue;

            NativeSlice<Vector3> positions = pointCloud.positions.Value;

            if (positions.Length <= 0)
                continue;

            foundAnyPoints = true;

            if (addVoxels)
            {
                ProcessPointCloud(positions);
            }
        }

        if (foundAnyPoints)
        {
            SetLogger("Have Points");

            if (voxelParent != null)
            {
                tree.renderOctTree(voxelParent);
            }
        }
        else
        {
            SetLogger("No Points");
        }
    }

    private void ProcessPointCloud(NativeSlice<Vector3> positions)
    {
        Texture2D cameraTexture = null;

        if (cameraManager != null &&
            cameraManager.TryAcquireLatestCpuImage(out XRCpuImage cpuImage))
        {
            cameraTexture = ConvertCpuImageToTexture(cpuImage);
            cpuImage.Dispose();
        }

        for (int i = 0; i < positions.Length; i++)
        {
            Vector3 worldPoint = positions[i];

            Color colour = Color.white;
            bool foundColour = false;

            if (cameraTexture != null)
            {
                Vector3 viewportPoint = arCamera.WorldToViewportPoint(worldPoint);

                if (viewportPoint.z > 0 &&
                    viewportPoint.x >= 0f && viewportPoint.x <= 1f &&
                    viewportPoint.y >= 0f && viewportPoint.y <= 1f)
                {
                    int x = Mathf.RoundToInt(viewportPoint.x * (cameraTexture.width - 1));
                    int y = Mathf.RoundToInt(viewportPoint.y * (cameraTexture.height - 1));

                    colour = cameraTexture.GetPixel(x, y);
                    foundColour = true;
                }
            }

            if (foundColour)
            {
                tree.addPoint(worldPoint, colour);
            }
        }

        if (cameraTexture != null)
        {
            Destroy(cameraTexture);
        }
    }

    private Texture2D ConvertCpuImageToTexture(XRCpuImage image)
    {
        XRCpuImage.ConversionParams conversionParams =
            new XRCpuImage.ConversionParams
            {
                inputRect = new RectInt(0, 0, image.width, image.height),
                outputDimensions = new Vector2Int(image.width, image.height),
                outputFormat = TextureFormat.RGBA32,
                transformation = XRCpuImage.Transformation.MirrorY
            };

        int size = image.GetConvertedDataSize(conversionParams);
        NativeArray<byte> buffer = new NativeArray<byte>(size, Allocator.Temp);

        image.Convert(conversionParams, buffer);

        Texture2D texture = new Texture2D(
            conversionParams.outputDimensions.x,
            conversionParams.outputDimensions.y,
            conversionParams.outputFormat,
            false
        );

        texture.LoadRawTextureData(buffer);
        texture.Apply();

        buffer.Dispose();

        return texture;
    }

    private void SetLogger(string message)
    {
        if (logger != null)
        {
            logger.text = message;
        }
    }
}