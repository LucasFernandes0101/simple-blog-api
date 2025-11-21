using Bogus;
using SimpleBlogApi.Domain.Entities;

namespace SimpleBlogApi.Tests.Mocks.Entities;

public class BlogPostFaker : Faker<BlogPost>
{
    public BlogPostFaker(int commentsCount = 3)
    {
        RuleFor(b => b.Id, f => f.Random.Int(1, 10000));
        RuleFor(b => b.Title, f => f.Lorem.Sentence(5, 5));
        RuleFor(b => b.Content, f => f.Lorem.Paragraphs(1, 3));
        RuleFor(b => b.CreatedAt, f => f.Date.RecentOffset());
        RuleFor(b => b.Comments, f =>
            new Faker<Comment>()
                .RuleFor(c => c.Id, _ => 0)
                .RuleFor(c => c.Content, cF => cF.Lorem.Sentence())
                .RuleFor(c => c.CreatedAt, cF => cF.Date.RecentOffset())
                .Generate(commentsCount)
        );
    }
}