using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intech.Business.Tests
{
    [TestFixture]
    public class StreamManipulation
    {
        [Test]
        public void ReadAndWriteStream()
        {
            string path = Path.Combine(TestHelper.TestSupportPath, "TestFile.pdf");
            string pathTarget = path + " - copy";

            using( var fstream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None))
            using (var fout = new FileStream(pathTarget, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                Transfert(fstream, fout);
            }
        }

        [Test]
        public void CompressionDecompression()
        {
            string path = Path.Combine(TestHelper.TestSupportPath, "TestFile.pdf");
            string pathCompressed = path + " - gzip";
            string pathTarget = path + " - copy";

            using (var fstream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None))
            using (var fcompress = new FileStream(pathCompressed, FileMode.Create, FileAccess.Write, FileShare.None))
            using (var fcompressor = new GZipStream(fcompress, CompressionMode.Compress))
            {
                Transfert(fstream, fcompressor);
            }

            using (var fstream = new FileStream(pathCompressed, FileMode.Open, FileAccess.Read, FileShare.None))
            using (var fdecompress = new FileStream(pathTarget, FileMode.Create, FileAccess.Write, FileShare.None))
            using (var fcompressor = new GZipStream(fstream, CompressionMode.Decompress))
            {
                Transfert(fcompressor, fdecompress);
            }


        }

        private void Transfert(Stream fstream, Stream fout)
        {
            var buff = new byte[4000];
            int readedByte;

            while ((readedByte = fstream.Read(buff, 0, buff.Length)) > 0)
            {
                fout.Write(buff, 0, readedByte);
            }
        }
    }
}
