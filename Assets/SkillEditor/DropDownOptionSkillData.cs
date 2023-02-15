using UnityEngine;
using UnityEngine.UI;

public class DropDownOptionSkillData : Dropdown.OptionData
{
    public int data;

    public DropDownOptionSkillData(int data)
    {
        this.data = data;
    }

    public DropDownOptionSkillData(string text, int data) : base(text)
    {
        this.data = data;
    }

    public DropDownOptionSkillData(Sprite image, int data) : base(image)
    {
        this.data = data;
    }

    public DropDownOptionSkillData(string text, Sprite image, int data) : base(text, image)
    {
        this.data = data;
    }
}