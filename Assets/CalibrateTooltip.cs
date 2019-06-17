using System;
using UnityEngine;
using System.Text;
using Valve.VR;
using Matrix = MathNet.Numerics.LinearAlgebra.Matrix<float>;

public class CalibrateTooltip : MonoBehaviour {

    public GameObject controller;     // The controller to add a tooltip
    public Mesh tooltipMesh;          // The mesh to use
    public float tooltipSize=0.01f;   // Local scale of the mesh

    public SteamVR_Input_Sources handType;
    public SteamVR_Action_Boolean grabAction;


    // For export to python test script
    private StringBuilder rotBuilder;
    private StringBuilder posBuilder;

    // For creation of the tooltip object
    private GameObject tooltip;
    private Renderer meshRenderer;
    private int measures;

    // For the inverse calculation
    private Matrix m;
    private Matrix v;

    void Start ()
    {
        tooltip = new GameObject("Tooltip");
        tooltip.transform.parent = controller.transform;
        var meshFilter = tooltip.AddComponent<MeshFilter>();
        tooltip.AddComponent<MeshRenderer>();
        meshFilter.sharedMesh = tooltipMesh;

        meshRenderer = meshFilter.GetComponent<Renderer>();
        meshRenderer.material = new Material(Shader.Find(" Diffuse"));

        meshRenderer.material.color = new Color(0.9f, 0.1f, 0.1f);
        tooltip.transform.localScale = new Vector3(tooltipSize, tooltipSize, tooltipSize);
        Clear();
    }

    void Update ()
    {
        if (grabAction.GetStateDown(handType))
        {
            AddOne();
        }
	}

    private void AddOne()
    {
        measures++;
        Matrix4x4 mat = controller.transform.localToWorldMatrix;
  
        { // For debugging & verification in Python tool
            Debug.Log("Add point (" + measures+")");
            posBuilder.Append(controller.transform.localPosition.x + "," + controller.transform.localPosition.y + "," + controller.transform.localPosition.z + ",");
            rotBuilder.Append(mat.m00 + "," + mat.m01 + "," + mat.m02 + "," +
                mat.m10 + "," + mat.m11 + "," + mat.m12 + "," +
                mat.m20 + "," + mat.m21 + "," + mat.m22 + ",");
            Debug.Log(posBuilder.ToString());
            Debug.Log(rotBuilder.ToString());
        }

        Matrix row = Matrix.Build.Dense(3,3);
        row[0, 0] = -mat.m00;
        row[0, 1] = -mat.m01;
        row[0, 2] = -mat.m02;
        row[1, 0] = -mat.m10;
        row[1, 1] = -mat.m11;
        row[1, 2] = -mat.m12;
        row[2, 0] = -mat.m20;
        row[2, 1] = -mat.m21;
        row[2, 2] = -mat.m22;
        row = Matrix.Build.DenseIdentity(3).Append(row);

        Matrix col = Matrix.Build.Dense(3,1);
        col[0,0] = controller.transform.localPosition.x;
        col[1,0] = controller.transform.localPosition.y;
        col[2,0] = controller.transform.localPosition.z;

        if (measures==1)
        {
            m = row;
            v = col;
        }
        else
        {
            m = m.Stack(row);
            v = v.Stack(col);
        }

        if (measures >=4)
        {
            Matrix inv = m.PseudoInverse();
            Matrix res = inv.Multiply(v);
            tooltip.transform.localPosition = new Vector3(res[3, 0], res[4, 0], res[5, 0]);
            meshRenderer.material.color = new Color(0.1f, 0.9f, 0.1f);
            Debug.Log("Offset: "+ tooltip.transform.localPosition.x + " " + tooltip.transform.localPosition.y + " " + tooltip.transform.localPosition.z);
        }
    }

    public void Clear()
    {
        rotBuilder = new StringBuilder();
        posBuilder = new StringBuilder();
        measures = 0;
        tooltip.transform.localPosition = new Vector3(0f,0f,0f);
        meshRenderer.material.color = new Color(0.9f, 0.1f, 0.1f);
        Debug.Log("Reset");
    }
}
