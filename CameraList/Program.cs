// See https://aka.ms/new-console-template for more information
using CameraList;
using DirectShowLib;
using System.Runtime.InteropServices;
var devices = new List<DsDevice>(DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice));
foreach (var device in devices)
{
    //Basic info
    Console.WriteLine("DEVICE:");
    Console.WriteLine("Name: {0}", device.Name);
    Console.WriteLine("DevicePath: {0}", device.DevicePath);
    Console.WriteLine("ClassID: {0}", device.ClassID);

    //Filter
    var iBaseFilterGuid = Guid.Parse("56a86895-0ad4-11ce-b03a-0020af0ba770");
    device.Mon.BindToObject(null, null, ref iBaseFilterGuid, out object filter);
    var baseFilter = (IBaseFilter)filter;

    //Pins
    Console.WriteLine("Pins:");
    baseFilter.EnumPins(out IEnumPins filterPins);
    IPin[] pins = new IPin[1];
    IntPtr fetched = new IntPtr();
    int pinNo = 0;
    while (filterPins.Next(1, pins, fetched) == 0)
    {
        Console.WriteLine("\tPin {0}", pinNo);

        var pin = pins[0];
        if (pin.QueryPinInfo(out PinInfo pinInfo) == 0)
        {
            Console.WriteLine("\tName: {0}", pinInfo.name);
            Console.WriteLine("\tDirection: {0}", pinInfo.dir);
        }

        //Media types
        pin.EnumMediaTypes(out IEnumMediaTypes filterMediaTypes);
        AMMediaType[] aMMediaTypes = new AMMediaType[1];
        int mediaTypeNo = 0;
        Console.WriteLine("\tMedia Types:");
        while (filterMediaTypes.Next(1, aMMediaTypes, fetched) == 0)
        {
            Console.WriteLine("\t\tMediaType {0}", mediaTypeNo);
            var mediaType = aMMediaTypes[0];
            if (mediaType.majorType.Equals(MediaType.Video))
            {
                Console.WriteLine("\t\tMajor type: Video");
            }
            else
            {
                Console.WriteLine("\t\tMajor type: unknown ({0})", mediaType.majorType);
            }
            //TODO: sub type to string
            Console.WriteLine("\t\tSub type: {0}", DirectShowUtils.MediaSubTypeToString(mediaType.subType));
            Console.WriteLine("\t\tFixed size samples: {0}", mediaType.fixedSizeSamples);
            Console.WriteLine("\t\tTemporal compression: {0}", mediaType.temporalCompression);
            Console.WriteLine("\t\tSample size: {0}", mediaType.sampleSize);
            Console.WriteLine("\t\tFormat size: {0}", mediaType.formatSize);
            if (mediaType.formatType.Equals(FormatType.VideoInfo2))
            {
                Console.WriteLine("\t\tFormat type: VideoInfoHeader2");
                VideoInfoHeader2 header = (VideoInfoHeader2)Marshal.PtrToStructure(mediaType.formatPtr, typeof(VideoInfoHeader2));
                Console.WriteLine("\t\t\tAvgTimePerFrame: {0}", header.AvgTimePerFrame);
                Console.WriteLine("\t\t\tBitrate: {0}", header.BitRate);
                Console.WriteLine("\t\t\tBitErrorRate: {0}", header.BitErrorRate);
                Console.WriteLine("\t\t\tControl flags: {0}", header.ControlFlags.ToString());
                Console.WriteLine("\t\t\tInterlace flags: {0}", header.InterlaceFlags.ToString());
                Console.WriteLine("\t\t\tPictAspectRatioX: {0}", header.PictAspectRatioX);
                Console.WriteLine("\t\t\tPictAspectRationY: {0}", header.PictAspectRatioY);
                Console.WriteLine("\t\t\tBmiHeader:");
                Console.WriteLine("\t\t\t\tWidth: {0}", header.BmiHeader.Width);
                Console.WriteLine("\t\t\t\tHeight: {0}", header.BmiHeader.Height);
                Console.WriteLine("\t\t\t\tSize: {0}", header.BmiHeader.Size);
                Console.WriteLine("\t\t\t\tImageSize: {0}", header.BmiHeader.ImageSize);
                Console.WriteLine("\t\t\t\tBitCount: {0}", header.BmiHeader.BitCount);
                Console.WriteLine("\t\t\t\tPlanes: {0}", header.BmiHeader.Planes);
                Console.WriteLine("\t\t\t\tCrlImportant: {0}", header.BmiHeader.ClrImportant);
                Console.WriteLine("\t\t\t\tCrlUsed: {0}", header.BmiHeader.ClrUsed);
                Console.WriteLine("\t\t\t\tCompression: {0}", DirectShowUtils.CompressionToString(header.BmiHeader.Compression));
            }
            else if (mediaType.formatType.Equals(FormatType.VideoInfo))
            {
                Console.WriteLine("\t\tFormat type: VideoInfoHeader");
                VideoInfoHeader header = (VideoInfoHeader)Marshal.PtrToStructure(mediaType.formatPtr, typeof(VideoInfoHeader));
                Console.WriteLine("\t\t\tAvgTimePerFrame: {0}", header.AvgTimePerFrame);
                Console.WriteLine("\t\t\tBitrate: {0}", header.BitRate);
                Console.WriteLine("\t\t\tBitErrorRate: {0}", header.BitErrorRate);
                Console.WriteLine("\t\t\tBmiHeader:");
                Console.WriteLine("\t\t\t\tWidth: {0}", header.BmiHeader.Width);
                Console.WriteLine("\t\t\t\tHeight: {0}", header.BmiHeader.Height);
                Console.WriteLine("\t\t\t\tSize: {0}", header.BmiHeader.Size);
                Console.WriteLine("\t\t\t\tImageSize: {0}", header.BmiHeader.ImageSize);
                Console.WriteLine("\t\t\t\tBitCount: {0}", header.BmiHeader.BitCount);
                Console.WriteLine("\t\t\t\tPlanes: {0}", header.BmiHeader.Planes);
                Console.WriteLine("\t\t\t\tCrlImportant: {0}", header.BmiHeader.ClrImportant);
                Console.WriteLine("\t\t\t\tCrlUsed: {0}", header.BmiHeader.ClrUsed);
                Console.WriteLine("\t\t\t\tCompression: {0}", DirectShowUtils.CompressionToString(header.BmiHeader.Compression));
            }
            else
            {
                Console.WriteLine("\t\t Format type: INVALID - {0}", mediaType.formatType);
            }
            mediaTypeNo++;
        }

        pinNo++;
    }
}
