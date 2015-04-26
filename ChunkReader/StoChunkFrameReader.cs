using System;
using System.IO;

namespace ChunkReader
{
    public class StoChunkFrameReader
    {
        public Boolean IsValid { get; private set; }
        public Int32 Result { get; private set; }
        public Boolean Mode { get; private set; } // false -> binary, true -> text
        public StoChunkFileReader Reader { get; private set; }
        public StoChunkHeader Header { get; private set; }

        public StoChunkFrameReader(StoChunkFileReader reader)
        {
            Reader = reader;
            IsValid = EnterChunk() >= 0;

            if (!IsValid)
                Console.WriteLine("Something happened! Reader: {0}", reader);
        }

        private Int32 EnterChunk()
        {
            if (!Reader.IsBinary)
            {
                if (Program.Writer != null)
                    Program.Writer.WriteLine(Path.GetFileName(Reader.FileStream.Name));

                Console.WriteLine("Text mode is not supported!");
                return 0;
            }

            StoChunkHeader header = null;
            var r = StoChunkHeader.Read(Reader.Reader, ref header);
            if ((r | Result) < 0)
                return -1;

            Header = header;

            return 0;
        }
    }

    public class StoChunkHeader
    {
        public UInt32 Name { get; private set; }
        public Int32 Size { get; private set; }
        public UInt32 Version { get; private set; }
        public Int32 Reserved { get; private set; }

        public static Int32 Read(BinaryReader reader, ref StoChunkHeader header)
        {
            if (reader.BaseStream.Length - reader.BaseStream.Position < 16)
                return -1;

            header = new StoChunkHeader
            {
                Name = reader.ReadUInt32(),
                Size = reader.ReadInt32(),
                Version = reader.ReadUInt32(),
                Reserved = reader.ReadInt32()
            };
            return 0;
        }
    }
}
