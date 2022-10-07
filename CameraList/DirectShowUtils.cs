using DirectShowLib;

namespace CameraList
{
    internal class DirectShowUtils
    {
        private static Dictionary<Guid, string> MediaSubtypesMap = new Dictionary<Guid, string> { { MediaSubType.RGB24, "RGB24" }, { MediaSubType.YUYV, "YUYV" }, { MediaSubType.NV12, "NV12" }, { MediaSubType.YUY2, "YUY2" }, { MediaSubType.IYUV, "IYUV" }, { MediaSubType.MJPG, "MJPG" }, { MediaSubType.I420, "I420" } };
      
        public static string MediaSubTypeToString(Guid mediaSubType)
        {
            if (DirectShowUtils.MediaSubtypesMap.ContainsKey(mediaSubType))
            {
                return DirectShowUtils.MediaSubtypesMap[mediaSubType];
            }

            return mediaSubType.ToString();
        }

        public static string CompressionToString(int compression)
        {
            if (compression == 0) { return "BI_RGB"; }
            if (compression == 3) { return "BI_BITMAP"; }
            if (compression == 844715353) { return "YUY2"; }
            if (compression == 808596553) { return "I420"; }
            if (compression == 842094158) { return "NV12"; }
            if (compression == 1498831189) { return "UYVY"; }
            if (compression == 1196444237) { return "MJPEG"; }
            return compression.ToString();
        }
    }
}
