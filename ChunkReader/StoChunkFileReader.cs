using System;
using System.Text;
using System.IO;

namespace ChunkReader
{
    public class StoChunkFileReader
    {
        public FileStream FileStream { get; private set; }
        public BinaryReader Reader { get; private set; }
        public Boolean IsBinary { get; private set; }
        public Boolean IsValid { get; private set; }

        public StoChunkFileReader(String file)
        {
            FileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
            Reader = new BinaryReader(FileStream);

            ReadHeader();
        }

        private void ReadHeader()
        {
            var strHeader = Encoding.UTF8.GetString(Reader.ReadBytes(4));
            if (strHeader != "CHNK")
            {
                IsValid = false;
                return;
            }

            var opts = Reader.ReadBytes(4);
            IsBinary = opts[0] == 66;
            IsValid = IsBinary && opts[1] == 76;
        }

        public override String ToString()
        {
            return String.Format("StoChunkFileReader - File: {0}", Path.GetFileName(FileStream.Name));
        }
    }
}
