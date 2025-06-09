using AiclaRM.Server.Services.Tourney.Employee;
using AiclaRM.Server.Services.Tourney.Person;
using AiclaRM.Server.Services.Tourney.Prize;
using AIRMDataManager.Library.Modules.Tourney.Employee.Models;
using AIRMDataManager.Library.Modules.Tourney.Person.Models;
using AIRMDataManager.Library.Modules.Tourney.Prize.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AiclaRM.Server.Extensions
{
    public static class TourneyApiExtensions
    {
        public static void MapTourneyAPI(this WebApplication app)
        {
            var tourneyApi = app.MapGroup("/api/v1/tourney");

            //
            // PRIZE ENDPOINTS
            //
            tourneyApi.MapGet("/prizes", async ([FromServices] IPrizeService prizeService) =>
                Results.Ok(await prizeService.GetAllPrizesAsync()))
            .WithName("GetAllPrizes")
            .WithTags("TNM :  Prize");

            tourneyApi.MapGet("/prizes/{id:int}", async (
                    [FromServices] IPrizeService prizeService,
                    [FromRoute] int id) =>
            {
                var prize = await prizeService.GetPrizeByIdAsync(id);
                return prize is not null
                    ? Results.Ok(prize)
                    : Results.NotFound();
            })
            .WithName("GetPrizeById")
            .WithTags("TNM :  Prize");

            tourneyApi.MapPost("/prizes", async (
                    [FromServices] IPrizeService prizeService,
                    [FromBody] mst_prize prize) =>
                Results.Ok(await prizeService.InsertPrizeAsync(prize)))
            .WithName("CreatePrize")
            .WithTags("TNM :  Prize");

            tourneyApi.MapPut("/prizes/{id:int}", async (
                    [FromServices] IPrizeService prizeService,
                    [FromBody] mst_prize prize,
                    [FromRoute] int id) =>
            {
                if (prize.id != id)
                    return Results.BadRequest("Prize ID mismatch.");
                return Results.Ok(await prizeService.UpdatePrizeAsync(prize));
            })
            .WithName("UpdatePrize")
            .WithTags("TNM :  Prize");

            tourneyApi.MapDelete("/prizes/{id:int}", async (
                    [FromServices] IPrizeService prizeService,
                    [FromRoute] int id) =>
            {
                var deleted = await prizeService.DeletePrizeAsync(id);
                return deleted
                    ? Results.NoContent()
                    : Results.NotFound();
            })
            .WithName("DeletePrize")
            .WithTags("TNM :  Prize");


            //
            // PERSON ENDPOINTS
            //
            tourneyApi.MapGet("/people", async ([FromServices] IPersonService personService) =>
                Results.Ok(await personService.GetAllPersonsAsync()))
            .WithName("GetAllPeople")
            .WithTags("TNM : Person");

            tourneyApi.MapGet("/people/{id:int}", async (
                    [FromServices] IPersonService personService,
                    [FromRoute] int id) =>
            {
                var person = await personService.GetPersonByIdAsync(id);
                return person is not null
                    ? Results.Ok(person)
                    : Results.NotFound();
            })
            .WithName("GetPersonById")
            .WithTags("TNM : Person");

            tourneyApi.MapPost("/people", async (
                    [FromServices] IPersonService personService,
                    [FromBody] tnm_person person) =>
                Results.Ok(await personService.InsertPersonAsync(person)))
            .WithName("CreatePerson")
            .WithTags("TNM : Person");

            tourneyApi.MapPut("/people/{id:int}", async (
                    [FromServices] IPersonService personService,
                    [FromBody] tnm_person person,
                    [FromRoute] int id) =>
            {
                if (person.id != id)
                    return Results.BadRequest("Person ID mismatch.");
                return Results.Ok(await personService.UpdatePersonAsync(person));
            })
            .WithName("UpdatePerson")
            .WithTags("TNM : Person");

            tourneyApi.MapDelete("/people/{id:int}", async (
                    [FromServices] IPersonService personService,
                    [FromRoute] int id) =>
            {
                var deleted = await personService.DeletePersonAsync(id);
                return deleted
                    ? Results.NoContent()
                    : Results.NotFound();
            })
            .WithName("DeletePerson")
            .WithTags("TNM : Person");








            //
            // EMPLOYEE ENDPOINTS
            //
            tourneyApi.MapGet("/employees", async ([FromServices] IEmployeeService employeeService) =>
                Results.Ok(await employeeService.GetAllEmployeesAsync()))
            .WithName("GetAllEmployees")
            .WithTags("TNM :  Employee");

            tourneyApi.MapGet("/employees/{id:int}", async (
                    [FromServices] IEmployeeService employeeService,
                    [FromRoute] int id) =>
            {
                var emp = await employeeService.GetEmployeeByIdAsync(id);
                return emp is not null
                    ? Results.Ok(emp)
                    : Results.NotFound();
            })
            .WithName("GetEmployeeById")
            .WithTags("TNM :  Employee");

            tourneyApi.MapPost("/employees", async (
                    [FromServices] IEmployeeService employeeService,
                    [FromBody] mst_employee employee) =>
                Results.Ok(await employeeService.InsertEmployeeAsync(employee)))
            .WithName("CreateEmployee")
            .WithTags("TNM :  Employee");

            tourneyApi.MapPut("/employees/{id:int}", async (
                    [FromServices] IEmployeeService employeeService,
                    [FromBody] mst_employee employee,
                    [FromRoute] int id) =>
            {
                if (employee.id != id)
                    return Results.BadRequest("Employee ID mismatch.");
                return Results.Ok(await employeeService.UpdateEmployeeAsync(employee));
            })
            .WithName("UpdateEmployee")
            .WithTags("TNM :  Employee");

            tourneyApi.MapDelete("/employees/{id:int}", async (
                    [FromServices] IEmployeeService employeeService,
                    [FromRoute] int id) =>
            {
                var deleted = await employeeService.DeleteEmployeeAsync(id);
                return deleted
                    ? Results.NoContent()
                    : Results.NotFound();
            })
            .WithName("DeleteEmployee")
            .WithTags("TNM :  Employee");
        }
    }
}
