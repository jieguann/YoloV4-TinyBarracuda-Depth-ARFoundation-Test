using UnityEngine;
using UnityEngine.UI;
using YoloV4Tiny;

public class DepthMarker : MonoBehaviour
{
    public int width_depth = 120;
    public int height_depth = 160;

    public float depth;
    // Start is called before the first frame update
    void Start()
    {
        
    }

   public void setDepth(in Detection d, Texture2D depthImage)
    {
        Color[] depthPixels = depthImage.GetPixels();

        for (int index_dst = 0, depth_y = 0; depth_y < height_depth; depth_y++)
        {
            //index_dst = depth_y * width_depth;
            for (int depth_x = 0; depth_x < width_depth; depth_x++, index_dst++)
            {
                var x = (int)(d.x * width_depth);
                var y = (int)((1 - d.y) * height_depth);
                var depthIndex = x * (y - 1) + x;
                depth = depthPixels[depthIndex].r;

                transform.position = Camera.main.ScreenToWorldPoint(new Vector3(d.x*Screen.width, (1 - d.y) * Screen.height, depth));
                //transform.position = new Vector3(d.x, (1 - d.y), depth);

            }
        }
        gameObject.SetActive(true);
    }

    public void Hide()
      => gameObject.SetActive(false);
}
