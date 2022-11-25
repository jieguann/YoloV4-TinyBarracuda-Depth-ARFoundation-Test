using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class getDepth : MonoBehaviour
{
    //public RenderTexture depthVideoTexture;
    //public RenderTexture colorVideoTexture;
    //public ParticleSystem ps;
    public CpuImageSample cpuImage;

    public float depthValue = 100;

    [Range(0, 1)] public float depthRange = 0.7f; //The value to show the reange of depth


    Mesh mesh;
    
    Vector3[] vertices = null;
    Color[] colors = null;
    private Vector2[] uvs;
    private int[] indices;
    private int[] triangles;


    public int width_depth;
    public int height_depth;
    

    Texture2D m_DepthTexture_Float;
    Texture2D m_ColorTexture_Float;

    


   


    void Start()
    {



        mesh = new Mesh();
        mesh.name = "Point Cloud";
        if (cpuImage.depthTexture != null)
        {
            m_DepthTexture_Float = cpuImage.depthTexture;
            m_ColorTexture_Float = cpuImage.depthTexture;


        }
        



        if (vertices == null || colors == null)
        {

            vertices = new Vector3[width_depth * height_depth];

            uvs = new Vector2[vertices.Length];



            colors = new Color[vertices.Length];
            indices = new int[vertices.Length];
            for (int i = 0; i < vertices.Length; i++)
            {
                indices[i] = i;


            }


        }
    }

    // Update is called once per frame
    void Update()
    {


        ReprojectPointCloud();
        

    }

    void ReprojectPointCloud()
    {   //Call the convert video to texture2d function

        if (cpuImage.depthTexture!=null)
        {
            m_DepthTexture_Float = cpuImage.depthTexture;
            m_ColorTexture_Float = cpuImage.depthTexture;
        }
        

        
        //raw.texture = m_DepthTexture_Float;

        


        //Mesh generation

        //print("Depth:" + m_DepthTexture_Float.width + "," + m_DepthTexture_Float.height);
        //print("Color:" + m_CameraTexture.width + "," + m_CameraTexture.height);




        Color[] depthPixels = m_DepthTexture_Float.GetPixels();


        //int index_dst;
        float depth;

        for (int index_dst = 0, depth_y = 0; depth_y < height_depth; depth_y++)
        {
            //index_dst = depth_y * width_depth;
            for (int depth_x = 0; depth_x < width_depth; depth_x++, index_dst++)
            {

                //colors[index_dst] = m_DepthTexture_Float.GetPixelBilinear((float)depth_x / (width_depth), (float)depth_y / (height_depth));
                colors[index_dst] = m_DepthTexture_Float.GetPixelBilinear((float)width_depth, (float)height_depth);
                //print(depthPixels[20].r);
                //depth = depthPixels[index_dst].r;


                //to showing the vertices since the r value is too small

                depth = depthPixels[index_dst].r * depthValue;
                //print(depthPixels[5]);

                vertices[index_dst].z = depth;
                //vertices[index_dst].z = 100;
                vertices[index_dst].x = depth_x;
                vertices[index_dst].y = depth_y;

                //index_dst++;

                //Set UV
                uvs[index_dst] = new Vector2((float)depth_x / (width_depth - 1), (float)depth_y / (height_depth - 1));


            }
        }


        //mesh vertices 
        mesh.vertices = vertices;
        //showing the point of the vertices(for testing, when generate the mesh can be commented)
        //mesh.SetIndices(indices, MeshTopology.Points, 0); 
        //apply uv
        mesh.uv = uvs;

        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;




        //mesh
        /*
        int[] triangles = new int[3];
        triangles[0] = 0;
        triangles[1] = width_depth;
        triangles[2] = 1;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        */

        int[] triangles = new int[(width_depth - 1) * (height_depth - 1) * 6]; //-1 since the mesh is one less then the width and heigh size of the pixel image 
                                                                               //int[] triangles = new int[width_depth * height_depth * 6];

        for (int ti = 0, vi = 0, y = 0; y < height_depth - 1; y++, vi++)
        {
            for (int x = 0; x < width_depth - 1; x++, ti += 6, vi++)
            {
                //IF statement
                //Attempt to show a part of the mesh

                
                    triangles[ti] = vi;
                    triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                    triangles[ti + 4] = triangles[ti + 1] = vi + width_depth;
                    triangles[ti + 5] = vi + width_depth + 1;

            }
        }
        mesh.triangles = triangles;
        //mesh.SetIndices(triangles, MeshTopology.Triangles, 0, false);
        mesh.RecalculateNormals();

    }


    


    //Function For convert RenderTexture(Video File) to Texture2d
    //https://stackoverflow.com/questions/44264468/convert-rendertexture-to-texture2d
    Texture2D toTexture2D(RenderTexture rTex)
    {
        Texture2D tex = new Texture2D(256, 256, TextureFormat.RGBA32, false);
        RenderTexture.active = rTex;
        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();
        return tex;
    }


    

    
}
