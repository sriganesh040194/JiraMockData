using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JiraMockData.Model;

namespace JiraMockData
{
    class Program
    {
        async static Task Main(string[] args)
        {


            Console.WriteLine("JIRA Mock Data");
            Console.WriteLine("Choose the operation");
            Console.WriteLine("1.Generate Data and Push to ELK \n2.Delete all Future date Indexes");

            var optioninput = Console.ReadLine();
            int option = string.IsNullOrEmpty(optioninput) ? 0 : Convert.ToInt32(optioninput);
            if (option == 1)
            {

                Console.WriteLine("Bulk Post Y/N? [N]: ");
                var input = Console.ReadLine();
                string answer = string.IsNullOrEmpty(input) ? "N" : input;


                var boardMetrics = new BoardMetrics((answer == "Y"|| answer ==  "y")? true: false );
                await boardMetrics.GenerateData();

                Console.WriteLine("############################# DONE ########################");
            }
            else if (option == 2)
            {
                Console.WriteLine("Enter number of days to delete:[60]");
                var input = Console.ReadLine();
                int days = string.IsNullOrEmpty(input) ? 60 : Convert.ToInt32(input);
                var elk = new ElasticSearch();
                await elk.DeleteAsync(days);

                Console.WriteLine("############################# DONE ########################");
            }
            else
                Console.WriteLine("Wrong option, Quitting the app");

        }
    }
}
