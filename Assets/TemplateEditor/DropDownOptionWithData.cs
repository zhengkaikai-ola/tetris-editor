using UnityEngine;
using UnityEngine.UI;

public class DropDownOptionWithData : Dropdown.OptionData
{
    public int data;

    public DropDownOptionWithData(int data)
    {
        this.data = data;
    }

    public DropDownOptionWithData(string text, int data) : base(text)
    {
        this.data = data;
    }

    public DropDownOptionWithData(Sprite image, int data) : base(image)
    {
        this.data = data;
    }

    public DropDownOptionWithData(string text, Sprite image, int data) : base(text, image)
    {
        this.data = data;
    }
}