﻿using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;

namespace BehaviorsSampleApp.ViewModels
{
  public class ViewModelBase : BindableBase, INavigationAware, IDestructible
  {
    protected INavigationService NavigationService { get; private set; }

    protected IPageDialogService PageDialogService { get; private set; }


    private string _title;
    public string Title
    {
      get => _title;
      set => SetProperty(ref _title, value);
    }

    public ViewModelBase(INavigationService navigationService, IPageDialogService pageDialogService)
    {
      NavigationService = navigationService;
      PageDialogService = pageDialogService;
    }

    public virtual void OnNavigatedFrom(INavigationParameters parameters)
    {

    }

    public virtual void OnNavigatedTo(INavigationParameters parameters)
    {

    }

    public virtual void OnNavigatingTo(INavigationParameters parameters)
    {

    }

    public virtual void Destroy()
    {

    }
  }
}
