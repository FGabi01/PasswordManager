using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JelszoKezelo
{
    public class usrData
    {
        private int id;
        private string fnev;
        private string jelszo;
        private string masterpw;
        private string sessiontoken;

        public int Id { get => id; set => id = value; }
        public string FNev { get => fnev;  set => fnev = value; }
        
        public string Jelszo { get => jelszo;  set => jelszo = value; }
        
        public string MasterPw { get => masterpw;  set => masterpw = value; }
        
        public string SessionToken { get => sessiontoken;  set => sessiontoken = value; }
    }
}
