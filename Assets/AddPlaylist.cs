using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Threading;
public class AddPlaylist : MonoBehaviour
{
    public AudioSource audio1;
    public Button button1;
    public Button buttonFolder;
    public GameObject buttons;
    public GameObject playlist;
    public GameObject main;
    public GameObject folderSearch;
    Vector3 vector1 = new Vector3(0,910,0);
    Vector3 vectorPlaylist = new Vector3(0,910,0);
    public List<Button> AllButtons = new List<Button>();
    float soundLength = 0;
    public Transform slider;
    public Transform sliderStart;
    public Transform sliderFinish;
    public Transform newSlider;
    float startX;
    float finishX;
    float fullDistans;
    void Start()
    {
        startX = sliderStart.position.x;
        finishX = sliderFinish.position.x;
        fullDistans = Vector2.Distance(sliderStart.position,sliderFinish.position);
        StartCoroutine(ViewPlay());
    }
    void Update()
    {
        if(main.activeSelf == true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                newSlider.gameObject.SetActive(true);
            }
            if (Input.GetMouseButton(0))
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos.y = 0;
                mousePos.z = 0;
                if(mousePos.x < startX)
                {
                    mousePos.x = startX;
                }
                if(mousePos.x > finishX)
                {
                    mousePos.x = finishX;
                }
                newSlider.position = mousePos;
            }
            if(Input.GetMouseButtonUp(0))
            {
                float value = (1/(fullDistans/Vector2.Distance(sliderStart.position,newSlider.position)));
                audio1.timeSamples = (int)(soundLength*value);
                newSlider.gameObject.SetActive(false);
            }
        }
    }
    IEnumerator ViewPlay()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            float value = 1/(soundLength / audio1.timeSamples);
            if(float.IsNaN(value) == false)
            {
                slider.position = Vector3.Lerp(sliderStart.position, sliderFinish.position, value);
            }
        }
    }
    public void SearchFolder()
    {
        folderSearch.SetActive(true);
        main.SetActive(false);
        playlist.SetActive(false);
        // OpenNextFolder("/storage/emulated/0/");
        
        var nameDrive = DriveInfo.GetDrives();
        foreach (var item in nameDrive)
        {
            AddFolder(item.Name);
        }
        vector1.y = 910;
    }
    public IEnumerator PlayAudioClip(string path)
    {
        string fullPath = "file://" + path;
        UnityWebRequest file = UnityWebRequestMultimedia.GetAudioClip(fullPath, AudioType.MPEG);
        yield return file.SendWebRequest();
        if (file.result != UnityWebRequest.Result.ConnectionError)
        {
            AudioClip myClip = DownloadHandlerAudioClip.GetContent(file);
            audio1.clip = myClip;
            soundLength = myClip.samples;
            audio1.Play();
        }
    }
    public void OpenPlaylist()
    {
        playlist.SetActive(true);
        main.SetActive(false);
        folderSearch.SetActive(false);
    }
    public void ClosePlaylist()
    {
        playlist.SetActive(false);
        main.SetActive(true);
        folderSearch.SetActive(false);
    }
    public void CloseFolder()
    {
        playlist.SetActive(false);
        main.SetActive(true);
        folderSearch.SetActive(false);
        DestroyButton();
    }
    public void AddNewPlaylist(string path)
    {
        var test = Directory.GetFiles(path);
        foreach (var item in test)
        {
            if (Path.GetExtension(item) == ".mp3")
            {
                var bb = Instantiate(button1,buttons.transform);
                bb.transform.localPosition = vectorPlaylist;
                // bb.transform.GetChild(0).GetComponent<Text>().text = Path.GetFileNameWithoutExtension(item);
                bb.transform.GetChild(0).GetComponent<Text>().text = item;
                bb.transform.GetChild(1).name = item;
                vectorPlaylist.y -= 100;
            }
        }
        // vectorPlaylist.y = 910; добавить удаление плейлиста.
    }
    public void AddFolder(string path)
    {
        var bb = Instantiate(buttonFolder,folderSearch.transform);
        AllButtons.Add(bb);
        bb.transform.localPosition = vector1;
        bb.transform.GetChild(0).GetComponent<Text>().text = path;
        bb.transform.GetChild(1).name = path;
        vector1.y -= 100;
    }
    public void OpenNextFolder(string path)
    {
        try
        {
            var test = Directory.GetDirectories(path);
            DestroyButton();
            if(test.Length == 0)
            {
                AddNewPlaylist(path);
                OpenPlaylist();
            }
            else
            {
                foreach (var item in test)
                {
                    AddFolder(item);
                }
            }
            vector1.y = 910;
        }
        catch
        {

        }
    }
    void DestroyButton()
    {
        foreach (var item in AllButtons)
        {
            Destroy(item.gameObject);
        }
        AllButtons.Clear();
    }
}