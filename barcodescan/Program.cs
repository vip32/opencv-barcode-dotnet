using System;
using OpenCvSharp;
using Dynamsoft.Barcode;
using System.Runtime.InteropServices;

namespace rtsp
{
    public static class Program
    {
        static void Main(string[] args)
        {
            var capture = new VideoCapture(0);
            if (capture.IsOpened())
            {
                var reader = new BarcodeReader("t0068NQAAALfRKISvetQErM8rtinv0UJdCkmkjMcAjgNx+tRuNksIpgAKSAfNq3H4EP6Ynbk0nOoNgDgCIgMi6zheV0CGPuQ=");
                while (true)
                {
                    var mat = capture.RetrieveMat();
                    Cv2.ImShow("video", mat);

                    int key = Cv2.WaitKey(20);
                    if (key == 27) // 'ESC'
                    {
                        break;
                    }

                    // Read barcode
                    var data = mat.Data;
                    int width = mat.Width;
                    int height = mat.Height;
                    int elemSize = mat.ElemSize();
                    int buffer_size = width * height * elemSize;
                    byte[] buffer = new byte[buffer_size];
                    Marshal.Copy(data, buffer, 0, buffer_size);
                    var results = reader.DecodeBuffer(buffer, width, height, width * elemSize, EnumImagePixelFormat.IPF_RGB_888, "");
                    if (results != null)
                    {
                        Console.WriteLine($"Total results: #{results.Length}");
                        foreach (var result in results)
                        {
                            Console.WriteLine($"[{result.BarcodeFormat}] {result.BarcodeText}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No barcode detected");
                    }
                }
            }
        }
    }
}
