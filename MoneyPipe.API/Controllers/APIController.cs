using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MoneyPipe.API.Common.Http;

namespace MoneyPipe.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class APIController : ControllerBase
    {
        protected IActionResult Problem(List<ErrorOr.Error> errors)
        {
            if (errors.Count == 0)
            {
                var defaultProblem = new ProblemDetails
                { 
                    Title = "An error occured!",
                    Status = StatusCodes.Status500InternalServerError
                };

                return WrapProblem(defaultProblem);
            }

            if (errors.All(error => error.Type is ErrorType.Validation))
            {
                return ValidationProblem(errors);
            }

            HttpContext.Items[HttpContextItemKeys.Errors] = errors;

            var firstError = errors[0];

            return Problem(firstError);
        }

        private IActionResult Problem(ErrorOr.Error firstError)
        {
            var statusCode = firstError.Type switch
            {
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError
            };

            var problemDetailsFactory = HttpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();
            var problemDetails = problemDetailsFactory.CreateProblemDetails(
                HttpContext,
                statusCode: statusCode,
                title: firstError.Description
            );

            return WrapProblem(problemDetails);
        }

        private IActionResult ValidationProblem(List<ErrorOr.Error> errors)
        {
            var modelStateDict = new ModelStateDictionary();

            foreach (var error in errors)
            {
                modelStateDict.AddModelError(
                        error.Code,
                        error.Description
                    );
            }

            var factory = HttpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();
            var validationProblem = factory.CreateValidationProblemDetails(HttpContext, modelStateDict);

            return WrapProblem(validationProblem);
        }

        private IActionResult WrapProblem(ProblemDetails problemDetails)
        {
            var response = ApiResponse<object>.Fail(problemDetails.Title ?? "An error occurred", problemDetails);
            return StatusCode(problemDetails.Status ?? 500, response);
        }

    }
}
