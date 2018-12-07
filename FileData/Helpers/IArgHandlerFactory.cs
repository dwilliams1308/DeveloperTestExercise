namespace FileData.Helpers
{
    public interface IArgHandlerFactory
    {
        IArgHandler Create(FileDataProcessor.FileActions actionType);
    }
}