using UnityEngine;
using Unity.InferenceEngine;
using Unity.AppUI.UI;
using System.ComponentModel;

public class DepthCameraManager : MonoBehaviour
{
    [Tooltip("For debugging, a material to show the camera image")]
    public Material colourImageMaterial;

    [Tooltip("For debugging, a material to show the depth image")]
    public Material depthImageMaterial;

    [Tooltip("For debugging, a material to show the mask image")]
    public Material maskImageMaterial;

    public Material holoMaterial;
    public int maskThreshold; // threshold between masked and unmasked pixels from depth image

    [Tooltip("The depth estimation model")]
    public ModelAsset estimationModel;

    private WebCamTexture webcamTexture;
    public int webcamIndex = 0;
    private Worker depthCameraWorker;
    private Tensor<float> inputTensor;
    private RenderTexture depthTexture;

    private void updateWebCam()
    {
        if (webcamTexture == null)
        {
            webcamTexture = new WebCamTexture();
        }

        if (!webcamTexture.isPlaying)
        {
            if (colourImageMaterial != null)
            {
                colourImageMaterial.mainTexture = webcamTexture;
            }
            webcamTexture.Play();
        }
    }

    public Texture getColourTexture()
    {
        return webcamTexture;
    }

    public Texture getDepthTexture()
    {
        return depthTexture;
    }

    private void loadModel()
    {
        var model = ModelLoader.Load(estimationModel);

        var graph = new FunctionalGraph();
        var inputs = graph.AddInputs(model);
        FunctionalTensor[] outputs = Functional.Forward(model, inputs);
        var output = outputs[0];
        FunctionalTensor max0 = Functional.ReduceMax(output, new int[] { 0, 1, 2 }, false);
        FunctionalTensor min0 = Functional.ReduceMin(output, new int[] { 0, 1, 2 }, false);
        FunctionalTensor maxMin = Functional.Sub(max0, min0);
        FunctionalTensor outputMin = Functional.Sub(output, min0);
        FunctionalTensor output2 = Functional.Div(outputMin, maxMin);
        model = graph.Compile(output2);

        depthCameraWorker = new Worker(model, BackendType.GPUCompute);
        depthTexture = new RenderTexture(256, 256, 0, RenderTextureFormat.ARGBFloat);
        inputTensor = new Tensor<float>(new TensorShape(1, 3, 256, 256), true);
    }

    private void getDepth()
    {
        TextureConverter.ToTensor(webcamTexture, inputTensor, new TextureTransform());
        depthCameraWorker.Schedule(inputTensor);

        var output = depthCameraWorker.PeekOutput() as Tensor<float>;
        output.Reshape(output.shape.Unsqueeze(0));
        TextureConverter.RenderToTexture(output as Tensor<float>, depthTexture, new TextureTransform().SetCoordOrigin(CoordOrigin.TopLeft));

        if (depthImageMaterial != null)
        {
            depthImageMaterial.mainTexture = depthTexture;
        }

        if (maskImageMaterial != null)
        {
            maskImageMaterial.mainTexture = depthTexture;
        }

        if (holoMaterial != null)
        {
            holoMaterial.mainTexture = depthTexture;
            holoMaterial.SetTexture("_maskTexture", depthTexture);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        loadModel();
    }

    // Update is called once per frame
    void Update()
    {
        updateWebCam();
        getDepth();
    }

    private void Destroy()
    {
        depthCameraWorker.Dispose();
        inputTensor.Dispose();
        depthTexture.Release();
    }
}
