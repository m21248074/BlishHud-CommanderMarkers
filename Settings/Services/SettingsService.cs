using Blish_HUD.Input;
using Blish_HUD.Settings;
using Manlaan.CommanderMarkers.Library.Enums;
using Manlaan.CommanderMarkers.Localization;
using Manlaan.CommanderMarkers.Settings.Enums;
using Manlaan.CommanderMarkers.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace Manlaan.CommanderMarkers.Settings.Services;

public class SettingService: IDisposable // singular because Setting"s"Service already exists in Blish
{
    public SettingEntry<bool> _settingGroundMarkersEnabled { get; private set; }
    public SettingEntry<bool> _settingTargetMarkersEnabled { get; private set; }
    public SettingEntry<KeyBinding> _settingArrowGndBinding { get; private set; }
    public SettingEntry<KeyBinding> _settingCircleGndBinding { get; private set; }
    public SettingEntry<KeyBinding> _settingHeartGndBinding { get; private set; }
    public SettingEntry<KeyBinding> _settingSpiralGndBinding { get; private set; }
    public SettingEntry<KeyBinding> _settingSquareGndBinding { get; private set; }
    public SettingEntry<KeyBinding> _settingStarGndBinding { get; private set; }
    public SettingEntry<KeyBinding> _settingTriangleGndBinding { get; private set; }
    public SettingEntry<KeyBinding> _settingXGndBinding { get; private set; }
    public SettingEntry<KeyBinding> _settingClearGndBinding { get; private set; }
    public SettingEntry<KeyBinding> _settingArrowObjBinding { get; private set; }
    public SettingEntry<KeyBinding> _settingCircleObjBinding { get; private set; }
    public SettingEntry<KeyBinding> _settingHeartObjBinding { get; private set; }
    public SettingEntry<KeyBinding> _settingSpiralObjBinding { get; private set; }
    public SettingEntry<KeyBinding> _settingSquareObjBinding { get; private set; }
    public SettingEntry<KeyBinding> _settingStarObjBinding { get; private set; }
    public SettingEntry<KeyBinding> _settingTriangleObjBinding { get; private set; }
    public SettingEntry<KeyBinding> _settingXObjBinding { get; private set; }
    public SettingEntry<KeyBinding> _settingClearObjBinding { get; private set; }
    public SettingEntry<KeyBinding> _settingInteractKeyBinding { get; private set; }

    public SettingEntry<Point> _settingLoc { get; private set; }
    public SettingEntry<Layout> _settingOrientation { get; private set; }
    public SettingEntry<int> _settingImgWidth { get; private set; }
    public SettingEntry<float> _settingOpacity { get; private set; }
    public SettingEntry<bool> _settingDrag { get; private set; }
    public SettingEntry<bool> _settingShowMarkersPanel{ get; private set; }
    public SettingEntry<bool> _settingOnlyWhenCommander { get; private set; }

    //public SettingEntry<VisibleOnMap> _settingMapVisible { get; private set; }
    public SettingEntry<int> AutoMarker_PlacementDelay { get; private set; }
    public SettingEntry<bool>AutoMarker_OnlyWhenCommander { get; private set; }
    public SettingEntry<bool> AutoMarker_FeatureEnabled { get; private set; }
    public SettingEntry<bool> AutoMarker_LibraryFilterToCurrent { get; private set; }
    public SettingEntry<bool> AutoMarker_LibraryFilterMine { get; private set; }
    public SettingEntry<bool> AutoMarker_ShowPreview { get; private set; }
    public SettingEntry<bool> AutoMarker_ShowTrigger { get; private set; }
    public SettingEntry<bool> AutoMarker_Allow_Combat_Placement { get; private set; }
    public SettingEntry<bool> RtApiIntegrationEnabled { get; private set; }
    public SettingEntry<bool> AutoMarker_Billboard_FeatureEnabled { get; private set; }
    public SettingEntry<bool> AutoMarker_Billboard_Placement { get; private set; }
    public SettingEntry<bool> AutoMarker_Billboard_Preview { get; private set; }
    public SettingEntry<bool> CornerIconEnabled { get; private set; }
    public SettingEntry<CornerIconActions> CornerIconLeftClickAction { get; private set; }
    public SettingEntry<int> CornerIconPriority { get; }
    public SettingEntry<SquadMarker> CornerIconTexture { get; }

    public SettingService(SettingCollection settings)
    {

        CornerIconPriority = settings.DefineSetting("CmdMrkCornerPriority",
            Constants.CornerIcon.DEFAULT_PRIORITY,
            () => "左上角圖示排序順序",
            () => "左 <----> 右");
        CornerIconPriority.SetRange(Constants.CornerIcon.MIN_PRIORITY, Constants.CornerIcon.MAX_PRIORITY);

        CornerIconTexture = settings.DefineSetting("CmdMrkCornerTexture",
            SquadMarker.Heart,
            () => "左上角圖示影像",
            () => "選擇一個要顯示在左上角圖示列的標記");
        CornerIconTexture.SetExcluded(new SquadMarker[]{SquadMarker.None, SquadMarker.Clear});

        _settingGroundMarkersEnabled = settings.DefineSetting("CmdMrkGnnEnabled", true, () => "顯示用於放置地面標記的圖示", () => "");
        _settingTargetMarkersEnabled = settings.DefineSetting("CmdMrkTgtEnabled", true, () => "顯示用於放置目標/物件標記的圖示", () => "");
        _settingArrowGndBinding = settings.DefineSetting("CmdMrkArrowGndBinding", new KeyBinding(ModifierKeys.Alt, Keys.D1), () => "Arrow Ground Binding", () => "");
        _settingArrowGndBinding = settings.DefineSetting("CmdMrkArrowGndBinding", new KeyBinding(ModifierKeys.Alt, Keys.D1), () => "Arrow Ground Binding", () => "");
        _settingCircleGndBinding = settings.DefineSetting("CmdMrkCircleGndBinding", new KeyBinding(ModifierKeys.Alt, Keys.D2), () => "Circle Ground Binding", () => "");
        _settingHeartGndBinding = settings.DefineSetting("CmdMrkHeartGndBinding", new KeyBinding(ModifierKeys.Alt, Keys.D3), () => "Heart Ground Binding", () => "");
        _settingSquareGndBinding = settings.DefineSetting("CmdMrkSquareGndBinding", new KeyBinding(ModifierKeys.Alt, Keys.D4), () => "Square Ground Binding", () => "");
        _settingStarGndBinding = settings.DefineSetting("CmdMrkStarGndBinding", new KeyBinding(ModifierKeys.Alt, Keys.D5), () => "Star Ground Binding", () => "");
        _settingSpiralGndBinding = settings.DefineSetting("CmdMrkSpiralGndBinding", new KeyBinding(ModifierKeys.Alt, Keys.D6), () => "Spiral Ground Binding", () => "");
        _settingTriangleGndBinding = settings.DefineSetting("CmdMrkTriangleGndBinding", new KeyBinding(ModifierKeys.Alt, Keys.D7), () => "Triangle Ground Binding", () => "");
        _settingXGndBinding = settings.DefineSetting("CmdMrkXGndBinding", new KeyBinding(ModifierKeys.Alt, Keys.D8), () => "X Ground Binding", () => "");
        _settingClearGndBinding = settings.DefineSetting("CmdMrkClearGndBinding", new KeyBinding(ModifierKeys.Alt, Keys.D9), () => "Clear Ground Binding", () => "");

        _settingArrowObjBinding = settings.DefineSetting("CmdMrkArrowObjBinding", new KeyBinding(ModifierKeys.Alt | ModifierKeys.Shift, Keys.D1), () => "Arrow Object Binding", () => "");
        _settingCircleObjBinding = settings.DefineSetting("CmdMrkCircleObjBinding", new KeyBinding(ModifierKeys.Alt | ModifierKeys.Shift, Keys.D2), () => "Circle Object Binding", () => "");
        _settingHeartObjBinding = settings.DefineSetting("CmdMrkHeartObjBinding", new KeyBinding(ModifierKeys.Alt | ModifierKeys.Shift, Keys.D3), () => "Heart Object Binding", () => "");
        _settingSquareObjBinding = settings.DefineSetting("CmdMrkSquareObjBinding", new KeyBinding(ModifierKeys.Alt | ModifierKeys.Shift, Keys.D4), () => "Square Object Binding", () => "");
        _settingStarObjBinding = settings.DefineSetting("CmdMrkStarObjBinding", new KeyBinding(ModifierKeys.Alt | ModifierKeys.Shift, Keys.D5), () => "Star Object Binding", () => "");
        _settingSpiralObjBinding = settings.DefineSetting("CmdMrkSpiralObjBinding", new KeyBinding(ModifierKeys.Alt | ModifierKeys.Shift, Keys.D6), () => "Spiral Object Binding", () => "");
        _settingTriangleObjBinding = settings.DefineSetting("CmdMrkTriangleObjBinding", new KeyBinding(ModifierKeys.Alt | ModifierKeys.Shift, Keys.D7), () => "Triangle Object Binding", () => "");
        _settingXObjBinding = settings.DefineSetting("CmdMrkXObjBinding", new KeyBinding(ModifierKeys.Alt | ModifierKeys.Shift, Keys.D8), () => "X Object Binding", () => "");
        _settingClearObjBinding = settings.DefineSetting("CmdMrkClearObjBinding", new KeyBinding(ModifierKeys.Alt | ModifierKeys.Shift, Keys.D9), () => "Clear Object Binding", () => "");

        _settingInteractKeyBinding = settings.DefineSetting("CmdMrkInteractBinding", new KeyBinding(Keys.F), () => "Interact Key", () => "");


        _settingLoc = settings.DefineSetting("CmdMrkLoc", new Point(100, 100), () => "Location", () => "");

        _settingOrientation = settings.DefineSetting("CmdMrkOrientation2", Layout.Horizontal, () => "排列方式", () => "");
        _settingImgWidth = settings.DefineSetting("CmdMrkImgWidth", Constants.UI.DEFAULT_ICON_SIZE, () => "圖示大小", () => "設定畫面上標記圖示的大小");
        _settingOpacity = settings.DefineSetting("CmdMrkOpacity", Constants.UI.DEFAULT_OPACITY, () => "透明度", () => "設定面板透明度\n隱藏<---->顯示");
        _settingDrag = settings.DefineSetting("CmdMrkDrag", false, () => "重新調整標記位置 - 啟用拖曳", () => "允許重新調整可點擊標記的位置");
        _settingShowMarkersPanel = settings.DefineSetting("CmdMrkShowMarkerPanelr", true, () => "在畫面上顯示可點擊的標記", () => "隱藏/顯示可點擊標記面板");
        _settingOnlyWhenCommander = settings.DefineSetting(
            "CmdMrkOnlyCommander",
            false,
            () => "僅在我是指揮官時顯示",
            () => "當您不是指揮官時，隱藏可點擊標記");
        AutoMarker_PlacementDelay = settings.DefineSetting(
            "CmdMrkPlacementDelay",
            Constants.AutoMarker.DEFAULT_PLACEMENT_DELAY_MS,
            () => "放置延遲",
            () => "放置標記之間的等待延遲\n較快 <-----> 較慢"
            );

        //_settingMapVisible = settings.DefineSetting("CmdMrkShow", VisibleOnMap.HideOnMap, ()=>"Show on map", () => "");

        AutoMarker_PlacementDelay.SetRange(Constants.AutoMarker.MIN_PLACEMENT_DELAY_MS, Constants.AutoMarker.MAX_PLACEMENT_DELAY_MS);
        _settingImgWidth.SetRange(Constants.UI.MIN_ICON_SIZE, Constants.UI.MAX_ICON_SIZE);
        _settingOpacity.SetRange(Constants.UI.MIN_OPACITY, Constants.UI.MAX_OPACITY);


        AutoMarker_OnlyWhenCommander = settings.DefineSetting(
            "CmdMrkAMOnlyCommander",
            true,
            () => "僅在身為指揮官時顯示",
            () => "當您是指揮官時，僅在地圖上顯示自動標記啟動區"
        );
        AutoMarker_FeatureEnabled = settings.DefineSetting(
            "CmdMrkAMEnabled",
            true,
            () => "啟用",
            () => "啟用/停用整個自動標記功能"
        );
        AutoMarker_LibraryFilterToCurrent = settings.DefineSetting(
            "CmdMrkAMLibraryFilter",
            false,
            () => "Current map",
            () => "Filter the library list to only show marker sets for your current map"
        );
        AutoMarker_LibraryFilterMine = settings.DefineSetting(
            "CmdMrkAMLibraryFilterMine",
            false,
            () => "Mine",
            () => "Hide marker sets imported from the community library"
        );
        AutoMarker_ShowPreview = settings.DefineSetting(
            "CmdMrkAMShowPreview",
            true,
            () => "在地圖開啟時顯示預覽",
            () => "當您開啟地圖，且距離足夠近以放置標記組合時，顯示標記預覽"
        );
        AutoMarker_ShowTrigger = settings.DefineSetting(
            "CmdMrkAMShowTrigger",
            true,
            () => "啟用地圖標記",
            () => "在可以從地圖啟用自動標記組合的位置，顯示 Blish HUD 的標記圖示"
        );

        AutoMarker_Billboard_FeatureEnabled = settings.DefineSetting(
            "CmdMrkBillboardEnabled",
            true,
            () => "啟用 3D 遊戲世界中的標記",
            () => "在 3D 遊戲世界中顯示標記"
        );
        
        AutoMarker_Billboard_Placement = settings.DefineSetting(
            "CmdMrkAMCanBypassMapOpen",
            true,
            () => "允許在不開啟地圖的情況下放置",
            () => "即使在關閉地圖時，也允許放置標記"
        );

        AutoMarker_Billboard_Preview = settings.DefineSetting(
            "CmdMrkBillboardPreview",
            true,
            () => "當靠近觸發點時預覽標記組合",
            () => "在遊戲世界中顯示即將放置的標記預覽"
        );

        AutoMarker_Allow_Combat_Placement = settings.DefineSetting(
            "CmdMrkCombatPlacement",
            false,
            () => "允許在戰鬥中使用自動標記功能",
            () => "自動標記功能將可在戰鬥中運作"
        );

        RtApiIntegrationEnabled = settings.DefineSetting(
            "CmdMrkRtApiEnabled",
            false,
            () => "啟用 Raidcore.GG Nexus 即時 API 整合",
            () => "使用 RTAPI (透過 Nexus 遊戲記憶體讀取) 在資料庫編輯器中匯入當前小隊標記位置"
        );

        CornerIconEnabled = settings.DefineSetting(
            "CmdMrkCornerIconEnabled",
            true,
            () => "在左上角選單列顯示圖示",
            () => "在左上角選單列新增捷徑圖示"
        );

        CornerIconLeftClickAction = settings.DefineSetting(
            "CmdMrkAMCornerIconAction",
            CornerIconActions.SHOW_ICON_MENU,
            () => "圖示左鍵點擊動作",
            () => "選擇選單列圖示左鍵點擊的動作\n右鍵點擊將永遠開啟小型選單"
        );
    }

    public void Dispose()
    {

    }
}