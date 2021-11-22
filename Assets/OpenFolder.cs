using UnityEngine;
public class OpenFolder : MonoBehaviour
{
    GameObject scripts;
    void Start()
    {
        scripts = FindObjectOfType<AddPlaylist>().gameObject;
    }
    public void NextFolder()
    {
        scripts.GetComponent<AddPlaylist>().OpenNextFolder(transform.GetChild(1).name);
    }
}