using UnityEngine;
using System.Collections;

// It will also be intantiate in the scene and therefore must inherit from monobehaviour
public class MapDisplayer : MonoBehaviour {

    // Here we will need to set a reference of the renderer of the plane in the scene to be used
    // later on the set its texture

    public Renderer textureRenderer;
    // a public reference to a mesh filter
    public MeshFilter meshFilter;
    // a public reference to a mesh renderer
    public MeshRenderer meshRenderer;

    public void DrawTextureToScreen(Texture2D texture)
    {
        textureRenderer.sharedMaterial.mainTexture = texture;
        textureRenderer.transform.localScale = new Vector3(texture.width, 1, texture.height);
    }

    public void DrawMeshToScreen(MeshData meshData, Texture2D texture)
    {
        meshFilter.sharedMesh = meshData.CreateMesh();
        meshRenderer.sharedMaterial.mainTexture = texture;
    }
}
