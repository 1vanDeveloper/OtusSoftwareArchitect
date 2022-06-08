using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using StockMarket.Shared.Models;

namespace Notification.Host.Helpers
{
    public static class StockHelper
    {
        private const string DirectoryPath = "Notification.Host.Helpers";
        
        public static async Task<Dictionary<DateTime, FinancialData>> GetHistoricalDataAsync()
        {
            var result = new Dictionary<DateTime, FinancialData>();
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"{DirectoryPath}.MSFT.csv";

            await using var stream = assembly.GetManifestResourceStream(resourceName);
            using var reader = new StreamReader(stream ?? throw new Exception("MSFT.csv"));
            string line;
            do
            {
                line = await reader.ReadLineAsync();
                if (string.IsNullOrEmpty(line))
                {
                    break;
                }

                var parts = line.Split(",");
                if (parts.Length != 8 || !DateTime.TryParse(parts[0], out _))
                {
                    break;
                }
                
                var newValue = new FinancialData
                {
                    Date = parts[0],
                    Time = DateTime.Parse(parts[0]),
                    Open = double.Parse(parts[1]),
                    High = double.Parse(parts[2]),
                    Low = double.Parse(parts[3]),
                    Close = double.Parse(parts[4]),
                    Volume = double.Parse(parts[6])
                };

                result[newValue.Time.Date] = newValue;

            } while (!string.IsNullOrEmpty(line));

            return result;
        }

        /// <summary>
        /// Generates new data based on passed length
        /// </summary>
        public static IDictionary<DateTime, FinancialData> GenerateData(this IDictionary<DateTime, FinancialData> data, DateTime toDate)
        { 
            var maxKey = data.Keys.Max();
            if (maxKey >= toDate)
            {
                data[maxKey].Update();
                return data;
            }

            for (var i = maxKey; i <= toDate.Date; i = i.AddDays(1))
            {
                if (i < toDate.Date)
                {
                    data[i.AddDays(1)] = data[i].NextRand();
                    continue;
                }
                
                data[i.AddDays(1)] = data[i].Next();
            }

            return data;
        }

        private static FinancialData Next(this FinancialData data)
        {
            return new FinancialData
            {
                Open = data.Close,
                High = data.Close,
                Low = data.Close,
                Close = data.Close
            };
        }
        
        private static FinancialData NextRand(this FinancialData data)
        {
            var rand = new Random();
            var max = rand.Next(50, 300);

            var result = data.Next();
            for (var _ = 0; _ < max; _++)
            {
                result.Update();
            }

            return result;
        }


        private static FinancialData Update(this FinancialData data)
        {
            var rand = new Random();
            data.Close = (rand.NextDouble() - 0.5) * (Math.Abs(data.Open - data.Close) * 0.1);
            if (data.Close > data.High)
            {
                data.High = data.Close;
            }

            if (data.Close < data.Low)
            {
                data.Low = data.Close;
            }

            return data;
        }
    }
}