using UnityEngine;
using UnityEngine.UI;
using Klak.TestTools;
using YoloV4Tiny;

class Visualizer : MonoBehaviour
{
    

    [SerializeField] getImage _source = null;
    public CpuImageSample cpuImage = null;
    [SerializeField, Range(0, 1)] float _threshold = 0.5f;
    [SerializeField] ResourceSet _resources = null;
    [SerializeField] RawImage _preview = null;
    [SerializeField] Marker _markerPrefab = null;
    [SerializeField] DepthMarker _3dMarkerPrefab = null;

    

    

    ObjectDetector _detector;
    Marker[] _markers = new Marker[50];
    DepthMarker[] _3dMarker = new DepthMarker[50];

    

    

    void Start()
    {
        _detector = new ObjectDetector(_resources);
        for (var i = 0; i < _markers.Length; i++)
            _markers[i] = Instantiate(_markerPrefab, _preview.transform);
        for (var i = 0; i < _3dMarker.Length; i++)
            _3dMarker[i] = Instantiate(_3dMarkerPrefab);
    }

    void OnDisable()
      => _detector.Dispose();

    void OnDestroy()
    {
        for (var i = 0; i < _markers.Length; i++) Destroy(_markers[i]);
    }

    void Update()
    {
        
            _detector.ProcessImage(_source.Texture, _threshold);

            var i = 0;
            foreach (var d in _detector.Detections)
            {
                if (i == _markers.Length) break;
                _markers[i++].SetAttributes(d);
                _3dMarker[i++].setDepth(d, cpuImage.depthTexture);
                
            
                //Debug.Log(d);
            }

            for (; i < _markers.Length; i++) _markers[i].Hide();

            _preview.texture = _source.Texture;
        }
           
    

    
}
