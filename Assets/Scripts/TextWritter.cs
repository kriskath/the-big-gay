using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;

public class TextWritter : MonoBehaviour
{

    private float delay = .05f;
    private string text;
    private string currentText = "";
    private AudioSource voice;
    private Regex regex;


    void OnEnable()
    {
        StartCoroutine(ShowText());
    }

    void OnDisable()
    {
        currentText = "";
    }

    void Start()
    {
        regex = new Regex("^[a-zA-Z0-9]*$");
        voice = GetComponent<AudioSource>();
        text = GetComponent<TextMeshPro>().text;
        StartCoroutine(ShowText());
    }

    IEnumerator ShowText()
    {
        for (int i = 0; i < text.Length; i++) 
        {
            string currentLetter = text[i].ToString();
            SpeakText(currentLetter);
            GetComponent<TextMeshPro>().text = currentText += currentLetter;
            yield return new WaitForSeconds(delay);
        }
    }

    void SpeakText(string currentLetter)
    {
        if (voice && regex.IsMatch(currentLetter)) 
        {
            voice.Play();
        }
    }
}
