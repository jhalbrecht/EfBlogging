# EfBlogging

A solution with two projects demonstrating several features and Adins that I frequently use in application development.

* EfBlogging.Wpf - ef and LocalDB
* EfBlogging.Uwp - ef and core.sqlite

##### End goal  for  projects
The goal is to bind Blogs to a ComboBox ItemsSopurce and bind it's SelectionChanged to a view model property `SelectedBlog` that is the ItemsSource for a ListView `SelectedBlog.Post`. This is working for the wpf version using LocalDB but not the uwp version using sqlite.

![Ef Blogging Cb Lv As It Should Be](GitHubStuff/EfBloggingCbLvAsItShouldBe.JPG)

## Impetus
Impetus for creating this project was attempting to implement sqlite in my updating of win8.0 TheLolFx in the store to uwp The implementation I created didn't act as I believed it should

I've used sql and entity framework (ef) a few times. It had been awhile. 
hence my implementing the Blog / Post classes of [this](https://docs.microsoft.com/en-us/ef/core/get-started/uwp/getting-started) article "UWP - New Database" for adding core.sqlite to uwp. 
So I would be working on a know good implementation. In fact the classes are in the wpf project and linked (not copied) to the uwp project. 
No chance of my fat fingers making a syntax error as I stare at the screen after hours of scratching my head at the seemingly odd behavior of ef SaveChanges()

## Tools used in projects
* [Bogus](https://github.com/bchavez/Bogus) - seeding fake data
* [PropertyChanged.Fody](https://github.com/Fody/PropertyChanged) - implements INotifyPropertyChanged at compile 
* [MvvmLight Toolkit](http://www.mvvmlight.net/)

## Reading resources
* [Entity Framework Add and Attach and Entity States](https://msdn.microsoft.com/en-us/library/jj592676%28v=vs.113%29.aspx?f=255&MSPPError=-2147217396)



## Other

This project also demonstrates an inconsistency ?Bug?? between ef sqlite and ef localDb


## The problem
I [created an issue](https://github.com/jhalbrecht/EfBlogging/issues/1) in this repository
It works great in the wpf LocalDB implementation, 
however in the uwp solution the **dbContext SaveChanges() doesn't appear to persist the List of posts in the Blogs after the dataContext is released.**
The Blogs and the Posts remain, but not the list of posts in a blog.

Here is an image of the uwp version working. The comboBox ItemsSource bound to bloggingContext.Blogs selects the blog, the ListView ItemsSource is bound to the Selected of the ComboBox displays the posts in that Blog. 

