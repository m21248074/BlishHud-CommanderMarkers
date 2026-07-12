using System.ComponentModel;

namespace Manlaan.CommanderMarkers.Settings.Enums;

public enum CornerIconActions
{
    [Description("顯示快速存取選單")]
    SHOW_ICON_MENU,

    [Description("顯示設定視窗")]
    SHOW_SETTINGS,

    [Description("開啟標記資料庫")]
    LIBRARY,

    [Description("副官模式")]
    LIEUTENANT,

    [Description("切換標記面板可見性")]
    CLICKMARKER_TOGGLE,


}