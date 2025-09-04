using Microsoft.ML;
using Microsoft.ML.Transforms.TimeSeries;
using SalesForcasting.Models;

namespace SalesForcasting.Services
{
    public class SalesForecastService
    {
        private readonly string _dataPath;
        private readonly MLContext _mlContext;

        public SalesForecastService(IWebHostEnvironment environment)
        {
            _mlContext = new MLContext();
            _dataPath = Path.Combine(environment.WebRootPath, "Data", "train.csv");
        }

        public async Task<List<SalesData>> ForecastSalesAsync()
        {
            var dataPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "train.csv");

            if (!File.Exists(dataPath))
                return new List<SalesData>();

            var mlContext = new MLContext();

            // Load data
            IDataView dataView = mlContext.Data.LoadFromTextFile<SalesData>(
                dataPath, hasHeader: false, separatorChar: ',');

            var forecastingPipeline = mlContext.Forecasting.ForecastBySsa(
                outputColumnName: "ForecastedSales",
                inputColumnName: "Sales",
                windowSize: 7,
                seriesLength: 30,
                trainSize: 30,
                horizon: 7);

            var model = forecastingPipeline.Fit(dataView);

            var forecastEngine = model.CreateTimeSeriesEngine<SalesData, SalesForcast>(mlContext);

            var forecast = forecastEngine.Predict();

            // 🟡 Use today's date instead of historical date from CSV
            var lastDate = DateTime.Today; 

            var results = new List<SalesData>();

            for (int i = 0; i < forecast.ForecastedSales.Length; i++)
            {
                results.Add(new SalesData
                {
                    Date = lastDate.AddDays(i + 1),
                    Sales = (float)Math.Round(forecast.ForecastedSales[i], 2) // Round for cleaner display
                });
            }

            return results;
        }

        public async Task<List<SalesData>> GetAllForecastsAsync()
        {
            return await ForecastSalesAsync();
        }

    }
}
