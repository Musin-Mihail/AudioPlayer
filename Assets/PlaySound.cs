using UnityEngine;
public class PlaySound : MonoBehaviour
{
    GameObject scripts;
    void Start()
    {
        scripts = FindObjectOfType<AddPlaylist>().gameObject;
    }
    public void PlaySound2()
    {
        StartCoroutine(scripts.GetComponent<AddPlaylist>().PlayAudioClip(transform.GetChild(1).name));
    }
}