using AutoMapper;
using challenge_3_net.Models;
using challenge_3_net.Models.DTOs;
using challenge_3_net.Repositories.Interfaces;
using challenge_3_net.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace challenge_3_net.Services
{
    /// <summary>
    /// Serviço para gerenciamento de usuários
    /// </summary>
    public class UsuarioService : BaseService, IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(
            IUsuarioRepository usuarioRepository,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor) : base(mapper, httpContextAccessor)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<PagedResultDto<UsuarioResponseDto>> ObterTodosAsync(int pageNumber, int pageSize)
        {
            var (items, totalCount) = await _usuarioRepository.GetPagedAsync(pageNumber, pageSize);
            var responseItems = _mapper.Map<IEnumerable<UsuarioResponseDto>>(items);

            // Adicionar links HATEOAS para cada item
            foreach (var item in responseItems)
            {
                item.Links = CreateHateoasLinks(item.IdUsuario, "v1.0/Usuarios", GetBaseUrl());
            }

            return CreatePagedResult(responseItems, pageNumber, pageSize, totalCount, "v1.0/Usuarios", GetBaseUrl());
        }

        public async Task<UsuarioResponseDto?> ObterPorIdAsync(int id)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(id);
            if (usuario == null)
                return null;

            var response = _mapper.Map<UsuarioResponseDto>(usuario);
            response.Links = CreateHateoasLinks(id, "v1.0/Usuarios", GetBaseUrl());

            return response;
        }

        public async Task<UsuarioResponseDto> CriarAsync(CriarUsuarioDto dto)
        {
            try
            {
                // Validar se email já existe
                if (await _usuarioRepository.EmailExistsAsync(dto.Email))
                {
                    throw new InvalidOperationException("Email já está em uso");
                }

                var usuario = _mapper.Map<Usuario>(dto);
                usuario.SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha);
                usuario.DataCadastro = DateTime.UtcNow;

                var usuarioCriado = await _usuarioRepository.AddAsync(usuario);
                
                // Verificar se o ID foi gerado
                if (usuarioCriado.IdUsuario == 0)
                {
                    throw new InvalidOperationException("Falha ao gerar ID do usuário. Verifique a configuração do banco de dados.");
                }

                var response = _mapper.Map<UsuarioResponseDto>(usuarioCriado);
                response.Links = CreateHateoasLinks(usuarioCriado.IdUsuario, "v1.0/Usuarios", GetBaseUrl());

                return response;
            }
            catch (DbUpdateException dbEx)
            {
                throw new InvalidOperationException($"Erro ao salvar no banco de dados: {dbEx.Message}", dbEx);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Erro ao criar usuário: {ex.Message}", ex);
            }
        }

        public async Task<UsuarioResponseDto?> AtualizarAsync(int id, AtualizarUsuarioDto dto)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(id);
            if (usuario == null)
                return null;

            // Validar se email já existe (excluindo o próprio usuário)
            if (await _usuarioRepository.EmailExistsAsync(dto.Email, id))
            {
                throw new InvalidOperationException("Email já está em uso");
            }

            _mapper.Map(dto, usuario);

            var usuarioAtualizado = await _usuarioRepository.UpdateAsync(usuario);
            var response = _mapper.Map<UsuarioResponseDto>(usuarioAtualizado);
            response.Links = CreateHateoasLinks(id, "v1.0/Usuarios", GetBaseUrl());

            return response;
        }

        public async Task<bool> ExcluirAsync(int id)
        {
            return await _usuarioRepository.RemoveByIdAsync(id);
        }

        public async Task<bool> ExisteAsync(int id)
        {
            return await _usuarioRepository.ExistsAsync(id);
        }

        public async Task<UsuarioResponseDto?> ObterPorEmailAsync(string email)
        {
            var usuario = await _usuarioRepository.GetByEmailAsync(email);
            if (usuario == null)
                return null;

            var response = _mapper.Map<UsuarioResponseDto>(usuario);
            response.Links = CreateHateoasLinks(usuario.IdUsuario, "v1.0/Usuarios", GetBaseUrl());

            return response;
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
