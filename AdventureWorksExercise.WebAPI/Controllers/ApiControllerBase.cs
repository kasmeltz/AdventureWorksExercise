using Microsoft.AspNetCore.Mvc;

namespace AdventureWorksExercise.WebAPI.Controllers.V1
{
    [ApiController]
    public class ApiControllerBase : ControllerBase
    {
        #region Constructors

        public ApiControllerBase(
            IConfiguration configuration,
            ILogger logger)
        {
            Configuration = configuration;
            Logger = logger;
        }

        #endregion

        #region Members

        protected IConfiguration Configuration { get; set; }

        protected ILogger Logger { get; set; }

        #endregion

        #region Helper Methods
       
        protected IActionResult HandleException(Exception ex)
        {
            Logger
                .LogError(ex, ex.Message);

            return Problem("An unexpected error occured", null, 500, "An unexpected error occured" );
        }

        #endregion
    }
}