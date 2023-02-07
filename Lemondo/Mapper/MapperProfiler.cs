using AutoMapper;
using Lemondo.Requestes;
using Lemondo.DbClasses;
using Lemondo.ClientClass;

namespace Lemondo.Mapper
{
    public class MapperProfiler : Profile
    {
        public MapperProfiler()
        {
            CreateMap<Book, BookResponse>().ReverseMap();
            CreateMap<Book, BookRequest>().ReverseMap()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title.ToUpper()))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description.ToUpper()));
            
            CreateMap<Author, AuthorResponse>().ReverseMap();
            CreateMap<Author, AuthorRequest>().ReverseMap()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName.ToUpper()))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName.ToUpper()));

            CreateMap<User, UserResponse>().ReverseMap();
            CreateMap<User, UserRequest>().ReverseMap()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName.ToUpper()))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName.ToUpper()))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.ToUpper()));

            CreateMap<BookRequest, BookResponse>().ReverseMap()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title.ToUpper()))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description.ToUpper()));


        }
    }
}
