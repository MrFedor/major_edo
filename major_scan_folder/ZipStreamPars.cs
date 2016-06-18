namespace major_scan_folder
{
    using major_data.Models;
    using NLog;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ZipStreamPars
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public static void RecursZip_stream(MemoryStream fullName, FileInSystem File_collection, List<TypeXML> typeXML)
        {
            try
            {
                using (ZipArchive archive = new ZipArchive(fullName, ZipArchiveMode.Read, false))
                {
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        if (entry.FullName.EndsWith(".xtdd", StringComparison.OrdinalIgnoreCase) && File_collection.FileType != FileType.FileCB)
                        {
                            using (MemoryStream reader = new MemoryStream())
                            {
                                entry.Open().CopyTo(reader);
                                reader.Seek(0, SeekOrigin.Begin);
                                ParsingXML.RazborXML_stream(reader, File_collection, typeXML);
                            }
                            break;
                        }
                        if (entry.FullName.EndsWith(".zip", StringComparison.OrdinalIgnoreCase) && File_collection.FileType != FileType.FileCB)
                        {
                            using (MemoryStream reader = new MemoryStream())
                            {
                                entry.Open().CopyTo(reader);
                                reader.Seek(0, SeekOrigin.Begin);
                                RecursZip_stream(reader, File_collection, typeXML);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "{0} : {1} : {2} : {3}", ex.Message, ex.TargetSite, ex.StackTrace, ex.InnerException);
            }
        }
    }
}
