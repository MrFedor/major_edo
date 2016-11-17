using client_data.Models;
using major_data;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace major_scan_folder
{
    static class RegRequest
    {
        public static RequestDeposits ProcRegRequest(RequestDeposits _RequestDeposit)
        {
            RegNumDateRequest _reg = new RegNumDateRequest();
            DateTime _date = DateTime.Now;

            using (ProcedureContext context = new ProcedureContext())
            {
                SqlParameter CHILD_CAT = new SqlParameter("@CHILD_CAT", 667);
                SqlParameter STEP_ID = new SqlParameter("@STEP_ID", 1);                
                SqlParameter PORTFOLIO = new SqlParameter("@PORTFOLIO", _RequestDeposit.PortfolioId);
                SqlParameter D_DATE = new SqlParameter("@D_DATE", _date);                
                SqlParameter DOC_NUM = new SqlParameter("@DOC_NUM", _RequestDeposit.OutNumber);
                SqlParameter DOC_DATE = new SqlParameter("@DOC_DATE", _RequestDeposit.OutDate);
                try
                {
                    _reg = context
                    .Database
                    .SqlQuery<RegNumDateRequest>("USD_PR_INSERT_FILER @CHILD_CAT, @STEP_ID, @PORTFOLIO, @D_DATE, @DOC_NUM, @DOC_DATE", CHILD_CAT, STEP_ID, PORTFOLIO, D_DATE, DOC_NUM, DOC_DATE)
                    .FirstOrDefault();
                }
                catch (Exception)
                {
                    throw;
                }                
            }
            if (_reg != null)
            {
                _RequestDeposit.RequestDate = _date;
                _RequestDeposit.RequestNum = _reg.NUM;
            }

            return _RequestDeposit;
        }

        public static DogovorDeposits ProcRegDogovor(DogovorDeposits _DogovorDeposit)
        {
            RegNumDateRequest _reg = new RegNumDateRequest();
            DateTime _date = DateTime.Now;

            using (ProcedureContext context = new ProcedureContext())
            {
                SqlParameter CHILD_CAT = new SqlParameter("@CHILD_CAT", 667);
                SqlParameter STEP_ID = new SqlParameter("@STEP_ID", 2);
                SqlParameter PORTFOLIO = new SqlParameter("@PORTFOLIO", _DogovorDeposit.PortfolioId);
                SqlParameter D_DATE = new SqlParameter("@D_DATE", _date);
                SqlParameter DOC_NUM = new SqlParameter("@DOC_NUM", _DogovorDeposit.OutNumber);
                SqlParameter DOC_DATE = new SqlParameter("@DOC_DATE", _DogovorDeposit.OutDate);
                try
                {
                    _reg = context
                    .Database
                    .SqlQuery<RegNumDateRequest>("USD_PR_INSERT_FILER @CHILD_CAT, @STEP_ID, @PORTFOLIO, @D_DATE, @DOC_NUM, @DOC_DATE", CHILD_CAT, STEP_ID, PORTFOLIO, D_DATE, DOC_NUM, DOC_DATE)
                    .FirstOrDefault();
                }
                catch (Exception)
                {
                    throw;
                }
            }
            if (_reg != null)
            {
                _DogovorDeposit.RequestDate = _date;
                _DogovorDeposit.RequestNum = _reg.NUM;
            }

            return _DogovorDeposit;
        }

        public static RequestDeposits ProcCartRequest(RequestDeposits _RequestDeposit, StringWriter custOrdDoc)
        {
            RegNumDateRequest _reg = new RegNumDateRequest();
            DateTime _date = DateTime.Now;

            using (ProcedureContext context = new ProcedureContext())
            {
                SqlParameter XML = new SqlParameter("@XML", custOrdDoc.ToString());
                _reg = context
                    .Database
                    .SqlQuery<RegNumDateRequest>("USD_PR_INSERT_CONSERT_XML @XML", XML)
                    .FirstOrDefault();
            }

            if (_reg != null)
            {
                _RequestDeposit.RequestDate = _date;
                _RequestDeposit.RequestNum = _reg.NUM;
            }

            return _RequestDeposit;
        }

        public static DogovorDeposits ProcCartDogovor(DogovorDeposits _DogovorDeposit, StringWriter custOrdDoc)
        {
            RegNumDateRequest _reg = new RegNumDateRequest();
            DateTime _date = DateTime.Now;

            using (ProcedureContext context = new ProcedureContext())
            {
                SqlParameter XML = new SqlParameter("@XML", custOrdDoc.ToString());
                _reg = context
                    .Database
                    .SqlQuery<RegNumDateRequest>("USD_PR_INSERT_DEPOSIT_XML @XML", XML)
                    .FirstOrDefault();
            }

            if (_reg != null)
            {
                _DogovorDeposit.RequestDate = _date;
                _DogovorDeposit.RequestNum = _reg.NUM;
            }

            return _DogovorDeposit;
        }

    }

    public class RegNumDateRequest
    {
        public int ID { get; set; }
        public string NUM { get; set; }
    }
}
