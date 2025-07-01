using System.Diagnostics;
using System.Globalization;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using TaskFive.Application.Helpers;
using TaskFive.Application.Interface;
using TaskFive.Core.Entities;
using TaskFive.Core.Enums;
using TaskFive.Web.Models;

namespace TaskFive.Web.Controllers;

public class HomeController(IUserDataService service) : Controller
{
   const int PageSize = 20;

   public IActionResult Index()
   {
      return View();
   }

   [HttpGet]
   public IActionResult GetData(DataRequestModel requestModel)
   {
      var regionEnum = (Regions)requestModel.Region;

      var data = service.GetUserData(
         requestModel.Seed,
         requestModel.MistakesRate,
         regionEnum.ToString() // "Ru", "EnUs", "De", "Fr"
      );

      return Json(data.Skip((requestModel.PageNumber - 1) * PageSize).Take(PageSize));
   }

   [HttpPost]
   public async Task<IActionResult> CreateCsv([FromBody] IEnumerable<User> persons)
   {
      var path = $"{Directory.GetCurrentDirectory()}{DateTime.Now.Ticks}.csv";
      await using var writer = new StreamWriter(path);
      
      await using (var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture))
      {
         csvWriter.Context.RegisterClassMap<DataCsvMap>();
         await csvWriter.WriteRecordsAsync(persons);
      }

      return PhysicalFile(path, "text/csv");
   }
   
   public IActionResult Privacy()
   {
      return View();
   }
}
