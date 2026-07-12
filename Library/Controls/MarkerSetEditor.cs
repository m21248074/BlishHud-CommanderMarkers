using Blish_HUD;
using Blish_HUD.Controls;
using Manlaan.CommanderMarkers.Presets.Model;
using Manlaan.CommanderMarkers.RtApi;
using Manlaan.CommanderMarkers.Utils;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Manlaan.CommanderMarkers.Library.Controls;

public class MarkerSetEditor : FlowPanel
{
    protected int _updateListingIndex = -1;
    Action<bool> _returnToList;

    protected MarkerSet _markerSet = new();
    protected StandardButton? _AddMarkerButton;
    protected StandardButton? _importAllButton;

    public MarkerSet MarkerSet { get => _markerSet; }
    public MarkerSetEditor(Action<bool> callback) : base()
    {
        ControlPadding = new Vector2(5, 5);
        _returnToList = callback;
    }

    public void LoadMarkerSet(MarkerSet? markerSet, int idx)
    {
        _markerSet = markerSet ?? new MarkerSet();
        _updateListingIndex = idx;
        ClearChildren();

        if (RtApiIntegrationHelper.IsEnabled)
        {
            Service.RtApiConnection?.EnsureActive();
        }

        var metaFlow = new FlowPanel()
        {
            Parent = this,
            FlowDirection = ControlFlowDirection.LeftToRight,
            ControlPadding = new Vector2(10,5),
            Size = new Point(450, 135),
        };
        
        new Label()
        {
            Parent = metaFlow,
            Text = "名稱",
            Size = new Point(99, 30),
            BasicTooltipText = "當您位於可使用此標記組合的範圍內時，地圖上將顯示此名稱"

        };
        var title = new TextBox()
        {
            Parent = metaFlow,
            Location = new Point(0, 0),
            Size = new Point(299, 30),
            Text = _markerSet.name,
            BasicTooltipText = "當您位於可使用此標記組合的範圍內時，地圖上將顯示此名稱"

        };
        title.TextChanged += (s, e) => _markerSet.name = title.Text;

        var Preview = new IconButton()
        {
            Parent = metaFlow,
            Icon = Service.Textures!.IconEye,
            BasicTooltipText = "預覽",
            Size = new Point(30, 30)
        };
        Preview.MouseEntered += (s, e) => Service.MapWatch.PreviewMarkerSet(_markerSet);
        Preview.MouseLeft += (s, e) => Service.MapWatch.RemovePreviewMarkerSet();

        new Label()
        {
            Parent = metaFlow,
            Text = "描述",
            Size = new Point(100, 30),
            BasicTooltipText = "當您在可使用此標記組合的範圍內時，這段文字將顯示在地圖上"
        };
       
        var description = new TextBox()
        {
            Parent = metaFlow,
            Location = new Point(0, 0),
            Size = new Point(300, 30),
            Text = _markerSet.description,
            BasicTooltipText = "當您在可使用此標記組合的範圍內時，這段文字將顯示在地圖上"

        };
        description.TextChanged += (s,e) => _markerSet.description = description.Text;

        new Label()
        {
            Parent = metaFlow,
            Size = new Point(100, 30),
            Text = "觸發位置",
            BasicTooltipText = "靠近此位置以啟用該標記組合"
        };
        var label = new Label()
        {
            Parent = metaFlow,
            Size = new Point(300, 30),
            Text = $"地圖: {Service.MapDataCache.Describe(_markerSet.MapId)}",
            BasicTooltipText = "設定觸發位置以更新地圖"
        };
        var triggerFields = new PositionFields(_markerSet.trigger)
        {
            Parent = metaFlow,
        };
        triggerFields.WorldCoordChanged += (s, e) =>
        {
            _markerSet.trigger = e;
            _markerSet.mapId = Gw2MumbleService.Gw2Mumble.CurrentMap.Id;
            label.Text = $"地圖: {Service.MapDataCache.Describe(_markerSet.MapId)}";
        };

        if (RtApiIntegrationHelper.IsEnabled)
        {
            _importAllButton = new StandardButton()
            {
                Parent = this,
                Text = "導入現有團隊標記",
                Width = 410,
                Icon = Service.Textures!.IconImport,
                BasicTooltipText = "從即時 API (Real-Time API) 複製當前已放置的團隊標記位置。\n需要安裝即時 API 插件。",
                Enabled = Service.RtApiConnection?.IsActive == true,
            };
            _importAllButton.Click += ImportAllButton_Click;
            if (Service.RtApiConnection != null)
            {
                Service.RtApiConnection.ConnectionStateChanged += OnRtApiConnectionStateChanged;
            }
        }

        _AddMarkerButton = new StandardButton()
        {
            Parent = this,
            Text = "新增標記",
            Width = 410,
            Enabled = _markerSet.marks.Count < 8
        };

        _markerSet.marks.ForEach(mark =>
        {
            new MarkerEditor(mark, RemoveMarker)
            {
                Parent = this,
            };
        });


        _AddMarkerButton.Click += (s, e) =>
        {
            if(_markerSet.marks.Count < 8)
            {
                var marker = new MarkerCoord();
                marker.SetFromMumbleLocation();
                _markerSet.marks.Add(marker);
                new MarkerEditor(marker, RemoveMarker) { Parent = this };
            }
            _AddMarkerButton.Enabled = _markerSet.marks.Count < 8;
            
        };
    }

    private void OnRtApiConnectionStateChanged(object? sender, RtApiConnectionState state)
    {
        if (_importAllButton != null)
        {
            _importAllButton.Enabled = state == RtApiConnectionState.Active;
        }
    }

    private void ImportAllButton_Click(object sender, Blish_HUD.Input.MouseEventArgs e)
    {
        if (Service.RtApiConnection == null || !Service.RtApiConnection.EnsureActive())
        {
            ScreenNotification.ShowNotification(
                "Real-Time API is not available.",
                ScreenNotification.NotificationType.Error,
                null,
                4);
            return;
        }

        var imported = new List<MarkerCoord>();
        for (var slotIndex = 0; slotIndex < RealTimeDataLayout.SquadMarkerSlotCount; slotIndex++)
        {
            var marker = new MarkerCoord();
            if (Service.RtApiConnection.TryImportSquadMarker(slotIndex, marker))
            {
                imported.Add(marker);
            }
        }

        if (imported.Count == 0)
        {
            ScreenNotification.ShowNotification(
                "No active squad markers were found to import.",
                ScreenNotification.NotificationType.Error,
                null,
                4);
            return;
        }

        _markerSet.marks = imported;
        _markerSet.mapId = Gw2MumbleService.Gw2Mumble.CurrentMap.Id;
        RebuildMarkerEditors();
    }

    private void RebuildMarkerEditors()
    {
        var markerEditors = Children.OfType<MarkerEditor>().ToList();
        foreach (var editor in markerEditors)
        {
            RemoveChild(editor);
            editor.Dispose();
        }

        foreach (var mark in _markerSet.marks)
        {
            new MarkerEditor(mark, RemoveMarker)
            {
                Parent = this,
            };
        }

        if (_AddMarkerButton != null)
        {
            _AddMarkerButton.Enabled = _markerSet.marks.Count < 8;
        }
    }

    protected void RemoveMarker(MarkerEditor editor)
    {
        Children.Remove(editor);
        _markerSet.marks.Remove(editor.Marker);
        _AddMarkerButton!.Enabled = _markerSet.marks.Count < 8;
        Invalidate();
    }

    protected override void DisposeControl()
    {
        if (Service.RtApiConnection != null)
        {
            Service.RtApiConnection.ConnectionStateChanged -= OnRtApiConnectionStateChanged;
        }

        base.DisposeControl();
    }

 }
