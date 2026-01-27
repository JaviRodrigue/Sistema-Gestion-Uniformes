public interface INavigationService
{
    void Navigate<TViewModel>() where TViewModel : class;
}
