namespace major_wcf
{
    using NLog;
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Security.Cryptography.Pkcs;
    using System.Security.Cryptography.X509Certificates;
    using System.ServiceModel;
    using System.Text;
    using System.Xml;
    using major_xmldiff;
    using Microsoft.XmlDiffPatch;
    public class MajorService : IMajorService
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Открываем хранилище 'My' и ищем сертификат для подписи сообщения. Сертификат должен иметь поля Субъект (subject name) "TestGost".
        /// </summary>
        /// <param name="signerName"></param>
        /// <returns></returns>
        /// 
        static private X509Certificate2Collection GetSignerCert(string[] signerName)
        {

            X509Certificate2Collection collection = new X509Certificate2Collection();

            // Открываем хранилище My.
            X509Store storeMy = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            storeMy.Open(OpenFlags.ReadOnly);

            // Ищем сертификат для подписи.
            foreach (var item in signerName)
            {
                X509Certificate2Collection certColl = storeMy.Certificates.Find(X509FindType.FindBySerialNumber, item, false);
                collection.Add(certColl[0]);
            }

            //X509Certificate2Collection certColl = storeMy.Certificates.Find(X509FindType.FindBySerialNumber, signerName, false);
            // Проверяем, что нашли требуемый сертификат
            //if (certColl.Count == 0)
            //{
            //    Console.WriteLine("Сертификат для данного примера не найден " + "в хранилище. Выберите другой сертификат для подписи. ");
            //}

            storeMy.Close();

            // Если найдено более одного сертификата,
            // возвращаем первый попавщийся.
            //return certColl[0];
            return collection;
        }

        /// <summary>
        /// Подписываем сообщение секретным ключем.
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="signerCert"></param>
        /// <returns></returns> 
        static private byte[] SignMsg(string file_data, string[] signerName)
        {
            byte[] src = File.ReadAllBytes(file_data);
            // Создаем объект ContentInfo по сообщению.
            // Это необходимо для создания объекта SignedCms.
            ContentInfo contentInfo = new ContentInfo(src);

            // Создаем объект SignedCms по только что созданному
            // объекту ContentInfo.
            // SubjectIdentifierType установлен по умолчанию в
            // IssuerAndSerialNumber.
            // Свойство Detached устанавливаем явно в true, таким
            // образом сообщение будет отделено от подписи.
            SignedCms signedCms = new SignedCms(contentInfo, true);

            X509Certificate2Collection signerCert = GetSignerCert(signerName);
            foreach (X509Certificate2 x509 in signerCert)
            {
                // Определяем подписывающего, объектом CmsSigner.
                CmsSigner cmsSigner = new CmsSigner();
                cmsSigner.Certificate = x509;
                //cmsSigner.SignerIdentifierType = SubjectIdentifierType.IssuerAndSerialNumber;
                //cmsSigner.DigestAlgorithm = Oid.FromOidValue(signerCert.GetKeyAlgorithm(), OidGroup.PublicKeyAlgorithm);
                cmsSigner.IncludeOption = X509IncludeOption.EndCertOnly;

                //CryptographicAttributeObjectCollection CollAttr = new CryptographicAttributeObjectCollection();
                //CollAttr.Add(new Pkcs9SigningTime(DateTime.Now));
                cmsSigner.SignedAttributes.Add(new Pkcs9SigningTime(DateTime.Now));


                //CmsSigner cmsSigner = new CmsSigner(signerCert);

                // Подписываем CMS/PKCS #7 сообщение.
                //Console.Write("Вычисляем подпись сообщения для субъекта " + "{0} ... ", signerCert.SubjectName.Name);

                signedCms.ComputeSignature(cmsSigner, true);


                // Console.WriteLine("Успешно.");
            }

            // Кодируем CMS/PKCS #7 подпись сообщения.
            return signedCms.Encode();
        }

        /// <summary>
        /// Добавление подписи.
        /// </summary>
        /// <param name="file_data">Файл для подписания</param>
        /// <param name="file_sig">Файл подписи</param>
        /// <param name="signerName">SN (серийный номер сертификата) сертификата</param>
        /// <returns></returns>
        public byte[] AddSign(string file_data, string file_sig, string[] signerName)
        {
            bool detached = true;
            byte[] src = File.ReadAllBytes(file_data);
            byte[] sig = File.ReadAllBytes(file_sig);
            byte[] encodedSignature = { };
            try
            {
                SignedCms _signedCms = new SignedCms();
                _signedCms.Decode(sig);

                if (_signedCms.ContentInfo.Content.Length > 0)
                {
                    detached = false;
                }

                X509Certificate2Collection signerCert = GetSignerCert(signerName);
                ContentInfo contentInfo = new ContentInfo(src);
                SignedCms signedCms = new SignedCms(contentInfo, detached);
                signedCms.Decode(sig);
                foreach (X509Certificate2 x509 in signerCert)
                {
                    CmsSigner cmsSigner = new CmsSigner(x509);
                    signedCms.ComputeSignature(cmsSigner, detached);
                    encodedSignature = signedCms.Encode();
                }
                _logger.Info("Подпись успешно добавлена к файлу: {0}", file_data);
                return encodedSignature;
            }
            catch (Exception ex)
            {
                _logger.Error("Ошибка в добавлении подписи к файлу {0} : {1}", file_data, ex);
                return encodedSignature;
            }
        }

        /// <summary>
        /// Добавление подписи с указанием даты подписания
        /// </summary>
        /// <param name="file_data">Файл для подписания</param>
        /// <param name="file_sig">Файл подписи</param>
        /// <param name="date_sig">Дата подписи</param>
        /// <param name="signerName">SN (серийный номер сертификата) сертификата</param>
        /// <returns></returns>
        public byte[] AddSignDate(string file_data, string file_sig, DateTime date_sig, string[] signerName)
        {
            bool detached = true;
            byte[] src = File.ReadAllBytes(file_data);
            byte[] sig = File.ReadAllBytes(file_sig);
            byte[] encodedSignature = { };
            //date_sig = date_sig.HasValue ? date_sig : DateTime.Now;
            try
            {
                SignedCms _signedCms = new SignedCms();
                _signedCms.Decode(sig);

                if (_signedCms.ContentInfo.Content.Length > 0)
                {
                    detached = false;
                }

                X509Certificate2Collection signerCert = GetSignerCert(signerName);
                ContentInfo contentInfo = new ContentInfo(src);
                SignedCms signedCms = new SignedCms(contentInfo, detached);
                signedCms.Decode(sig);
                foreach (X509Certificate2 x509 in signerCert)
                {
                    CmsSigner cmsSigner = new CmsSigner(x509);
                    cmsSigner.SignedAttributes.Add(new Pkcs9SigningTime(date_sig));
                    signedCms.ComputeSignature(cmsSigner, detached);
                    encodedSignature = signedCms.Encode();
                }
                _logger.Info("Подпись успешно добавлена к файлу: {0}", file_data);
                return encodedSignature;
            }
            catch (Exception ex)
            {
                _logger.Error("Ошибка в добавлении подписи к файлу {0} : {1}", file_data, ex);
                return encodedSignature;
            }
        }

        /// <summary>
        /// Проверяем SignedCms сообщение и возвращаем Boolean значение определяющее результат проверки.
        /// </summary>
        /// <param name="file_data">Подписываемый файл</param>
        /// <param name="file_sig">Файл подписи</param>
        /// <returns></returns>
        public bool VerifyMsg(string file_data, string file_sig)
        {
            try
            {
                byte[] sig = File.ReadAllBytes(file_sig);
                byte[] src = File.ReadAllBytes(file_data);

                SignedCms _signedCms = new SignedCms();
                _signedCms.Decode(sig);
                bool detached = (_signedCms.ContentInfo.Content.Length > 0) ? false : true;

                // Создаем объект ContentInfo по сообщению.
                // Это необходимо для создания объекта SignedCms.
                ContentInfo contentInfo = new ContentInfo(src);

                // Создаем SignedCms для декодирования и проверки.
                SignedCms signedCms = new SignedCms(contentInfo, detached);

                // Декодируем подпись
                signedCms.Decode(sig);

                // Перехватываем криптографические исключения, для
                // возврата о false значения при некорректности подписи.
                // Проверяем подпись. В данном примере не
                // проверяется корректность сертификата подписавшего.
                // В рабочем коде, скорее всего потребуется построение
                // и проверка корректности цепочки сертификата.    
                signedCms.CheckSignature(true);
            }
            catch (CryptographicException ex)
            {
                _logger.Error("Ошибка проверки подписи у файла {0} : {1}", file_data, ex);
                return false;
                //throw;

                //Console.WriteLine("Функция VerifyMsg возбудила исключение: {0}", e.Message);
                //Console.WriteLine("Проверка PKCS #7 сообщения завершилась " +
                //"неудачно. Возможно сообщене, подпись, или " +
                //"соподписи модифицированы в процессе передачи или хранения. " +
                //"Подписавший или соподписавшие возможно не те " +
                //"за кого себя выдают. Достоверность и/или целостность " +
                //"сообщения не гарантируется. ");
                //return false;
            }
            _logger.Info("Успешная проверка подписи у файла: {0}", file_data);
            return true;
        }

        /// <summary>
        /// Сравнение двух XML
        /// </summary>
        /// <param name="sourceXmlFile">Исходный XML</param>
        /// <param name="changedXmlFile">Сравниваемый XML</param>
        /// <returns>Возвращает MemoryStream HTML таблицу сравнения двух XML</returns>
        public Stream XmlCompare(string sourceXmlFile, string changedXmlFile)
        {
            bool bFragment = false;

            // decode options
            XmlDiffOptions options = XmlDiffOptions.None;
            options |= XmlDiffOptions.IgnoreChildOrder;
            options |= XmlDiffOptions.IgnoreComments;
            options |= XmlDiffOptions.IgnorePI;
            options |= XmlDiffOptions.IgnoreWhitespace;
            options |= XmlDiffOptions.IgnoreNamespaces;
            options |= XmlDiffOptions.IgnorePrefixes;
            //options |= XmlDiffOptions.IgnoreXmlDecl;
            options |= XmlDiffOptions.IgnoreDtd;
                        
            MemoryStream diffgram = new MemoryStream();
            XmlTextWriter diffgramWriter = new XmlTextWriter(new StreamWriter(diffgram));

            MemoryStream memoryStream = new MemoryStream();
            TextWriter tw = new StreamWriter(memoryStream, Encoding.Unicode);
            
            XmlDiff xmlDiff = new XmlDiff(options);
            bool bIdentical = xmlDiff.Compare(sourceXmlFile, changedXmlFile, bFragment, diffgramWriter);
            

            
            tw.WriteLine("<h3>XmlDiff view</h3><table border='0'><tr><td><table border='0'>");
            tw.WriteLine("<tr><th>" + sourceXmlFile + "</th><th>" + changedXmlFile + "</th></tr>" + "<tr><td colspan=2><hr size=1></td></tr>");

            if (bIdentical)
            {
                tw.WriteLine("<tr><td colspan='2' align='middle'>Files are identical.</td></tr>");
            }
            else
            {
                tw.WriteLine("<tr><td colspan='2' align='middle'>Files are different.</td></tr>");
            }

            diffgram.Seek(0, SeekOrigin.Begin);
            XmlDiffView xmlDiffView = new XmlDiffView();
            XmlTextReader sourceReader;
            if (bFragment)
            {
                NameTable nt = new NameTable();
                sourceReader = new XmlTextReader(new FileStream(sourceXmlFile, FileMode.Open, FileAccess.Read),
                                                  XmlNodeType.Element,
                                                  new XmlParserContext(nt, new XmlNamespaceManager(nt),
                                                                        string.Empty, XmlSpace.Default));
            }
            else
            {
                sourceReader = new XmlTextReader(sourceXmlFile);
            }

            sourceReader.XmlResolver = null;
            xmlDiffView.Load(sourceReader, new XmlTextReader(diffgram));
            xmlDiffView.GetHtml(tw);
            
            tw.WriteLine("</table></table>");


            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
            
        }
    }
}
