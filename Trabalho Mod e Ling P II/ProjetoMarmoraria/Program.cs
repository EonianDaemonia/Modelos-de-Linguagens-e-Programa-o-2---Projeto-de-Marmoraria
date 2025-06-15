
using ProjetoMarmoraria.Data;
using ProjetoMarmoraria.Models;


Database.Initialize(); 

Console.WriteLine("--- Sistema de Gestão de Marmoraria ---");

Usuario? usuarioLogado = EfetuarLogin();

if (usuarioLogado != null)
{
    Console.WriteLine($"\nBem-vindo, {usuarioLogado.Nome ?? "Usuário"}!");
    ExibirMenuPrincipal();
}

Console.WriteLine("\nObrigado por usar o sistema!");



Usuario? EfetuarLogin()
{
    var usuarioRepo = new UsuarioRepository();
    Usuario? usuario = null;
    while (usuario == null)
    {
        Console.Write("Login: ");
        string login = Console.ReadLine() ?? "";
        Console.Write("Senha: ");
        string senha = Console.ReadLine() ?? "";

        usuario = usuarioRepo.ValidarLogin(login, senha);

        if (usuario == null)
            Console.WriteLine("Login ou senha inválidos. Tente novamente.\n");
    }
    return usuario;
}

void ExibirMenuPrincipal()
{
    var blocoRepo = new BlocoRepository();
    var chapaRepo = new ChapaRepository();
    var usuarioRepo = new UsuarioRepository();
    
    bool sair = false;
    while (!sair)
    {
        Console.WriteLine("\n--- Menu Principal ---");
        Console.WriteLine("1. Cadastrar Usuário");
        Console.WriteLine("2. Cadastrar Bloco");
        Console.WriteLine("3. Cadastrar Chapa Avulsa");
        Console.WriteLine("4. Processo de Serragem (Bloco -> Chapas)");
        Console.WriteLine("5. Listar Blocos em Estoque");
        Console.WriteLine("6. Listar Chapas em Estoque");
        Console.WriteLine("0. Sair");
        Console.Write("Escolha uma opção: ");

        switch (Console.ReadLine())
        {
            case "1": CadastrarUsuario(usuarioRepo); break;
            case "2": CadastrarBloco(blocoRepo); break;
            case "3": CadastrarChapaAvulsa(chapaRepo); break;
            case "4": ProcessoSerragem(blocoRepo, chapaRepo); break;
            case "5": ListarBlocos(blocoRepo); break;
            case "6": ListarChapas(chapaRepo); break;
            case "0": sair = true; break;
            default: Console.WriteLine("Opção inválida!"); break;
        }
    }
}

void CadastrarUsuario(UsuarioRepository repo)
{
    Console.WriteLine("\n--- Cadastro de Novo Usuário ---");
    var usuario = new Usuario();

    Console.Write("Nome Completo: ");
    usuario.Nome = Console.ReadLine();
    Console.Write("Login: ");
    usuario.Login = Console.ReadLine();
    Console.Write("Senha: ");
    usuario.Senha = Console.ReadLine();

    if (!string.IsNullOrEmpty(usuario.Nome) && !string.IsNullOrEmpty(usuario.Login) && !string.IsNullOrEmpty(usuario.Senha))
    {
        repo.Cadastrar(usuario);
        Console.WriteLine("Usuário cadastrado com sucesso!");
    }
    else
    {
        Console.WriteLine("Dados inválidos. O cadastro foi cancelado.");
    }
}

void CadastrarBloco(BlocoRepository repo)
{
    Console.WriteLine("\n--- Cadastro de Novo Bloco ---");
    try
    {
        var bloco = new Bloco();
        
        Console.Write("Código do Bloco (Ex: BL-001): ");
        bloco.CodigoBloco = Console.ReadLine();
        Console.Write("Pedreira de Origem: ");
        bloco.PedreiraOrigem = Console.ReadLine();
        Console.Write("Metragem Cúbica (M³): ");
        bloco.MetragemCubica = double.Parse(Console.ReadLine() ?? "0");
        Console.Write("Tipo de Material (Ex: Granito Amarelo): ");
        bloco.TipoMaterial = Console.ReadLine();
        Console.Write("Valor de Compra (R$): ");
        bloco.ValorCompra = decimal.Parse(Console.ReadLine() ?? "0");
        Console.Write("Número da Nota Fiscal: ");
        bloco.NumeroNotaFiscal = Console.ReadLine();

        repo.Cadastrar(bloco);
        Console.WriteLine("Bloco cadastrado com sucesso!");
    }
    catch (FormatException)
    {
        Console.WriteLine("Erro: Valor numérico inválido. O cadastro foi cancelado.");
    }
}

void CadastrarChapaAvulsa(ChapaRepository repo)
{
    Console.WriteLine("\n--- Cadastro de Chapa Avulsa ---");
    try
    {
        var chapa = new Chapa();
        
        Console.Write("Tipo de Material: ");
        chapa.TipoMaterial = Console.ReadLine();
        Console.Write("Altura (m): ");
        chapa.Altura = double.Parse(Console.ReadLine() ?? "0");
        Console.Write("Largura (m): ");
        chapa.Largura = double.Parse(Console.ReadLine() ?? "0");
        Console.Write("Valor (R$): ");
        chapa.Valor = decimal.Parse(Console.ReadLine() ?? "0");
        chapa.BlocoOrigemId = null; 

        repo.Cadastrar(chapa);
        Console.WriteLine("Chapa avulsa cadastrada com sucesso!");
    }
    catch (FormatException)
    {
        Console.WriteLine("Erro: Valor numérico inválido. O cadastro foi cancelado.");
    }
}

void ProcessoSerragem(BlocoRepository blocoRepo, ChapaRepository chapaRepo)
{
    Console.WriteLine("\n--- Processo de Serragem ---");
    ListarBlocos(blocoRepo);

    var blocos = blocoRepo.Listar();
    if (!blocos.Any()) return;

    try
    {
        Console.Write("\nDigite o ID do bloco que será serrado: ");
        int idBloco = int.Parse(Console.ReadLine() ?? "0");

        var blocoEscolhido = blocos.FirstOrDefault(b => b.Id == idBloco);

        if (blocoEscolhido == null)
        {
            Console.WriteLine("Bloco não encontrado!");
            return;
        }

        const double m3PorChapa = 0.05;
        int numeroDeChapas = (int)(blocoEscolhido.MetragemCubica / m3PorChapa);
        if (numeroDeChapas == 0)
        {
             Console.WriteLine("A metragem do bloco é insuficiente para gerar chapas.");
             return;
        }
        
        decimal valorPorChapa = (blocoEscolhido.ValorCompra / numeroDeChapas) * 1.20m;

        for (int i = 0; i < numeroDeChapas; i++)
        {
            var novaChapa = new Chapa
            {
                BlocoOrigemId = blocoEscolhido.Id,
                TipoMaterial = blocoEscolhido.TipoMaterial,
                Altura = 2.80,
                Largura = 1.80,
                Valor = Math.Round(valorPorChapa, 2)
            };
            chapaRepo.Cadastrar(novaChapa);
        }
        
        Console.WriteLine($"{numeroDeChapas} chapas geradas do bloco {blocoEscolhido.CodigoBloco} e adicionadas ao estoque!");
    }
    catch (FormatException)
    {
        Console.WriteLine("Erro: ID inválido.");
    }
}

void ListarBlocos(BlocoRepository repo)
{
    Console.WriteLine("\n--- Lista de Blocos em Estoque ---");
    var blocos = repo.Listar();

    if (!blocos.Any())
    {
        Console.WriteLine("Nenhum bloco cadastrado.");
        return;
    }

    foreach (var bloco in blocos)
    {
        Console.WriteLine($"ID: {bloco.Id} | Cód: {bloco.CodigoBloco} | Material: {bloco.TipoMaterial} | M³: {bloco.MetragemCubica} | Valor: R$ {bloco.ValorCompra:F2}");
    }
}

void ListarChapas(ChapaRepository repo)
{
    Console.WriteLine("\n--- Lista de Chapas em Estoque ---");
    var chapas = repo.Listar();

    if (!chapas.Any())
    {
        Console.WriteLine("Nenhuma chapa cadastrada.");
        return;
    }

    foreach (var chapa in chapas)
    {
        string origem = chapa.BlocoOrigemId.HasValue ? chapa.BlocoOrigemId.ToString() : "Avulsa";
        Console.WriteLine($"ID: {chapa.Id} | Origem (ID Bloco): {origem} | Material: {chapa.TipoMaterial} | Medidas: {chapa.Altura:F2}x{chapa.Largura:F2}m | Valor: R$ {chapa.Valor:F2}");
    }
}