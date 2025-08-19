using UdonSharp;
using UnityEngine;
using VRC.SDK3.Image;
using VRC.SDKBase;
using VRC.Udon.Common.Interfaces;

public class Leaderboard : UdonSharpBehaviour
{
    public VRCUrl imageUrl;
    public new Renderer renderer;
    private VRCImageDownloader _imageDownloader;
    private IUdonEventReceiver _udonEventReceiver;

    void Start()
    {
        DownloadImage();
    }

    public override void Interact()
    {
        DownloadImage();
    }

    private void DownloadImage()
    {
        _imageDownloader = new VRCImageDownloader();
        _udonEventReceiver = (IUdonEventReceiver)this;

        var rgbInfo = new TextureInfo();
        rgbInfo.GenerateMipMaps = true;
        _imageDownloader.DownloadImage(imageUrl, renderer.material, _udonEventReceiver, rgbInfo);
    }
}