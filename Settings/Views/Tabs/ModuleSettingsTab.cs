using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Settings;
using Manlaan.CommanderMarkers.Settings.Views.SubViews;
using Manlaan.CommanderMarkers.Utils;

namespace Manlaan.CommanderMarkers.Settings.Views.Tabs;

public class ModuleSettingsTab : ISettingsMenuRegistrar
{
    public event EventHandler<EventArgs>? RegistrarListChanged;
    private readonly List<MenuViewItem> _registeredMenuItems = new();
    
    public ModuleSettingsTab()
    {
#if DEBUG
        _registeredMenuItems.Add(new MenuViewItem(
            new MenuItem("Setup Wizard"),
            _ => new SetupWizardView()
        ));
#endif
        _registeredMenuItems.Add(new MenuViewItem(
            new MenuItem("可點擊標記"),
            _ => new MarkerPanelSettingsView()
        ));
        _registeredMenuItems.Add(new MenuViewItem(
            new MenuItem("自動標記設定"),
            _ => new AutoMarkerSettingsView()
        ));
        //Update index in ActivateLibraryTab
        _registeredMenuItems.Add(new MenuViewItem(
            new MenuItem("自動標記庫"),
            _ => new AutoMarkerLibraryView()
        ));
        _registeredMenuItems.Add(new MenuViewItem(
        new MenuItem("社群資料庫"),
            _ => new AutoMarkerCommunityLibraryView()
        ));
        _registeredMenuItems.Add(new MenuViewItem(
            new MenuItem("按鍵綁定"),
            _ => new KeybindSettingsView()
        ));

        _registeredMenuItems.Add(new MenuViewItem(
            new MenuItem("一般"),
            _ => new CornerIconSettingsView()
        ));
    }

    public void ActivateLibraryTab()
    {
        _registeredMenuItems[2].MenuItem.Select();
    }
    
    public IEnumerable<MenuItem> GetSettingMenus() => 
        _registeredMenuItems
        .Select(mi => mi.MenuItem);

    public IView? GetMenuItemView(MenuItem selectedMenuItem)
    {
        foreach (var (menuItem, viewFunc) in _registeredMenuItems) {
            if (menuItem == selectedMenuItem || menuItem.GetDescendants().Contains(selectedMenuItem)) {
                return viewFunc(selectedMenuItem);
            }
        }

        return null;
    }

}