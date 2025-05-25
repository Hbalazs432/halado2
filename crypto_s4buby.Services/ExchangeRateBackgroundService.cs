using crypto_s4buby.Context.Context;
using crypto_s4buby.Context.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crypto_s4buby.Services
{
    public class ExchangeRateBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ExchangeRateBackgroundService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    Console.WriteLine($"Exchange Rate Background Service running..");

                    using(var scope = _serviceScopeFactory.CreateScope())
                    {
                        var context = scope.ServiceProvider.GetRequiredService<CryptoDbContext>();
                        var cryptoHistoryService = scope.ServiceProvider.GetRequiredService<ICryptoService>();

                        var cryptos = await context.Cryptos.ToListAsync();

                        foreach(var crypto in cryptos)
                        {
                            var num = Math.Round(new Random().NextDouble() * 0.3 - 0.15, 2);

                            var exchangeRate = crypto.ExchangeRate * (1-num);
                            
                            var temp = new CryptoUpdateDto { Id = crypto.Id, ExchangeRate = exchangeRate };

                            await cryptoHistoryService.UpdateExchangeRate(temp);
                        }

                        await context.SaveChangesAsync();
                    }

                    

                    await Task.Delay(TimeSpan.FromSeconds(60), stoppingToken);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                
            }
        }
    }
}
