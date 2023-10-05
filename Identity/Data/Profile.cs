using AutoMapper;
using Identity.Data.DTOs;
using Identity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Data;

namespace Identity.Data
{
    internal class UsuarioProfile : Profile
    {
        public UsuarioProfile()
        {
            CreateMap<UsuarioCadastroRequest, Usuario>()
            .ForMember(usuario => usuario.UserName, opts => opts.MapFrom(dto => dto.Nome))
            .ForMember(usuario => usuario.PhoneNumber, opts => opts.MapFrom(dto => dto.Telefone));
        }
    }
}
