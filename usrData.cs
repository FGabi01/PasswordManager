using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JelszoKezelo
{
    public class usrData
    {
        private string fnev;
        public string FNev { get => fnev;  set => fnev = value; }
        private string jelszo;
        public string Jelszo { get => jelszo;  set => jelszo = value; }
        private string masterpw;
        public string MasterPw { get => masterpw;  set => masterpw = value; }
        private string sessiontoken;
        public string SessionToken { get => sessiontoken;  set => sessiontoken = value; }
    }
}
