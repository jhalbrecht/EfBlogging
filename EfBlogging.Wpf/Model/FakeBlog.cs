using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;

namespace EfBlogging.Wpf.Model
{
    public class FakeBlog
    {
        static Random random = new Random();
        public static Faker<Blog> Generator { get; } = new Faker<Blog>()
        //.RuleFor(p => p.BlogId, p => IdCounter++)
        .RuleFor(p => p.Name, p => p.Name.FirstName())
        .RuleFor(p => p.Posts, p => FakePost.Generator.Generate(random.Next(2, 12)).ToList());
        protected static int IdCounter = 0;
        public static IList<Blog> Blogs { get; } = FakeBlog.Generator.Generate(5).ToList();
    }
}
