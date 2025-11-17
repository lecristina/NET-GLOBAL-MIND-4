using AutoMapper;
using challenge_3_net.Models;
using challenge_3_net.Models.DTOs;
using challenge_3_net.Repositories.Interfaces;
using challenge_3_net.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace challenge_3_net.Services
{
    public class HumorService : BaseService, IHumorService
    {
        private readonly IHumorRepository _humorRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public HumorService(
            IHumorRepository humorRepository,
            IUsuarioRepository usuarioRepository,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor) : base(mapper, httpContextAccessor)
        {
            _humorRepository = humorRepository;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<PagedResultDto<HumorResponseDto>> ObterTodosAsync(int pageNumber, int pageSize)
        {
            var (items, totalCount) = await _humorRepository.GetPagedAsync(pageNumber, pageSize);
            var responseItems = _mapper.Map<IEnumerable<HumorResponseDto>>(items);

            foreach (var item in responseItems)
            {
                item.Links = CreateHateoasLinks(item.IdHumor, "v1.0/Humor", GetBaseUrl());
            }

            return CreatePagedResult(responseItems, pageNumber, pageSize, totalCount, "v1.0/Humor", GetBaseUrl());
        }

        public async Task<HumorResponseDto?> ObterPorIdAsync(int id)
        {
            var humor = await _humorRepository.GetByIdAsync(id);
            if (humor == null)
                return null;

            var response = _mapper.Map<HumorResponseDto>(humor);
            response.Links = CreateHateoasLinks(id, "v1.0/Humor", GetBaseUrl());
            return response;
        }

        public async Task<List<HumorResponseDto>> ObterPorUsuarioAsync(int usuarioId)
        {
            var humores = await _humorRepository.GetByUsuarioAsync(usuarioId);
            var response = _mapper.Map<List<HumorResponseDto>>(humores);
            
            foreach (var item in response)
            {
                item.Links = CreateHateoasLinks(item.IdHumor, "v1.0/Humor", GetBaseUrl());
            }

            return response;
        }

        public async Task<HumorResponseDto> CriarAsync(CriarHumorDto dto)
        {
            var userId = GetCurrentUserId();
            if (!userId.HasValue)
                throw new UnauthorizedAccessException("Usuário não autenticado");

            var humor = _mapper.Map<Humor>(dto);
            humor.IdUsuario = userId.Value;
            humor.DataRegistro = DateTime.UtcNow;

            var humorCriado = await _humorRepository.AddAsync(humor);
            var response = _mapper.Map<HumorResponseDto>(humorCriado);
            response.Links = CreateHateoasLinks(humorCriado.IdHumor, "v1.0/Humor", GetBaseUrl());
            return response;
        }

        public async Task<HumorResponseDto?> AtualizarAsync(int id, AtualizarHumorDto dto)
        {
            var humor = await _humorRepository.GetByIdAsync(id);
            if (humor == null)
                return null;

            _mapper.Map(dto, humor);
            var humorAtualizado = await _humorRepository.UpdateAsync(humor);
            var response = _mapper.Map<HumorResponseDto>(humorAtualizado);
            response.Links = CreateHateoasLinks(id, "v1.0/Humor", GetBaseUrl());
            return response;
        }

        public async Task<bool> ExcluirAsync(int id)
        {
            return await _humorRepository.RemoveByIdAsync(id);
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

