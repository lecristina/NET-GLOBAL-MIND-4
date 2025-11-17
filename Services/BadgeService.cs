using AutoMapper;
using nexus.Models;
using nexus.Models.DTOs;
using nexus.Repositories.Interfaces;
using nexus.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace nexus.Services
{
    public class BadgeService : BaseService, IBadgeService
    {
        private readonly IBadgeRepository _badgeRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public BadgeService(
            IBadgeRepository badgeRepository,
            IUsuarioRepository usuarioRepository,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor) : base(mapper, httpContextAccessor)
        {
            _badgeRepository = badgeRepository;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<PagedResultDto<BadgeResponseDto>> ObterTodosAsync(int pageNumber, int pageSize)
        {
            var (items, totalCount) = await _badgeRepository.GetPagedAsync(pageNumber, pageSize);
            var responseItems = _mapper.Map<IEnumerable<BadgeResponseDto>>(items);

            foreach (var item in responseItems)
            {
                item.Links = CreateHateoasLinks(item.IdBadge, "v1.0/Badges", GetBaseUrl());
            }

            return CreatePagedResult(responseItems, pageNumber, pageSize, totalCount, "v1.0/Badges", GetBaseUrl());
        }

        public async Task<BadgeResponseDto?> ObterPorIdAsync(int id)
        {
            var badge = await _badgeRepository.GetByIdAsync(id);
            if (badge == null)
                return null;

            var response = _mapper.Map<BadgeResponseDto>(badge);
            response.Links = CreateHateoasLinks(id, "v1.0/Badges", GetBaseUrl());
            return response;
        }

        public async Task<List<UsuarioBadgeResponseDto>> ObterPorUsuarioAsync(int usuarioId)
        {
            var usuarioBadges = await _badgeRepository.GetByUsuarioAsync(usuarioId);
            var response = _mapper.Map<List<UsuarioBadgeResponseDto>>(usuarioBadges);
            
            foreach (var item in response)
            {
                item.Links = CreateHateoasLinks(item.IdBadge, "v1.0/Badges", GetBaseUrl());
            }

            return response;
        }

        public async Task<BadgeResponseDto> CriarAsync(CriarBadgeDto dto)
        {
            var badge = _mapper.Map<Badge>(dto);
            var badgeCriado = await _badgeRepository.AddAsync(badge);
            var response = _mapper.Map<BadgeResponseDto>(badgeCriado);
            response.Links = CreateHateoasLinks(badgeCriado.IdBadge, "v1.0/Badges", GetBaseUrl());
            return response;
        }

        public async Task<BadgeResponseDto?> AtualizarAsync(int id, AtualizarBadgeDto dto)
        {
            var badge = await _badgeRepository.GetByIdAsync(id);
            if (badge == null)
                return null;

            _mapper.Map(dto, badge);
            var badgeAtualizado = await _badgeRepository.UpdateAsync(badge);
            var response = _mapper.Map<BadgeResponseDto>(badgeAtualizado);
            response.Links = CreateHateoasLinks(id, "v1.0/Badges", GetBaseUrl());
            return response;
        }

        public async Task<UsuarioBadgeResponseDto?> ConcederBadgeAsync(int usuarioId, int badgeId)
        {
            // Verificar se usuário existe
            var usuario = await _usuarioRepository.GetByIdAsync(usuarioId);
            if (usuario == null)
                return null;

            // Verificar se badge existe
            var badge = await _badgeRepository.GetByIdAsync(badgeId);
            if (badge == null)
                return null;

            // Verificar se já possui o badge
            var existing = await _badgeRepository.GetUsuarioBadgeAsync(usuarioId, badgeId);
            if (existing != null)
                throw new InvalidOperationException("Usuário já possui este badge");

            var usuarioBadge = new UsuarioBadge
            {
                IdUsuario = usuarioId,
                IdBadge = badgeId,
                DataConquista = DateTime.UtcNow
            };

            var usuarioBadgeCriado = await _badgeRepository.AddUsuarioBadgeAsync(usuarioBadge);
            var response = _mapper.Map<UsuarioBadgeResponseDto>(usuarioBadgeCriado);
            response.Links = CreateHateoasLinks(badgeId, "v1.0/Badges", GetBaseUrl());
            return response;
        }

        public async Task<bool> ExcluirAsync(int id)
        {
            return await _badgeRepository.RemoveByIdAsync(id);
        }

        private string GetBaseUrl()
        {
            var request = _httpContextAccessor?.HttpContext?.Request;
            if (request == null)
                return "https://localhost:7000";
            return $"{request.Scheme}://{request.Host}";
        }
    }
}

