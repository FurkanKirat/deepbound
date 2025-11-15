using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Core;
using Systems.SaveSystem.Interfaces;
using Utils;

namespace Systems.SaveSystem
{
    public static class SaveHelper
    {
        private const string MagicNumber = "UDSF";
        private static readonly byte[] MagicNumberBytes = Encoding.ASCII.GetBytes(MagicNumber); // 4 byte

        public static void Save<T>(T obj, string path, SaveType saveType, SaveOptions options = SaveOptions.Compress | SaveOptions.Hash)
        {
            // JSON serialize
            if (options == SaveOptions.None)
            {
                JsonHelper.SaveRaw(path, obj);
                return;
            }
            string json = JsonHelper.Serialize(obj);
            
            var data = Encoding.UTF8.GetBytes(json);

            // Compression 
            if (options.HasFlag(SaveOptions.Compress))
            {
                using var ms = new MemoryStream();
                using (var gzip = new GZipStream(ms, CompressionMode.Compress))
                {
                    gzip.Write(data, 0, data.Length);
                }
                data = ms.ToArray();
            }

            // Hash 
            byte[] hash = null;
            if (options.HasFlag(SaveOptions.Hash))
            {
                using var sha = SHA256.Create();
                hash = sha.ComputeHash(data);
            }
            
            byte[] version = BitConverter.GetBytes(VersionHandler.CurrentVersion);
            
            FileUtils.CreateDirectoryIfNotExists(path);

            // Writing to file
            using var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);

            fs.Write(MagicNumberBytes, 0, MagicNumberBytes.Length); // 4 byte
            
            fs.Write(version, 0, version.Length); // 4 byte
            fs.WriteByte((byte)saveType); // 1 byte
            fs.WriteByte((byte)options); // 1 byte

            if (hash != null)
                fs.Write(hash, 0, hash.Length); // 32 byte hash

            fs.Write(data, 0, data.Length);
        }

        public static T Load<T>(string path, SaveType saveType) where T : ISaveData
        {
            using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);

            // Header
            byte[] magic = new byte[MagicNumberBytes.Length];
            fs.Read(magic, 0, MagicNumberBytes.Length);
            if (!magic.SequenceEqual(MagicNumberBytes))
                throw new Exception("File format is invalid!");

            byte[] versionBytes = new byte[4];
            fs.Read(versionBytes, 0, 4);
            int version = BitConverter.ToInt32(versionBytes, 0);
            SaveType type = (SaveType)fs.ReadByte();
            
            SaveOptions options = (SaveOptions)fs.ReadByte();
            
            if (type != saveType)
                throw new Exception("Unmatching save type!");
            
            // Hash 
            byte[] hash = null;
            if (options.HasFlag(SaveOptions.Hash))
            {
                hash = new byte[32];
                fs.Read(hash, 0, 32);
            }

            // Data
            byte[] data = new byte[fs.Length - fs.Position];
            fs.Read(data, 0, data.Length);
            
            // Hash verification
            if (options.HasFlag(SaveOptions.Hash))
            {
                using var sha = SHA256.Create();
                byte[] checkHash = sha.ComputeHash(data);
                if (hash != null && !hash.SequenceEqual(checkHash))
                    throw new Exception("Save is corrupt!");
            }

            // Compression decoding
            if (options.HasFlag(SaveOptions.Compress))
            {
                using var ms = new MemoryStream(data);
                using var gzip = new GZipStream(ms, CompressionMode.Decompress);
                using var reader = new MemoryStream();
                gzip.CopyTo(reader);
                data = reader.ToArray();
            }

            // JSON deserialize
            string json = Encoding.UTF8.GetString(data);
            return JsonHelper.Deserialize<T>(json);
        }

    }

}