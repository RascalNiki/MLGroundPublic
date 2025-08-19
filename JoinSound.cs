using UdonSharp;
using UnityEngine;

public class JoinSound : UdonSharpBehaviour
{
    [SerializeField]
    private AudioSource audioSource;

    public void OnPlayerJoined()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
}
