namespace FileCabinetApp.CommandHandlers
{
    /// <inheritdoc/>
    public abstract class ServiceCommandHandlerBase : CommandHandlerBase
    {
        private readonly IFileCabinetService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceCommandHandlerBase"/> class.
        /// </summary>
        /// <param name="service">Service.</param>
        protected ServiceCommandHandlerBase(IFileCabinetService service)
        {
            if (service is null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            this.service = service;
        }

        /// <summary>
        /// Gets service.
        /// </summary>
        /// <value>Service.</value>
        protected IFileCabinetService Service => this.service;

        /// <inheritdoc/>
        protected abstract override void Action(AppCommandRequest commandRequest);
    }
}