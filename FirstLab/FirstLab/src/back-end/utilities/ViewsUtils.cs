namespace FirstLab.src.back_end.utilities
{
    internal class ViewsUtils
    {
        public static void ChangeWindow<T>(MenuWindow menuWindowReference, string headerText, T view) where T : class
        {
            menuWindowReference.ViewsName.Text = headerText;
            menuWindowReference.contentControl.Content = view;
        }
    }
}