using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using major_data.Models;
using System.Data.Entity;
using System.Globalization;
using major_scan_folder;

namespace major_web.Hubs
{
    public class NewFile : Hub
    {
        private UserContext db = new UserContext();

        public void FileOnTake(int id)
        {
            FileInSystem _FileInfo = db.FileInSystem.Find(id);
            _FileInfo.FileStatus = m_FileStatus.NaPodpis;
            db.Entry(_FileInfo).State = EntityState.Modified;
            db.SaveChanges();
            Clients.All.takeOnFile(id);
        }

        public void FileClose(int id, string msg)
        {
            FileInSystem _FileInfo = db.FileInSystem.Include(p=>p.CBInfo).Where(p=>p.Id == id).FirstOrDefault();
            _FileInfo.CBInfo.Comment = msg;
            _FileInfo.FileStatus = m_FileStatus.Close;
            db.Entry(_FileInfo).State = EntityState.Modified;
            db.SaveChanges();
            int id_out = _FileInfo.Id;
            Clients.All.closeFile(id, msg);
        }

        public void FileToSign(string user, int id)
        {
            FileInSystem _FileInfo = db.FileInSystem.Find(id);
            _FileInfo.FileStatus = m_FileStatus.NaPodpis;
            db.Entry(_FileInfo).State = EntityState.Modified;
            db.SaveChanges();
            Clients.All.toSignFile(id);
        }


        public void FileSign(int id)
        {
            FileInSystem file = db.FileInSystem.Include(n=>n.RuleSystem.Department).Include(m=>m.RuleSystem.Secshondeportament).Where(v=>v.Id == id).FirstOrDefault();
            List<TypeXML> typeXML = db.TypeXML.ToList();
            FileInSystem _out_file = SignOut.AddSignOutFile(file, typeXML);
            if (_out_file != null)
            {
                file.FileStatus = m_FileStatus.Podpisan;
                db.FileInSystem.Add(_out_file);
                db.SaveChanges();

                Clients.All.signFile(id, _out_file.Id, _out_file.DataCreate.ToString());
            }
            else
            {
                string msg = "Не удалось подписать файл";
                Clients.All.closeFile(id, msg);
            }
        }

        public void FileReOpen(int id)
        {
            FileInSystem _FileInfo = db.FileInSystem.Find(id);
            _FileInfo.FileStatus = m_FileStatus.ReOpen;
            db.Entry(_FileInfo).State = EntityState.Modified;
            db.SaveChanges();
            Clients.All.reOpenFile(id);
        }
    }
}