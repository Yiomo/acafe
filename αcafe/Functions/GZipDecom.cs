using System.IO;
using System.IO.Compression;
using System.Text;

namespace αcafe.Functions
{
    class GZipDecom
    {
        public static string GZipDecompress(Stream stream)
        {
            byte[] buffer = new byte[100];
            int length = 0;

            GZipStream gz = new GZipStream(stream, CompressionMode.Decompress);
            MemoryStream msTemp = new MemoryStream();

            while ((length = gz.Read(buffer, 0, buffer.Length)) != 0)
            {
                msTemp.Write(buffer, 0, length);
            }
            return Encoding.UTF8.GetString(msTemp.ToArray());
        }
    }
}
