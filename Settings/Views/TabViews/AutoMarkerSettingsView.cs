using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Manlaan.CommanderMarkers.Settings.Controls;
using Manlaan.CommanderMarkers.Settings.Services;
using Manlaan.CommanderMarkers.Utils;
using Microsoft.Xna.Framework;
using System;

namespace Manlaan.CommanderMarkers.Settings.Views.SubViews;

public class AutoMarkerSettingsView : View
{
    protected SettingService _settings = null!;

    protected override void Build(Container buildPanel)
    {
        _settings = Service.Settings;

        base.Build(buildPanel);

        var panel = new FlowPanel()
            .BeginFlow(buildPanel)
            .AddString("自動標記功能")
            .AddString("允許快速放置儲存的標記組合。")
            .AddSpace()
            .AddSetting(_settings.AutoMarker_FeatureEnabled)
            .AddSetting(_settings.AutoMarker_OnlyWhenCommander)
            .AddSpace()
            .AddString("地圖圖示")
            .AddSetting(_settings.AutoMarker_ShowTrigger)
            .AddSetting(_settings.AutoMarker_ShowPreview)
            .AddSpace()
            .AddString("遊戲世界圖示")
            .AddSetting(_settings.AutoMarker_Billboard_FeatureEnabled)
            .AddSetting(_settings.AutoMarker_Billboard_Placement)
            .AddSetting(_settings.AutoMarker_Billboard_Preview)
            .AddSpace()
            .AddSetting(_settings.AutoMarker_Allow_Combat_Placement)
            .AddSpace()
            .AddSetting(_settings.AutoMarker_PlacementDelay)
            .AddFlowControl(new Label()
            {
                Text = $"  延遲時間: {_settings.AutoMarker_PlacementDelay.Value} 毫秒",

                AutoSizeWidth = true,
            }, out var delayLabel)
            ;


        new Label()
        {
            Parent = buildPanel,
            Text = "按住 Ctrl 和 Shift 鍵以啟用按鈕",
            AutoSizeWidth = true,
            Location = new Point(0, buildPanel.Height - 65)
        };
        var ResetButton = new NuclearOptionButton()
        {
            Parent = buildPanel,
            Text = $"將資料庫重設為預設值",
            BasicTooltipText = "警告：此操作將刪除您資料庫中的所有標記組合，並還原為預設標記。\n\n按住 Ctrl 和 Shift 鍵以啟用按鈕",
            Width = 200,
            Location = new Point(0, buildPanel.Height - 35)
        };
        ResetButton.Click += (s, e) =>
        {
            Service.MarkersListing.ResetToDefault();
            ScreenNotification.ShowNotification("AutoMarker Library has been reset", ScreenNotification.NotificationType.Gray, null, 4);

        };

        new Image()
        {
            Parent = buildPanel,
            Texture = Service.Textures!._blishHeart,
            Size = new Point(96, 96),
            Location = new Point(buildPanel.Width - 96, buildPanel.Height - 96),
        };

        _settings.AutoMarker_PlacementDelay.SettingChanged += (s, e) =>
        {
            if (delayLabel is Label label)
            {
                label.Text = $"  Delay Time: {_settings.AutoMarker_PlacementDelay.Value} ms";
            }
        };
    }
}