namespace WorkoutGlobal.LoggingService.Api.Contracts
{
    /// <summary>
    /// Base interface for all models.
    /// </summary>
    public interface IModel<TId>
    {
        /// <summary>
        /// Unique identifier of model.
        /// </summary>
        public TId Id { get; set; }
    }
}
