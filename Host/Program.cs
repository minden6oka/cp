using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Host
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("ENTER TO HOST");
            Console.ReadKey();
            System.ServiceModel.ServiceHost host = new System.ServiceModel.ServiceHost(typeof(CinemaServiceDLL.CService));
            
                host.Open();
                Console.WriteLine("Press enter to exit...");
                Console.ReadLine();
            }   
            
            
        }
    }

