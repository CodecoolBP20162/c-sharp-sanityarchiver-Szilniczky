using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SanityArchiveCC
{
    class Archive
    {
        public void Compress(FileInfo FileToCompress)
        {
            using(FileStream originalFileStream = FileToCompress.OpenRead())
            {
                if ((File.GetAttributes(FileToCompress.FullName) & FileAttributes.Hidden) != FileAttributes.Hidden & FileToCompress.Extension != ".gz")
                {
                    using(FileStream compressedFileStream = File.Create(FileToCompress.FullName + ".gz"))
                    {
                        using (GZipStream compressionStream = new GZipStream(compressedFileStream, CompressionMode.Compress))
                        {
                            originalFileStream.CopyTo(compressionStream);
                        }
                    }
                }
            }
        }

        public void Extract(FileInfo FileToExtract)
        {
            using(FileStream originalStream = FileToExtract.OpenRead())
            {
                string currentFileName = FileToExtract.FullName;
                string newFileName = currentFileName.Remove(currentFileName.Length - FileToExtract.Extension.Length);
                using (FileStream ExtractedFileStream = File.Create(newFileName))
                {
                    using (GZipStream ExtractionStream = new GZipStream(originalStream, CompressionMode.Decompress))
                    {
                        if (FileToExtract.Extension == ".gz")
                        {
                            ExtractionStream.CopyTo(ExtractedFileStream);
                        }
                    }
                }
            }
        }
    }
}
