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
                if (parts.Length != 7 || !DateTime.TryParse(parts[0], out _))
                {
                    continue;
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
            var maxKey = data.Keys.Count > 0 ? data.Keys.Max() : toDate;
            if (maxKey >= toDate)
            {
                data[maxKey] = data.TryGetValue(maxKey, out var value) ? value?.Update() : new FinancialData().Update();
                return data;
            }

            for (var i = maxKey; i < toDate.Date; i = i.AddDays(1))
            {
                data[i.AddDays(1)] = data[i].NextRand();
            }

            return data;
        }

        private static FinancialData Next(this FinancialData data)
        {
            return new FinancialData
            {
                Date = data.Time.AddDays(1).ToString("yyyy-MM-dd"),
                Time = data.Time.AddDays(1),
                Index = data.Index + 1,
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
            data.Close *= 1 + (rand.NextDouble() - 0.5) * 0.01;
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