using Application.Posts;
using AutoMapper;
using Domain;
using Domain.Entities;

namespace Application.Core
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Post, Post>();
            CreateMap<PostDto, Post>();
            CreateMap<Post, PostDto>()
                .ForMember(d => d.Author, o => o.MapFrom(s => s.User));

            CreateMap<User, Profiles.Profile>()
                .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.Email))
                .ForMember(d => d.Username, o => o.MapFrom(s => s.Email));
        }
    }
}
