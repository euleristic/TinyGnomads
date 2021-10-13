using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class VoiceRecognition : MonoBehaviour
{
    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, Action> actions = new Dictionary<string, Action>();

    private void Start()
    {
        //actions.Add("Fuck you", FuckYou);
        //actions.Add("Love gnomes no bounds", Love);
        //keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray());
        //dictationRecognizer = new DictationRecognizer();
        //dictationRecognizer.Start()
        //dictationRecognizer.DictationResult += DictationRecognizer_DictationResult;

        string[] strings = new string[] { "Yes", "No", "yeah", "waiter", "uh", "ok", "raid shadow legends", "Muricah" };


        keywordRecognizer = new KeywordRecognizer(strings);
        keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
        keywordRecognizer.Start();
    }

    private void DictationRecognizer_DictationResult(string text, ConfidenceLevel confidence)
    {
        Debug.Log(text);
    }

    private void RecognizedSpeech(PhraseRecognizedEventArgs speech)
    {
        Debug.Log(speech.text);
    }
}