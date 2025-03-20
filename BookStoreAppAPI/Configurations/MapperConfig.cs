using AutoMapper;
using BookStoreAppAPI.Models.Author;
using BookStoreAppAPI.Data;
using BookStoreAppAPI.Models;
using BookStoreAppAPI.Models.Book;

namespace BookStoreAppAPI.Configurations
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            
            CreateMap<AuthorCreateDto, Author>().ReverseMap();
            CreateMap<AuthorUpdateDto, Author>().ReverseMap();
            CreateMap<AuthorReadOnlyDto, Author>().ReverseMap();

            CreateMap<BookCreateDto, Book>().ReverseMap();
            CreateMap<BookUpdateDto, Book>().ReverseMap();
      
            CreateMap<Book, BookReadOnlyDto>()
                .ForMember(q =>q.AuthorName, d => d.MapFrom(map =>$"{map.Author.FirstName} {map.Author.LastName}"))
                .ReverseMap();
            
           
            

            CreateMap<Book, BookDetailsDto>()
                .ForMember(q => q.AuthorName, d => d.MapFrom(map => $"{map.Author.FirstName} {map.Author.LastName}"))
                .ReverseMap();

        }
    }
}
