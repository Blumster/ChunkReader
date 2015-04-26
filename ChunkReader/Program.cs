using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ChunkReader
{
    public class Program
    {
        public static StreamWriter Writer = new StreamWriter("ki.txt", false);

        public static String[] Extensions = { "geo", "anm", "fxi", "cat" };
        public static String[] NeedFrame = { "" };

        public static void Main(String[] args)
        {
            if (Writer != null)
                Writer.AutoFlush = true;

            var path = Environment.CurrentDirectory;
            if (args.Length > 0)
                path = args[0];

            if (!Directory.Exists(path))
            {
                Console.WriteLine("Invalid path: {0}", path);
                Console.WriteLine("Press enter to return...");
                Console.ReadLine();
                return;
            }

            var s = new HashSet<String>();

            foreach (var ext in Extensions)
            {
                foreach (var file in Directory.EnumerateFiles(path, String.Format("*.{0}", ext)))
                {
                    var reader = new StoChunkFileReader(file);
                    var frame = new StoChunkFrameReader(reader);

                    if (!reader.IsValid || !frame.IsValid || !reader.IsBinary)
                        continue;

                    var name = Encoding.UTF8.GetString(BitConverter.GetBytes(frame.Header.Name).Reverse().ToArray());
                    if (!s.Contains(name))
                    {
                        Console.WriteLine(name);
                        s.Add(name);
                    }

                    switch (name)
                    {
                        case "AEVT": // Anim Events
                        case "ANIM": // Anim Master
                        case "BBOX": // Bounding Box
                        case "BDAT": // Bone Shared Data
                        case "BIFX": // 
                        case "BVBX": // Bounding Volume Box
                        case "BVCP": // Bounding Volume Capsule
                        case "BVOL": // Bounding Volume
                        case "BVSP": // Bounding Volume Sphere
                        case "BVWS": // Bounding Volume Walkable Surface
                        case "CPDF": // CP Definition
                        case "CPDG": // CP Definition Group
                        case "CPFX": // Compile Fx
                        case "CTLG": // Catalog
                        case "DECL": // Vertex Decl
                        case "EFCT": // Effect
                        case "EVTB": // Event Base
                        case "GBOD": // Geometry Body
                        case "GMPH": // Geometry Piece Morphed
                        case "GPCE": // Geometry Piece
                        case "GSKN": // Geometry Piece Skinned
                        case "INDX": // Index Buffer
                        case "ISTR": // 
                        case "KERT": // KeyFrame RT
                        case "KESR": // KeyFrame SR
                        case "KEST": // KeyFrame ST
                        case "KEUV": // KeyFrame UV
                        case "KEYF": // KeyFrame F
                        case "KEYR": // KeyFrame R
                        case "KEYS": // KeyFrame S
                        case "KEYT": // KeyFrame T
                        case "KSRT": // Keyframe SRT
                        case "LDAA": // LOD Handler Auto Assault
                        case "LDSD": // LOD Simple Distance
                        case "MWGT": // Morph Weight
                        case "PARM": // Parameter
                        case "PBON": // Phy Bone
                        case "PFXD": // Precompiled FX Data
                        case "PSKE": // Phy Skeleton
                        case "TEVT": // Animation Track Events
                        case "TRAK": // Animation Track Master
                        case "USDA": // User Data
                        case "VERT": // Vertex Buffer
                            break;
                    }
                }
            }

            Console.ReadLine();
        }
    }
}
