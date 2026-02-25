public static class ServiceCollector
{
    // TODO: Collect all services here
    public static IGridService Grid
        => ServiceLocator.GetRequired<IGridService>();

    public static IBuildService Build
        => ServiceLocator.GetRequired<IBuildService>();
}
