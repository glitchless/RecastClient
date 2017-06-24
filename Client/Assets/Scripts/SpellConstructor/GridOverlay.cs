using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridOverlay : MonoBehaviour {
    public bool show = true;

    public int gridSizeX;
    public int gridSizeY;
    public int gridSizeZ;

    public float step;

    public float startX;
    public float startY;
    public float startZ;

    private Material lineMaterial;

    public Color mainColor = new Color(0f, 1f, 0f, 1f);

    void CreateLineMaterial() {
        if (!lineMaterial) {
            // Unity has a built-in shader that is useful for drawing
            // simple colored things.
            var shader = Shader.Find("Hidden/Internal-Colored");
            lineMaterial = new Material(shader);
            lineMaterial.hideFlags = HideFlags.HideAndDontSave;
            // Turn on alpha blending
            lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            // Turn backface culling off
            lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            // Turn off depth writes
            lineMaterial.SetInt("_ZWrite", 0);
        }
    }

    void OnPostRender() {
        CreateLineMaterial();
        // set the current material
        lineMaterial.SetPass(0);

        GL.Begin(GL.LINES);

        if (step == 0)
            Debug.Log("Step is 0, wont draw Grid. Step it to higher than 0.");

        if (show) {
            GL.Color(mainColor);

            //Layers
            for (float j = 0; j <= gridSizeY; j += step) {
                //X axis lines
                for (float i = 0; i <= gridSizeZ; i += step) {
                    GL.Vertex3(startX, startY + j, startZ + i);
                    GL.Vertex3(startX + gridSizeX, startY + j, startZ + i);
                }

                //Z axis lines
                for (float i = 0; i <= gridSizeX; i += step) {
                    GL.Vertex3(startX + i, startY + j, startZ);
                    GL.Vertex3(startX + i, startY + j, startZ + gridSizeZ);
                }
            }

            //Y axis lines
            for (float i = 0; i <= gridSizeZ; i += step) {
                for (float k = 0; k <= gridSizeX; k += step) {
                    GL.Vertex3(startX + k, startY, startZ + i);
                    GL.Vertex3(startX + k, startY + gridSizeY, startZ + i);
                }
            }
        }

        GL.End();
    }
}
