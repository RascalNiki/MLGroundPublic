using UdonSharp;
using UnityEngine;
using VRC.SDK3.Image;
using VRC.SDK3.UdonNetworkCalling;
using VRC.SDKBase;
using VRC.Udon.Common.Interfaces;

public class Leaderboard : UdonSharpBehaviour
{
    [SerializeField]
    private VRCUrl imageUrl;
    private Renderer _renderer;
    private VRCImageDownloader _imageDownloader;
    private IUdonEventReceiver _udonEventReceiver;

    void Start()
    {
        _renderer = GetComponent<Renderer>();
        DownloadImage();
    }

    public override void Interact()
    {
        SendCustomNetworkEvent(NetworkEventTarget.All, nameof(DownloadImage));
    }

    [NetworkCallable]
    public void DownloadImage()
    {
        _imageDownloader = new VRCImageDownloader();
        _udonEventReceiver = (IUdonEventReceiver)this;

        var rgbInfo = new TextureInfo();
        rgbInfo.GenerateMipMaps = true;
        _imageDownloader.DownloadImage(imageUrl, _renderer.material, _udonEventReceiver, rgbInfo);
    }
}
