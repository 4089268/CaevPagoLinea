using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CAEV.PagoLinea.Models;
using CAEV.PagoLinea.Data;
using CAEV.PagoLinea.Services;
using System.Reflection.Metadata.Ecma335;
using Microsoft.EntityFrameworkCore;

namespace CAEV.PagoLinea.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly PagoLineaContext context;
    private readonly ArquosService arquosService;

    public HomeController(ILogger<HomeController> logger, PagoLineaContext pagoLineaContext, ArquosService arquosService)
    {
        _logger = logger;
        this.context = pagoLineaContext;
        this.arquosService = arquosService;
    }

    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }


}
