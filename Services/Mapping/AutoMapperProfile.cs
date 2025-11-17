using AutoMapper;
using challenge_3_net.Models;
using challenge_3_net.Models.DTOs;

namespace challenge_3_net.Services.Mapping
{
    /// <summary>
    /// Perfil de mapeamento do AutoMapper para MindTrack
    /// </summary>
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Mapeamentos para Usuario
            CreateMap<Usuario, UsuarioResponseDto>()
                .ForMember(dest => dest.Links, opt => opt.Ignore());

            CreateMap<CriarUsuarioDto, Usuario>()
                .ForMember(dest => dest.IdUsuario, opt => opt.Ignore())
                .ForMember(dest => dest.SenhaHash, opt => opt.Ignore())
                .ForMember(dest => dest.DataCadastro, opt => opt.Ignore())
                .ForMember(dest => dest.Humores, opt => opt.Ignore())
                .ForMember(dest => dest.Sprints, opt => opt.Ignore())
                .ForMember(dest => dest.AlertasIA, opt => opt.Ignore())
                .ForMember(dest => dest.Habitos, opt => opt.Ignore())
                .ForMember(dest => dest.UsuarioBadges, opt => opt.Ignore());

            CreateMap<AtualizarUsuarioDto, Usuario>()
                .ForMember(dest => dest.IdUsuario, opt => opt.Ignore())
                .ForMember(dest => dest.SenhaHash, opt => opt.Ignore())
                .ForMember(dest => dest.DataCadastro, opt => opt.Ignore())
                .ForMember(dest => dest.Humores, opt => opt.Ignore())
                .ForMember(dest => dest.Sprints, opt => opt.Ignore())
                .ForMember(dest => dest.AlertasIA, opt => opt.Ignore())
                .ForMember(dest => dest.Habitos, opt => opt.Ignore())
                .ForMember(dest => dest.UsuarioBadges, opt => opt.Ignore());

            // Mapeamentos para Humor
            CreateMap<Humor, HumorResponseDto>()
                .ForMember(dest => dest.Links, opt => opt.Ignore());

            CreateMap<CriarHumorDto, Humor>()
                .ForMember(dest => dest.IdHumor, opt => opt.Ignore())
                .ForMember(dest => dest.IdUsuario, opt => opt.Ignore())
                .ForMember(dest => dest.DataRegistro, opt => opt.Ignore())
                .ForMember(dest => dest.Usuario, opt => opt.Ignore());

            CreateMap<AtualizarHumorDto, Humor>()
                .ForMember(dest => dest.IdHumor, opt => opt.Ignore())
                .ForMember(dest => dest.IdUsuario, opt => opt.Ignore())
                .ForMember(dest => dest.DataRegistro, opt => opt.Ignore())
                .ForMember(dest => dest.Usuario, opt => opt.Ignore());

            // Mapeamentos para Sprint
            CreateMap<Sprint, SprintResponseDto>()
                .ForMember(dest => dest.Links, opt => opt.Ignore());

            CreateMap<CriarSprintDto, Sprint>()
                .ForMember(dest => dest.IdSprint, opt => opt.Ignore())
                .ForMember(dest => dest.IdUsuario, opt => opt.Ignore())
                .ForMember(dest => dest.Usuario, opt => opt.Ignore());

            CreateMap<AtualizarSprintDto, Sprint>()
                .ForMember(dest => dest.IdSprint, opt => opt.Ignore())
                .ForMember(dest => dest.IdUsuario, opt => opt.Ignore())
                .ForMember(dest => dest.Usuario, opt => opt.Ignore());

            // Mapeamentos para AlertaIA
            CreateMap<AlertaIA, AlertaIAResponseDto>()
                .ForMember(dest => dest.Links, opt => opt.Ignore());

            CreateMap<CriarAlertaIADto, AlertaIA>()
                .ForMember(dest => dest.IdAlerta, opt => opt.Ignore())
                .ForMember(dest => dest.IdUsuario, opt => opt.Ignore())
                .ForMember(dest => dest.DataAlerta, opt => opt.Ignore())
                .ForMember(dest => dest.Usuario, opt => opt.Ignore());

            // Mapeamentos para Habito
            CreateMap<Habito, HabitoResponseDto>()
                .ForMember(dest => dest.Links, opt => opt.Ignore());

            CreateMap<CriarHabitoDto, Habito>()
                .ForMember(dest => dest.IdHabito, opt => opt.Ignore())
                .ForMember(dest => dest.IdUsuario, opt => opt.Ignore())
                .ForMember(dest => dest.DataHabito, opt => opt.Ignore())
                .ForMember(dest => dest.Usuario, opt => opt.Ignore());

            // Mapeamentos para Badge
            CreateMap<Badge, BadgeResponseDto>()
                .ForMember(dest => dest.Links, opt => opt.Ignore());

            CreateMap<CriarBadgeDto, Badge>()
                .ForMember(dest => dest.IdBadge, opt => opt.Ignore())
                .ForMember(dest => dest.UsuarioBadges, opt => opt.Ignore());

            CreateMap<AtualizarBadgeDto, Badge>()
                .ForMember(dest => dest.IdBadge, opt => opt.Ignore())
                .ForMember(dest => dest.UsuarioBadges, opt => opt.Ignore());

            // Mapeamentos para UsuarioBadge
            CreateMap<UsuarioBadge, UsuarioBadgeResponseDto>()
                .ForMember(dest => dest.Badge, opt => opt.MapFrom(src => src.Badge))
                .ForMember(dest => dest.Links, opt => opt.Ignore());
        }
    }
}
