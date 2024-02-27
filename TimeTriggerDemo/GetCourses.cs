using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace TimeTriggerDemo
{
    public class GetCourses
    {
        [FunctionName("GetCourses")]
        public async Task RunAsync([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer, ILogger log)
        {
            //List<Course> courses = new List<Course>();
            //using (var httpClient = new HttpClient())
            //{
            //    using (var response = await httpClient.GetAsync("http://localhost:5027/api/Course"))
            //    {
            //        string apiResponse = await response.Content.ReadAsStringAsync();
            //        courses = JsonConvert.DeserializeObject<List<Course>>(apiResponse);
            //    }
            //}

            var client = new GraphQLHttpClient("https://localhost:7287/graphql", new NewtonsoftJsonSerializer());
            var query = new GraphQLRequest
            {
                Query = @"
{
                    products {
                    id,
                    name,
quantity
}
                    }"
            };

            var response = client.SendQueryAsync<CoursesResponse>(query);

            foreach(var product in response.Result.Data.products)
            {
                Console.WriteLine($"{product.Name}, {product.Quantity}");
            }
        }
    }
}
