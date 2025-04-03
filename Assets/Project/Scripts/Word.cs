using System.Collections.Generic;
using UnityEngine;

public class Word
{
    private string word;
    public List<bool> sections = new();

    public void Initialize(string word, List<bool> sections)
    {
        this.word = word;
        this.sections = sections;
    }
}
