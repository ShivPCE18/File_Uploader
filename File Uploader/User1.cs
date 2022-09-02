using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace File_Uploader
{
    public class User 
    {
        public string name { get; set; }
        public string password { get; set; }
        public string company { get; set; }
        public string branch { get; set; }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }

       
      
    }
}