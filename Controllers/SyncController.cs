using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NameBandit.Data;
using HtmlAgilityPack;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace NameBandit.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SyncController : ControllerBase
    {

        private readonly ILogger<SyncController> _logger;
        private readonly ApplicationDbContext _context;

        public SyncController(ILogger<SyncController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public ActionResult<bool> Sync()
        {          
            try {
                var names = from n in _context.Names select n;

                var theList = names.ToList();

                theList = ScapeNames(theList, "http://www.urd.dk/fornavne/drenge.htm");

                theList = ScapeNames(theList, "http://www.urd.dk/fornavne/piger.htm", true);

                foreach (var item in theList.OrderBy(x => x.Text)) {
                    item.Vibration = CalculateNameVibration(item.Text);

                    if (item.Id == 0) {
                        _context.Names.Add(item);
                    } else {
                        var lilleIb = _context.Names.Where(x => x.Id == item.Id).FirstOrDefault();
                        if (lilleIb != null) {
                            lilleIb.Male = item.Male;
                            lilleIb.Female = item.Female;
                            lilleIb.Active = item.Active;
                            lilleIb.Text = item.Text;
                        }
                    }
                }

            _context.SaveChanges();
            } catch (Exception exp) {
                return false;
            }

            return true;
        }
        
        private static List<Name> ScapeNames(List<Name> theList, string site, bool feminine = false) {
            HtmlWeb web = new HtmlWeb();
            web.AutoDetectEncoding = false;
            web.OverrideEncoding = Encoding.GetEncoding("iso-8859-1");

            var nameList = web.Load(site);
            var ib = nameList.DocumentNode.Descendants("p").ToList().FindAll(x => x.InnerHtml.Contains("</big>"));

            foreach (var bo in ib) {
                var per = bo.InnerHtml.Split("</big>")[1];

                if (per.Contains("</a>")) {
                    per = per.Split("</a>")[1];
                }

                if (per.Contains("<br>")) {
                    string[] persons = per.Split("<br>");

                    foreach (var mrMr in persons) {
                        string p = mrMr.Replace(@"\r\n","").Trim(); 

                        if (p.Length > 0 && !p.Contains("<a name=")) {
                            if (!theList.Select(x => x.Text).Contains(p)) {
                                theList.Add(new Name() {
                                    Id = 0,
                                    Text = p,
                                    Male = !feminine ? true : false, 
                                    Female = feminine ? true : false, 
                                    Active = true,
                                });
                            } else {
                                var uh = theList.Find(x => x.Text.Contains(p));
                                if (uh != null) {
                                    if (feminine) {
                                        uh.Female = true;
                                    } else {
                                        uh.Male = true;
                                    }
                                    uh.Active = true;
                                }
                            }
                        }
                    }
                }                
            }

            return theList;
        }

        private static int CalculateNameVibration (string name) {
            int sum = 0;
            char[] arr = name.ToUpper().ToCharArray();

            foreach (char letter in arr) {
                sum += GetVibrationByLetter(letter);
            }

            return sum;
        }

        private static int GetVibrationByLetter (char letter) {
            switch (letter) {
                case 'A': return 1;
                case 'B': return 2;
                case 'C': return 3;
                case 'D': return 4;
                case 'E': return 5;
                case 'F': return 8;  
                case 'G': return 3;
                case 'H': return 5;
                case 'I': return 1;
                case 'J': return 1;
                case 'K': return 2;
                case 'L': return 3;
                case 'M': return 4;
                case 'N': return 5;
                case 'O': return 7;
                case 'P': return 8;
                case 'Q': return 1;
                case 'R': return 2;  
                case 'S': return 3;
                case 'T': return 4;
                case 'U': return 6;
                case 'V': return 6;
                case 'X': return 5;
                case 'Y': return 1;     
                case 'Z': return 7;
                case 'Æ': return 6;
                case 'Ø': return 7;
                case 'Å': return 1;                                   
                default: return  0;
            }
        }
    }
}