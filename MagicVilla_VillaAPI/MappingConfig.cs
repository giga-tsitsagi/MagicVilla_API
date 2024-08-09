using AutoMapper;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPI
{
    public class MappingConfig : Profile
    {
        public MappingConfig() 
        {
            CreateMap< Villa, VillaDTO >().ReverseMap();

            CreateMap<Villa, VillaUpdateDTO >().ReverseMap();

            CreateMap<VillaUpdateDTO, VillaCreateDTO>().ReverseMap();
        }
    }
}
