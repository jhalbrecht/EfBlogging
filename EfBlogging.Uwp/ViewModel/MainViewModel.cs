﻿using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.PointOfService;
using AppDevPro.Utility;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using EfBlogging.Uwp.Model;
using EfBlogging.Wpf.Model;
using Microsoft.EntityFrameworkCore;
using Remotion.Linq.Clauses;

// some classes linked (not copied) here, hence this using statement

namespace EfBlogging.Uwp.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public const string ClockPropertyName = "Clock";
        public const string WelcomeTitlePropertyName = "WelcomeTitle";

        private readonly IDataService _dataService;
        private readonly INavigationService _navigationService;
        private string _clock = "Starting...";
        private int _counter;
        private RelayCommand _incrementCommand;
        private RelayCommand<string> _navigateCommand;
        private string _originalTitle;
        private bool _runClock;
        private RelayCommand _sendMessageCommand;
        private RelayCommand _showDialogCommand;
        private string _welcomeTitle = string.Empty;
        private BloggingContext bloggingContext;
        // public BloggingContext EfBloggingContext { get; set; } = new BloggingContext();

        public MainViewModel(IDataService dataService, INavigationService navigationService)
        {
            _dataService = dataService;
            _navigationService = navigationService;
            Initialize();
            bloggingContext = new BloggingContext();
            //FakeEmUp(); // seed the database with fake data from bogus. Comment out after first run.
            Init();
        }

        private void Init()
        {
            // bloggingContext = new BloggingContext();
            Blogs = new ObservableCollection<Blog>(bloggingContext.Blogs);
            SelectedBlog = Blogs.FirstOrDefault();
            var wellAreTheseTheListOfPosts = bloggingContext.Blogs.FirstOrDefault().Posts;
            Logger.Log($"Blogs: {bloggingContext.Blogs.Count()} Posts: {bloggingContext.Posts.Count()}");
            Logger.Log(this, $"In Init() is the list of posts still in the blog? ({wellAreTheseTheListOfPosts?.Count})");

        }

        public ObservableCollection<Blog> Blogs { get; set; }
        public ObservableCollection<Post> Posts { get; set; }
        public Blog SelectedBlog { get; set; }

        private void FakeEmUp()
        {
            var fakeBlogs = FakeBlog.Generator.Generate(4).ToList();
            var c = fakeBlogs.ToList();
            int saveChangesResult = 0;
            try
            {
                foreach (var fakeBlog in fakeBlogs)
                {
                    var foo = bloggingContext.Blogs.Add(fakeBlog);
                    Logger.Log(this, $"foo state added: {foo.State}");
                }
                var peas = bloggingContext.Posts;
                //bloggingContext.Entry(peas).State = EntityState.Modified;
                //bloggingContext.Entry(peas).State = EntityState.Added;
                //bloggingContext.Entry(peas.Entity).CurrentValues.SetValues(peas);

                saveChangesResult = bloggingContext.SaveChanges();
                Logger.Log("FakeEmUp()", $"saveChangesResult: {saveChangesResult}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            var b = bloggingContext.Blogs.Count();
            var p = bloggingContext.Posts.Count();
            var s = b + p;
            Logger.Log(this, $"Blogs {b} + Posts {p} = {s}");
            Logger.Log(this, $"They're equal right? changes in context from count of blogs and posts is {s} saveChangesResult is {saveChangesResult} ");
            var firstBlog = bloggingContext.Blogs.FirstOrDefault();
            var posts = firstBlog.Posts;
            var wellAreTheseTheListOfPosts = bloggingContext.Blogs.FirstOrDefault().Posts;
            var then = bloggingContext.Blogs.FirstOrDefault().Posts.FirstOrDefault().Title = "CHANGED";
            Logger.Log(this, $"is this ({then}) the title of the first post in the first blog?");
            var firstBlogPostsCount = posts.Count();
            Logger.Log(this, $"number of posts in the first blog {firstBlogPostsCount}");
            var onecount = bloggingContext.SaveChanges();
            Logger.Log(this, $"onecount {onecount}");
            var againwellAreTheseTheListOfPosts = bloggingContext.Blogs.FirstOrDefault().Posts;
            Logger.Log(this, $"In FakeEm() is the list of posts still in the blog? ({againwellAreTheseTheListOfPosts?.Count})");
            Logger.Log($"Blogs: {bloggingContext.Blogs.Count()} Posts: {bloggingContext.Posts.Count()}");
        }

        public string Clock
        {
            get
            {
                return _clock;
            }
            set
            {
                Set(ClockPropertyName, ref _clock, value);
            }
        }

        public RelayCommand IncrementCommand
        {
            get
            {
                return _incrementCommand
                    ?? (_incrementCommand = new RelayCommand(
                    () =>
                    {
                        WelcomeTitle = string.Format("Counter clicked {0} times", ++_counter);
                    }));
            }
        }

        public RelayCommand<string> NavigateCommand
        {
            get
            {
                return _navigateCommand
                       ?? (_navigateCommand = new RelayCommand<string>(
                           p => _navigationService.NavigateTo(ViewModelLocator.SecondPageKey, p),
                           p => !string.IsNullOrEmpty(p)));
            }
        }

        public RelayCommand SendMessageCommand
        {
            get
            {
                return _sendMessageCommand
                    ?? (_sendMessageCommand = new RelayCommand(
                    () =>
                    {
                        Messenger.Default.Send(
                            new NotificationMessageAction<string>(
                                "Testing",
                                reply =>
                                {
                                    WelcomeTitle = reply;
                                }));
                    }));
            }
        }

        public RelayCommand ShowDialogCommand
        {
            get
            {
                return _showDialogCommand
                       ?? (_showDialogCommand = new RelayCommand(
                           async () =>
                           {
                               var dialog = ServiceLocator.Current.GetInstance<IDialogService>();
                               await dialog.ShowMessage("Hello Universal Application", "it works...");
                           }));
            }
        }

        public string WelcomeTitle
        {
            get
            {
                return _welcomeTitle;
            }

            set
            {
                Set(ref _welcomeTitle, value);
            }
        }



        public void RunClock()
        {
            _runClock = true;

            Task.Run(async () =>
            {
                while (_runClock)
                {
                    try
                    {
                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            Clock = DateTime.Now.ToString("HH:mm:ss");
                        });

                        await Task.Delay(1000);
                    }
                    catch (Exception ex)
                    {
                    }
                }
            });
        }

        public void StopClock()
        {
            _runClock = false;
        }

        private async Task Initialize()
        {
            try
            {
                var item = await _dataService.GetData();
                _originalTitle = item.Title;
                WelcomeTitle = item.Title;
            }
            catch (Exception ex)
            {
                // Report error here
                WelcomeTitle = ex.Message;
            }
        }
    }
}