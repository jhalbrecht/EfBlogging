using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using GalaSoft.MvvmLight;
using EfBlogging.Wpf.Model;
using GalaSoft.MvvmLight.Command;

namespace EfBlogging.Wpf.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private BloggingContext bloggingContext;
        private readonly IDataService _dataService;

        /// <summary>
        /// The <see cref="WelcomeTitle" /> property's name.
        /// </summary>
        public const string WelcomeTitlePropertyName = "WelcomeTitle";

        private string _welcomeTitle = string.Empty;

        /// <summary>
        /// Gets the WelcomeTitle property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
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

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IDataService dataService)
        {
            _dataService = dataService;
            _dataService.GetData(
                (item, error) =>
                {
                    if (error != null)
                    {
                        // Report error here
                        return;
                    }

                    WelcomeTitle = item.Title;
                });

            bloggingContext = new BloggingContext();
            PressMeCommand = new RelayCommand(() => DoPressMeCommand(), canExecute: () => Please());

            //InitialSetup(); // seed the db with fake data by bogus
            Setup();
        }

        private void DoPressMeCommand()
        {
            Debug.WriteLine("yup DoPressMeCommand");
            var blogs = bloggingContext.Blogs;
            var posts = bloggingContext.Posts;
        }
        private bool Please()
        {
            return true;
        }

        private void Setup()
        {
            Blogs = new ObservableCollection<Blog>(bloggingContext.Blogs);
            SelectedBlog = Blogs.FirstOrDefault();
            //using (var db = new BloggingContext())
            //{
            //    Blogs = new ObservableCollection<Blog>(db.Blogs);
            //}
        }

        private void InitialSetup()
        { // use this to seed the db
            var blogs = FakeBlog.Generator.Generate(5).ToList();
            using (var db = new BloggingContext())
            {
                foreach (var blog in blogs)
                {
                    db.Blogs.Add(blog);
                }
                var result = db.SaveChanges();
                Blogs = new ObservableCollection<Blog>(db.Blogs);
            }
        }

        // I'm using Fody which will expand these properties on build. 
        public ObservableCollection<Blog> Blogs { get; set; }
        public ObservableCollection<Post> Posts { get; set; }
        public Blog SelectedBlog { get; set; }
        public Post SelectedPost { get; set; }   

        public RelayCommand PressMeCommand { get; set; }


        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}