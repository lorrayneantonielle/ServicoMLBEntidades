using ServicoMLBEntidades.Application.Common.Exceptions;
using ServicoMLBEntidades.Application.Membros.Commands;
using ServicoMLBEntidades.Domain.Entities;
using ServicoMLBEntidades.Domain.Repositories;

namespace ServicoMLBEntidades.Application.Membros;

public class MembroService
{
    private readonly IFamiliaRepository _familiaRepository;

    public MembroService(IFamiliaRepository familiaRepository)
    {
        _familiaRepository = familiaRepository;
    }

    public async Task<List<MembroResponse>> ListMembrosPorFamiliaAsync(Guid familiaId, CancellationToken ct = default)
    {
        var familia = await _familiaRepository.ObterPorIdAsync(familiaId, ct)
            ?? throw new NotFoundException("Família não encontrada.");

        return familia.Membros.Select(MapToResponse).ToList();
    }

    public async Task<MembroResponse> CreateMembroAsync(MembroCommand command, CancellationToken ct = default)
    {
        var familia = await _familiaRepository.ObterPorIdAsync(command.FamiliaId, ct)
            ?? throw new NotFoundException("Família não encontrada.");

        if (await _familiaRepository.ExisteCpfEmFamiliaAtivaAsync(command.Cpf, null, ct))
        {
            throw new ConflictException($"CPF {command.Cpf} já está vinculado a outra família ativa.");
        }

        var membro = new Membro
        {
            Id = Guid.NewGuid(),
            FamiliaId = familia.Id,
            Nome = command.Nome,
            DataNascimento = command.DataNascimento,
            Vinculo = command.Vinculo,
            Cpf = command.Cpf,
        };

        familia.Membros.Add(membro);
        await _familiaRepository.SalvarAlteracoesAsync(ct);

        return MapToResponse(membro);
    }

    public async Task<MembroResponse> UpdateMembroAsync(Guid id, MembroCommand command, CancellationToken ct = default)
    {
        var familia = await _familiaRepository.ObterPorMembroIdAsync(id, ct)
            ?? throw new NotFoundException("Membro não encontrado.");

        var membro = familia.Membros.First(m => m.Id == id);

        if (!string.Equals(membro.Cpf, command.Cpf, StringComparison.Ordinal)
            && await _familiaRepository.ExisteCpfEmFamiliaAtivaAsync(command.Cpf, id, ct))
        {
            throw new ConflictException($"CPF {command.Cpf} já está vinculado a outra família ativa.");
        }

        membro.Nome = command.Nome;
        membro.DataNascimento = command.DataNascimento;
        membro.Vinculo = command.Vinculo;
        membro.Cpf = command.Cpf;

        await _familiaRepository.SalvarAlteracoesAsync(ct);

        return MapToResponse(membro);
    }

    public async Task DeleteMembroAsync(Guid id, CancellationToken ct = default)
    {
        var familia = await _familiaRepository.ObterPorMembroIdAsync(id, ct)
            ?? throw new NotFoundException("Membro não encontrado.");

        var membro = familia.Membros.First(m => m.Id == id);
        familia.Membros.Remove(membro);

        await _familiaRepository.SalvarAlteracoesAsync(ct);
    }

    internal static MembroResponse MapToResponse(Membro membro) => new()
    {
        Id = membro.Id,
        FamiliaId = membro.FamiliaId,
        Nome = membro.Nome,
        DataNascimento = membro.DataNascimento,
        Vinculo = membro.Vinculo,
        Cpf = membro.Cpf,
    };
}
