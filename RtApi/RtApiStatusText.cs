namespace Manlaan.CommanderMarkers.RtApi;

public static class RtApiStatusText
{
    public static string ForState(RtApiConnectionState state)
    {
        return state switch
        {
            RtApiConnectionState.Active => "RTAPI: 已啟用",
            RtApiConnectionState.Inactive => "RTAPI: 已偵測到(未啟用)",
            _ => "RTAPI: 未偵測到",
        };
    }
}
