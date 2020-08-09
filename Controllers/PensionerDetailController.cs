using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MFRP_Pension_Detail.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PensionerDetailController : ControllerBase
    {
        /// <summary>
        /// Defining log Object
        /// </summary>
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(PensionerDetailController));
        private IConfiguration configuration;
        // string dbconn = "demo.csv";

        /// <summary>
        ///  Dependency Injection
        /// </summary>
        /// <param name="iConfig"></param>
        public PensionerDetailController(IConfiguration iConfig)
        {
            configuration = iConfig;
        }
        ///Summary
        ///Getting the details of the pensioner details from csv file by giving Aadhar Number
        ///Summary
        /// <returns> pensioner Values</returns>
        /// 
        // GET: api/PensionerDetail/5
        [HttpGet("{aadhar}")]
        public PensionerDetail PensionerDetailByAadhar(string aadhar)
        {
            List<PensionerDetail> pensionDetails = GetDetailsCsv();
            _log4net.Info("Pensioner details invoked by Aadhar Number!");
            return pensionDetails.FirstOrDefault(s => s.AadharNumber == aadhar);
        }
        ///Summary
        /// Getting the Values from Csv File 
        ///Summary
        /// <returns> Returning the list of values</returns>
        private List<PensionerDetail> GetDetailsCsv()
        {
            _log4net.Info("Data is read from CSV file");  // Logging Implemented
            List<PensionerDetail> pensionerdetail = new List<PensionerDetail>();
            try
            {
                string csvConn = configuration.GetValue<string>("MySettings:CsvConnection");  // Initializing the csvConn  for the File path
                using (StreamReader sr = new StreamReader(csvConn))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] values = line.Split(',');
                        pensionerdetail.Add(new PensionerDetail() {
                            Name = values[0],
                            Dateofbirth = Convert.ToDateTime(values[1]), 
                            Pan = values[2],
                            AadharNumber = values[3],
                            SalaryEarned = Convert.ToInt32(values[4]),
                            Allowances = Convert.ToInt32(values[5]),
                            PensionType = (PensionTypeValue)Enum.Parse(typeof(PensionTypeValue), values[6]), 
                            BankName = values[7],
                            AccountNumber = values[8], 
                            BankType = (BankType)Enum.Parse(typeof(BankType), values[9]) });
                    }

                }
            }
            catch (NullReferenceException e)
            {
                _log4net.Error("Values cannot be fetched from the Csv file"+e);
                return null;
            }
            catch(Exception e)
            {
                _log4net.Error("Values cannot be fetched from the Csv file" + e);
                return null;
            }
            return pensionerdetail.ToList();
        }
    }
}