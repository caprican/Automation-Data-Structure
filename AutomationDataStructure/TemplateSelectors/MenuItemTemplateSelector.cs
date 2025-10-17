using System.Windows;
using System.Windows.Controls;

using MahApps.Metro.Controls;

namespace AutomationDataStructure.TemplateSelectors;

public class MenuItemTemplateSelector : DataTemplateSelector
{
    public DataTemplate GlyphDataTemplate { get; set; } = new DataTemplate();
    public DataTemplate ImageDataTemplate { get; set; } = new DataTemplate();
    public DataTemplate IconDataTemplate { get; set; } = new DataTemplate();

    public override DataTemplate SelectTemplate(object item, DependencyObject container)
    {
        switch (item)
        {
            case HamburgerMenuGlyphItem:
                return GlyphDataTemplate;
            case HamburgerMenuImageItem:
                return ImageDataTemplate;
            case HamburgerMenuIconItem:
                return IconDataTemplate;
            default:
                return base.SelectTemplate(item, container);
        }
    }
}