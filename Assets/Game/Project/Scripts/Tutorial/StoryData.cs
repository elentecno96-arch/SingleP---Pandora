using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStoryData", menuName = "Data/Story/StoryData")]
public class StoryData : ScriptableObject
{
    public List<StoryLine> lines = new List<StoryLine>();
}

[System.Serializable]
public class StoryLine
{
    public Sprite background;

    public string speakerName;

    [TextArea(3, 6)]
    public string dialogue;
}
