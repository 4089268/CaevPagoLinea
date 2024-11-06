using System;
using Quartz;

namespace CAEV.PagoLinea.Services;

public class HelloJob : IJob {
    public async Task Execute(IJobExecutionContext context) {
        Console.WriteLine("Hola mundo, soy un job");
        await Task.Delay(100);
    }

}
