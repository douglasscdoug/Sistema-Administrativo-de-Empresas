using SistemaEmpresas.Domain.Entities;

namespace SistemaEmpresas.API.Seed;

public static class DemoData
{
    public static List<Empresa> Empresas => new()
    {
        new Empresa
        {
            RazaoSocial = "Tech Solutions Ltda",
            Cnpj = "22273738000165",
            Ativo = true,
            Endereco = new Endereco
            {
                Logradouro = "Rua das Flores",
                Numero = "123",
                Bairro = "Centro",
                Cidade = "São Paulo",
                Estado = "SP",
                Cep = "01001000"
            },
            Contato = new Contato
            {
                Nome = "João Silva",
                Email = "contato@techsolutions.com.br",
                Telefone = "11987654321"
            }
        },
        new Empresa
        {
            RazaoSocial = "Alpha Consultoria Empresarial Ltda",
            Cnpj = "76506363000165",
            Ativo = true,
            Endereco = new Endereco
            {
                Logradouro = "Av. Paulista",
                Numero = "1500",
                Bairro = "Bela Vista",
                Cidade = "São Paulo",
                Estado = "SP",
                Cep = "01310200"
            },
            Contato = new Contato
            {
                Nome = "Mariana Costa",
                Email = "mariana@alphaconsultoria.com.br",
                Telefone = "11991234567"
            }
        },

        new Empresa
        {
            RazaoSocial = "Beta Logística e Transportes Ltda",
            Cnpj = "35051008000161",
            Ativo = false,
            Endereco = new Endereco
            {
                Logradouro = "Rua Industrial",
                Numero = "450",
                Bairro = "Distrito Industrial",
                Cidade = "Campinas",
                Estado = "SP",
                Cep = "13050000"
            },
            Contato = new Contato
            {
                Nome = "Carlos Mendes",
                Email = "carlos@betalogistica.com.br",
                Telefone = "19988776655"
            }
        },

        new Empresa
        {
            RazaoSocial = "Gamma Tecnologia da Informação Ltda",
            Cnpj = "74459804000190",
            Ativo = true,
            Endereco = new Endereco
            {
                Logradouro = "Rua da Inovação",
                Numero = "88",
                Bairro = "Centro",
                Cidade = "Curitiba",
                Estado = "PR",
                Cep = "80010000"
            },
            Contato = new Contato
            {
                Nome = "Fernanda Rocha",
                Email = "fernanda@gammati.com.br",
                Telefone = "41997775544"
            }
        },

        new Empresa
        {
            RazaoSocial = "Delta Engenharia Ltda",
            Cnpj = "41140673000124",
            Ativo = true,
            Endereco = new Endereco
            {
                Logradouro = "Rua das Acácias",
                Numero = "321",
                Bairro = "Savassi",
                Cidade = "Belo Horizonte",
                Estado = "MG",
                Cep = "30112000"
            },
            Contato = new Contato
            {
                Nome = "Ricardo Almeida",
                Email = "ricardo@deltaengenharia.com.br",
                Telefone = "31998887766"
            }
        },

        new Empresa
        {
            RazaoSocial = "Omega Comércio de Alimentos Ltda",
            Cnpj = "43443703000105",
            Ativo = true,
            Endereco = new Endereco
            {
                Logradouro = "Av. Brasil",
                Numero = "789",
                Bairro = "Centro",
                Cidade = "Rio de Janeiro",
                Estado = "RJ",
                Cep = "20040002"
            },
            Contato = new Contato
            {
                Nome = "Patrícia Souza",
                Email = "patricia@omegacomercio.com.br",
                Telefone = "21996665544"
            }
        },

        new Empresa
        {
            RazaoSocial = "Vision Marketing Digital Ltda",
            Cnpj = "98346553000103",
            Ativo = false,
            Endereco = new Endereco
            {
                Logradouro = "Rua XV de Novembro",
                Numero = "456",
                Bairro = "Centro",
                Cidade = "Florianópolis",
                Estado = "SC",
                Cep = "88010000"
            },
            Contato = new Contato
            {
                Nome = "Lucas Martins",
                Email = "lucas@visionmarketing.com.br",
                Telefone = "48995554433"
            }
        },

        new Empresa
        {
            RazaoSocial = "Prime Sistemas Corporativos Ltda",
            Cnpj = "36210753000179",
            Ativo = true,
            Endereco = new Endereco
            {
                Logradouro = "Rua dos Andradas",
                Numero = "900",
                Bairro = "Centro Histórico",
                Cidade = "Porto Alegre",
                Estado = "RS",
                Cep = "90020004"
            },
            Contato = new Contato
            {
                Nome = "Juliana Ferreira",
                Email = "juliana@primesistemas.com.br",
                Telefone = "51994443322"
            }
        },

        new Empresa
        {
            RazaoSocial = "Nexus Consultoria Financeira Ltda",
            Cnpj = "99917418000125",
            Ativo = true,
            Endereco = new Endereco
            {
                Logradouro = "Av. Tancredo Neves",
                Numero = "2222",
                Bairro = "Caminho das Árvores",
                Cidade = "Salvador",
                Estado = "BA",
                Cep = "41820021"
            },
            Contato = new Contato
            {
                Nome = "Roberto Lima",
                Email = "roberto@nexusfinanceira.com.br",
                Telefone = "71993332211"
            }
        },

        new Empresa
        {
            RazaoSocial = "Solar Energia Sustentável Ltda",
            Cnpj = "85149418000118",
            Ativo = true,
            Endereco = new Endereco
            {
                Logradouro = "Rua das Palmeiras",
                Numero = "75",
                Bairro = "Jardim América",
                Cidade = "Goiânia",
                Estado = "GO",
                Cep = "74230030"
            },
            Contato = new Contato
            {
                Nome = "Amanda Ribeiro",
                Email = "amanda@solarenergia.com.br",
                Telefone = "62992221100"
            }
        }
    };
}