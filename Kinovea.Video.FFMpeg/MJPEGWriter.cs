using System;
using System.Drawing;
using Kinovea.Services;

namespace Kinovea.Video.FFMpeg
{
    public class SavingContext
    {
    }

    public class MJPEGWriter : IDisposable
    {
        public MJPEGWriter()
        {
        }

        public RecordingResult OpenSavingContext(RecordingSettings recordingSettings)
        {
            return RecordingResult.Success;
        }

        public RecordingResult CloseSavingContext(bool encodingSuccess)
        {
            return RecordingResult.Success;
        }

        public bool SaveFrame(ImageFormat format, byte[] buffer, long payloadLength, bool topDown)
        {
            return true;
        }

        public bool WrapAndWrite(SavingContext context, byte[] managedBuffer, long length)
        {
            return true;
        }

        public bool EncodeAndWrite(SavingContext context, byte[] managedBuffer, long length, bool topDown)
        {
            return true;
        }

        public void Dispose()
        {
        }
    }

    public class VideoReaderFFMpeg : VideoReader
    {
        public override VideoCapabilities Flags => VideoCapabilities.None;
        public override VideoDecodingMode DecodingMode => VideoDecodingMode.OnDemand;
        public override VideoFrame Current => null;
        public override VideoSection WorkingZone => new VideoSection();
        public override VideoInfo Info => new VideoInfo();
        public override VideoGeometry Geometry => new VideoGeometry();
        public override CacheSnapshot CacheSnapshot => new CacheSnapshot(0, new VideoSection[0]);
        public override bool Loaded => false;

        public override OpenVideoResult Open(string path) => OpenVideoResult.Success;
        public override void Close() { }
        public override VideoSummary ExtractSummary(string path, int count, Size size) => new VideoSummary("");
        public override bool PlayerRequest(PlayerState newState) => false;
        public override bool MoveRequest(bool next, long timestamp) => false;
        public override bool UpdateVideoGeometry(VideoGeometryRequest request) => false;
    }
}
