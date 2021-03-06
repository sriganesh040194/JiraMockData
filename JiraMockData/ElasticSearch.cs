using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace JiraMockData
{
    public class ElasticSearch
    {
        //readonly Uri host = new Uri("http://localhost:9200");
        HttpClient httpClient;
        public ElasticSearch(string hostname)
        {
            var host = new Uri(hostname);
            httpClient = new HttpClient();
            httpClient.Timeout = new TimeSpan(0, 2, 0);
            httpClient.BaseAddress = host;
        }

        public async Task PostAsync(string indexName, string data)
        {

            var httpContent = new StringContent(TransformToELKFormat(data), Encoding.UTF8, "application/x-ndjson");


            //Console.WriteLine(await httpContent.ReadAsStringAsync());
            try
            {
                var response = await httpClient.PostAsync(indexName + "/_bulk", httpContent);

                if (response.IsSuccessStatusCode)
                    Console.WriteLine(indexName + " POSTED successfully to ELK");
                else
                    Console.WriteLine(indexName + " FAILED POST to ELK");
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public async Task BulkPostAsync(string data)
        {
            var httpContent = new StringContent(data, Encoding.UTF8, "application/x-ndjson");


            Console.WriteLine(await httpContent.ReadAsStringAsync());
            try
            {
                var response = await httpClient.PostAsync("/_bulk", httpContent);

                if (response.IsSuccessStatusCode)
                    Console.WriteLine("Bulk POSTED successfully to ELK");
                else
                {
                    Console.WriteLine("BULK POST FAILED POST to ELK");
                    Console.WriteLine(await response.Content.ReadAsStringAsync());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }


        public async Task GetAsync()
        {
            var response = await httpClient.GetAsync("dev.logevent.anypoint-jira-board-user-issue-metrics.2021.05.05/_search");

            Console.WriteLine(await response.Content.ReadAsStringAsync());

        }

        public async Task DeleteAsync(int days)
        {
            var indexes = new string[] { BoardMetrics.boardIndexname, BoardMetrics.boardUserIssueIndexName, BoardMetrics.userIndexName };

            foreach (var index in indexes)
            {
                for (int day = 0; day <= days; day++)
                {
                    var indexName = index + BoardMetrics.GetYMDPatternDate(day);
                    var response = await httpClient.DeleteAsync(indexName);


                    Console.WriteLine("Deleting Index: {0}", indexName);

                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Index not found, Escaping further deleting of {0}", index);
                        break;
                    }
                }
            }

        }

        private string TransformToELKFormat(string data)
        {
            var index = "{\"index\": {}}";
            var tranformedData = index + "\n" + data + "\n";
            return tranformedData;
        }


        public string BulkPostDataFormatter(string indexName, string data)
        {
            var index = "{\"index\": {\"_index\": \"" + indexName + "\"}}";
            var tranformedData = index + "\n" + data + "\n";
            return tranformedData;

        }
    }
}
