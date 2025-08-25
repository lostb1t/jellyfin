#pragma warning disable
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ATL.Logging;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Library;
using MediaBrowser.Controller.MediaEncoding;
// using MediaBrowser.Controller.MediaInfo;
using MediaBrowser.Controller.LiveTv;
using MediaBrowser.Model.Dto;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.MediaInfo;
using Microsoft.Extensions.Logging;
using MediaBrowser.Controller.Persistence;


public sealed class ExternalMediaSourceProvider : IMediaSourceProvider
{
    private readonly ILogger<ExternalMediaSourceProvider> _log;
    private readonly IMediaSourceManager _mediaSourceManager;
    private readonly IMediaStreamRepository _mediaStreamRepository;

    public ExternalMediaSourceProvider(
        IMediaStreamRepository mediaStreamRepository,
        ILogger<ExternalMediaSourceProvider> log,
        IMediaSourceManager mediaSourceManager)
    {
        _log = log;
        _mediaSourceManager = mediaSourceManager;
        _mediaStreamRepository = mediaStreamRepository;
    }

    public async Task<IEnumerable<MediaSourceInfo>> GetMediaSources(BaseItem item, CancellationToken ct)
    {
        _log.LogInformation("ExternalMediaSourceProvider GetMediaSources for {Path}", item.Path ?? item.Name);
        // Only handle *your* virtual items — use any predicate you control:
        // e.g., custom scheme, provider id, or IsVirtualItem flag
        if (!item.IsVirtualItem || item.Path is null || !item.Path.StartsWith("https://your.cdn/", StringComparison.OrdinalIgnoreCase))
            return Array.Empty<MediaSourceInfo>();

        var items = new List<MediaSourceInfo>
            {
                new MediaSourceInfo
                {
                    Id = Guid.NewGuid().ToString("N"),
                    Name = "Test Stream",
                    Path = "https://test-videos.co.uk/vids/bigbuckbunny/mp4/h264/1080/Big_Buck_Bunny_1080_10s_30MB.mp4",
                    Protocol = MediaProtocol.Http,
                    Container = "mp4",
                    Size = 123456789,
                    Type = MediaSourceType.Placeholder,
                    IsRemote = true,
                    RequiresOpening = false,
                    MediaStreams = new List<MediaStream>
                    {
                        new MediaStream
                        {
                            Index = 0,
                            Codec = "h264",
                            Type = MediaStreamType.Video,
                            Width = 1280,
                            Height = 720,
                            IsDefault = true
                        }
                    }
                }
            };

            // _mediaStreamRepository.SaveMediaStreams(item.Id, items, ct);
        return items;
    }

    public Task<ILiveStream> OpenMediaSource(string mediaSourceId, List<ILiveStream> liveStreams, CancellationToken ct)
    {
        // This provider only returns static/remote sources and does not require an "open" handshake.
        // MediaSourceInfo instances we return have RequiresOpening = false, so this should not be called.
        throw new NotSupportedException("ExternalMediaSourceProvider does not open media sources.");
    }

};
