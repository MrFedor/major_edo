namespace major_scan_folder
{
    using major_data.Models;
    using NLog;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    public class ParsingXML
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        //private static UserContext db = new UserContext();

        public static void RazborXML_stream(MemoryStream info_xml, FileInSystem File_collection, List<TypeXML> typeXML)
        {
            try
            {
                XDocument custOrdDoc = XDocument.Load(info_xml);
                string namesp = custOrdDoc.Root.Name.LocalName;
                TypeXML tag_search = typeXML.Where(m => m.Xml_type == namesp).FirstOrDefault();
                CBInfo _CBInfo = new CBInfo();
                File_collection.CBInfo = _CBInfo;
                File_collection.FileType = FileType.FileCB;
                _CBInfo.TypeXML = tag_search;
                if (!String.IsNullOrEmpty(tag_search.TagSearch))
                {
                    string period = "";
                    if (tag_search.TagSearch == "Год")
                    {
                        period = custOrdDoc.Root.Element(custOrdDoc.Root.Name.Namespace + tag_search.TagSearch).Value;
                    }
                    else if (tag_search.TagSearch == "ОтчетныйМесяц")
                    {
                        period = custOrdDoc.Root.Element(custOrdDoc.Root.Name.Namespace + tag_search.TagSearch).Element(custOrdDoc.Root.Name.Namespace + "Описание").Value;
                    }
                    _CBInfo.PeriodXML = period;
                }
                //Разбор Реестра
                if (namesp == "ОКУД0420504" && File_collection.FileIn == null)
                {
                    XElement del_element = custOrdDoc.Root.Element(custOrdDoc.Root.Name.Namespace + "СопроводительноеПисьмо");
                    List<XAttribute> del_att = custOrdDoc.Root.Attributes().ToList();
                    foreach (var item in del_att)
                    {
                        if (item.IsNamespaceDeclaration != true)
                        {
                            item.Remove();
                        }
                    }
                    del_element.Remove();

                    byte[] source_string = Encoding.Unicode.GetBytes(custOrdDoc.ToString());
                    string md5_hash = ScanFolder.MD5Hashe(source_string);

                    _CBInfo.HashTag = md5_hash;
                    
                }
                //using (UserContext db = new UserContext())
                //{
                //    db.CBInfo.Add(_CBInfo);
                //}
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "{0} : {1} : {2} : {3}", ex.Message, ex.TargetSite, ex.StackTrace, ex.InnerException);
            }
        }
    }
}
