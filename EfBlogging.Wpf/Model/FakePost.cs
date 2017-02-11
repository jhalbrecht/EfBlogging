using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;

namespace EfBlogging.Wpf.Model
{
    public class FakePost
    {
        static Random random = new Random();
        public static Faker<Post> Generator { get; } = new Faker<Post>()
        // .RuleFor(p => p.BlogId, p => IdCounter++)
        .RuleFor(p => p.Title, p => p.Name.FirstName())
        .RuleFor(p => p.Content, p => p.Lorem.Paragraph());
        protected static int IdCounter = 0;
        public static IList<Post> PlayLists { get; } = FakePost.Generator.Generate(5).ToList();
    }
}
