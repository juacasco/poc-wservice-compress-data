using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace WService_ConceptTest_HostConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ServiceHost sh = new ServiceHost(typeof(WService_ConceptTest_HostConsole.ExecQuery));
                sh.Open();
                Console.WriteLine("Service started");
                Console.WriteLine("Presione enter para finalizar el servicio");
                Console.ReadLine();
                sh.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }
    }
}
