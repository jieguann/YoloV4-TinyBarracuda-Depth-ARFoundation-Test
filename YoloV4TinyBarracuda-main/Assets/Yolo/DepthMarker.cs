using UnityEngine;
using UnityEngine.UI;
using YoloV4Tiny;
using TMPro;
public class DepthMarker : MonoBehaviour
{
    public int width_depth = 120;
    public int height_depth = 160;

    public float depth;
    public TMP_Text text;

    public static string[] _labels = new[]
    {
        "Plane", "Bicycle", "Bird", "Boat",
        "Bottle", "Bus", "Car", "Cat",
        "Chair", "Cow", "Table", "Dog",
        "Horse", "Motorbike", "Person", "Plant",
        "Sheep", "Sofa", "Train", "TV"
    };
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
                depth = depthPixels[depthIndex].grayscale;

                transform.position = Camera.main.ScreenToWorldPoint(new Vector3(d.x*Screen.width, (1 - d.y) * Screen.height, depth));
                //transform.position = new Vector3(d.x, (1 - d.y), depth);

            }
        }
        var name = _labels[(int)d.classIndex];
        text.text = $"{name} {(int)(d.score * 100)}%" + "x:" + $"{d.x}" + "y:" + $"{d.y}";

        gameObject.SetActive(true);
    }

    public void Hide()
      => gameObject.SetActive(false);
}
