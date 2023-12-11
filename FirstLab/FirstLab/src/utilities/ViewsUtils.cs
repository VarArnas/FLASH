using FirstLab.src.controllers;

namespace FirstLab.src.utilities;

public class ViewsUtils
{
    public static MenuWindow? menuWindowReference;

    public static void ChangeWindow<T>(string headerText, T view) where T : class
    {
        menuWindowReference!.ViewsName.Text = headerText;
        menuWindowReference.contentControl.Content = view;
    }
}