using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
public class Sound : MonoBehaviour
{

    public List<AudioSource> audios;
    Slider slider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        slider = GetComponent<Slider>();
        slider.value = PlayerPrefs.GetFloat("Volume",0.5f);
        slider.onValueChanged.AddListener((value) =>
        {
            PlayerPrefs.SetFloat("Volume", value);
        });
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < audios.Count; i++)
        {
            audios[i].volume = slider.value;
        }
    }
}
