using AutoMapper;
using BookStoreAppAPI.Models.Author;
using BookStoreAppAPI.Data;
using BookStoreAppAPI.Models;

namespace BookStoreAppAPI.Configurations
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<AuthorCreateDto, Author>().ReverseMap();
            CreateMap<AuthorUpdateDto, Author>().ReverseMap();
            CreateMap<AuthorReadOnlyDto, Author>().ReverseMap();
        }
    }
}
