using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class DateSpeachHandler : MonoBehaviour
{
    [System.Serializable]
    public class Preference
    {
        [Header("Replace option with \"[BLANK]\"")]
        [SerializeField] string statement;
        [SerializeField] string question;
        [SerializeField] int favorite;
        [SerializeField] List<string> options;
        public void RandomizeFavorite()
        {
            favorite = Random.Range(0, options.Count);
            
        }

        public string GetStatement(string option = "")
        {
            return statement.Replace("[BLANK]", option == "" ? options[favorite] : option);
        }
        
        public string GetQuestion(string option = "")
        {
            return question.Replace("[BLANK]", option == "" ? options[favorite] : option);
        }
    }

    [SerializeField] TextMeshPro text;
    [SerializeField] SpriteRenderer bubble;
    public List<Preference> preferences = new List<Preference>();
    float duration = 0f;
    float timer = 0f;

    private void Start()
    {
        DisplayQuestion(0, 1f);
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= duration)
        {
            duration = Mathf.Infinity;
            text.enabled = false;
            bubble.enabled = false;
        }

        
    }
    public void DisplayStatement(int index, float duration = Mathf.Infinity)
    {
        if (!text.enabled)
        {
            text.enabled = true;
            bubble.enabled = true;
        }
        this.duration = duration;
        text.text = preferences[index].GetStatement();
    }

    public void DisplayQuestion(int index, float duration = Mathf.Infinity)
    {
        if (!text.enabled)
        {
            text.enabled = true;
            bubble.enabled = true;
        }
        this.duration = duration;
        text.text = preferences[index].GetQuestion();
    }

   
}
