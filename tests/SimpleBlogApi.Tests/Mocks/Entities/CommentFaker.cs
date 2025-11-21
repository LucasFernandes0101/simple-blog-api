using Bogus;
using SimpleBlogApi.Domain.Entities;

namespace SimpleBlogApi.Tests.Mocks.Entities;

public class CommentFaker : Faker<Comment>
{
    public CommentFaker()
    {
        RuleFor(c => c.Id, f => f.Random.Int(1, 100000));
        RuleFor(c => c.Content, f => f.Lorem.Sentence());
        RuleFor(c => c.CreatedAt, f => f.Date.RecentOffset());
    }
}
