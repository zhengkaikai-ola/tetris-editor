using UnityEngine;
using UnityEngine.Serialization;

public class UIEventCenter : MonoBehaviour
{
    public InitMapEditorScript InitMapEditorScript;
    public TaskEditorScript TaskEditorScript;
    public GenerateBlockEditor GenerateBlockEditorScript;
    public TemplateEditorScript TemplateEditorScript;
    public LevelSelectorScript LevelSelectorScript;


    // Start is called before the first frame update

    void Start()
    {
    }
}