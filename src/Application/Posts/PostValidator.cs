using Domain;
using FluentValidation;

namespace Application.Posts;

public class PostValidator : AbstractValidator<Post>
{
	public PostValidator()
	{
		RuleFor(x => x.Title).NotEmpty();
		RuleFor(x => x.Content).NotEmpty();
		RuleFor(x => x.Date).NotEmpty();
	}
}
