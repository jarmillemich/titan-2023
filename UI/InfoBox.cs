using Godot;
using System;

public class InfoBox : HBoxContainer
{

public void LoadText(string InfoText){
    RichTextLabel infotext = (RichTextLabel)GetNode("ColorRect/MarginContainer/RichTextLabel");
    infotext.Text = InfoText;
    infotext.Visible = true;
}
public void PreviewAbout(){
    RichTextLabel infotext = (RichTextLabel)GetNode("ColorRect/MarginContainer/RichTextLabel");
    infotext.Text = "HI";
    infotext.Visible = true;
}

public void _on_Button_button_down(){
    Visible = false;
}
}
