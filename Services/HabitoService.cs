using AutoMapper;
using challenge_3_net.Models;
using challenge_3_net.Models.DTOs;
using challenge_3_net.Repositories.Interfaces;
using challenge_3_net.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace challenge_3_net.Services
{
    public class HabitoService : BaseService, IHabitoService
    {
        private readonly IHabitoRepository _habitoRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public HabitoService(
            IHabitoRepository habitoRepository,
            IUsuarioRepository usuarioRepository,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor) : base(mapper, httpContextAccessor)
        {
            _habitoRepository = habitoRepository;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<PagedResultDto<HabitoResponseDto>> ObterTodosAsync(int pageNumber, int pageSize)
        {
            var (items, totalCount) = await _habitoRepository.GetPagedAsync(pageNumber, pageSize);
            var responseItems = _mapper.Map<IEnumerable<HabitoResponseDto>>(items);

            foreach (var item in responseItems)
            {
                item.Links = CreateHateoasLinks(item.IdHabito, "v1.0/Habitos", GetBaseUrl());
            }

            return CreatePagedResult(responseItems, pageNumber, pageSize, totalCount, "v1.0/Habitos", GetBaseUrl());
        }

        public async Task<HabitoResponseDto?> ObterPorIdAsync(int id)
        {
            var habito = await _habitoRepository.GetByIdAsync(id);
            if (habito == null)
                return null;

            var response = _mapper.Map<HabitoResponseDto>(habito);
            response.Links = CreateHateoasLinks(id, "v1.0/Habitos", GetBaseUrl());
            return response;
        }

        public async Task<List<HabitoResponseDto>> ObterPorUsuarioAsync(int usuarioId)
        {
            var habitos = await _habitoRepository.GetByUsuarioAsync(usuarioId);
            var response = _mapper.Map<List<HabitoResponseDto>>(habitos);
            
            foreach (var item in response)
            {
                item.Links = CreateHateoasLinks(item.IdHabito, "v1.0/Habitos", GetBaseUrl());
            }

            return response;
        }

        public async Task<HabitoResponseDto> CriarAsync(CriarHabitoDto dto)
        {
            var userId = GetCurrentUserId();
            if (!userId.HasValue)
                throw new UnauthorizedAccessException("Usuário não autenticado");

            var habito = _mapper.Map<Habito>(dto);
            habito.IdUsuario = userId.Value;
            if (dto.DataHabito.HasValue)
            {
                habito.DataHabito = dto.DataHabito.Value;
            }
            else
            {
                habito.DataHabito = DateTime.UtcNow;
            }

            var habitoCriado = await _habitoRepository.AddAsync(habito);
            var response = _mapper.Map<HabitoResponseDto>(habitoCriado);
            response.Links = CreateHateoasLinks(habitoCriado.IdHabito, "v1.0/Habitos", GetBaseUrl());
            return response;
        }

        public async Task<bool> ExcluirAsync(int id)
        {
            return await _habitoRepository.RemoveByIdAsync(id);
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

