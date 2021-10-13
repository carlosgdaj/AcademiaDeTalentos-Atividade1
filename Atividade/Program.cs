using Atividade.Model;
using Microsoft.Xrm.Sdk;
using System;

namespace Atividade
{
    class Program
    {
        static void Main(string[] args)
        {
            IOrganizationService service = ConnectionFactory.GetCrmService();
            Oportunity oportunity = new Oportunity(service);
            Console.WriteLine("Qual oportunidade você deseja aplicar o desconto ? ");
            Guid idOpotunity = new Guid(Console.ReadLine());//id = 67ef9454-6529-4e53-a1be-c2ec205db4d6
            Console.WriteLine("________________________________________________________________________________________");

            EntityCollection oportunityCRM = oportunity.RetrieveMultipleAccountByOportunity(idOpotunity);
            foreach(Entity oportunitycrm in oportunityCRM.Entities)
            {
                Console.WriteLine();
                Console.WriteLine($"O nome da oportunidade é: {oportunitycrm["name"]}");
                Console.WriteLine("________________________________________________________________________________________");

                EntityReference accountId = (EntityReference)oportunitycrm["parentaccountid"];

                Account account = new Account(service);
                EntityCollection accountCRM = account.RetrieveMultipleNivelByAccount(accountId.Id);
                foreach(Entity accountcrm in accountCRM.Entities)
                {
                    EntityReference nivelDoClienteId = (EntityReference)accountcrm["grp_niveldocliente"];

                    NivelDoCliente nivelDoCliente = new NivelDoCliente(service);
                    EntityCollection nivelDoClienteCRM = nivelDoCliente.RetrieveMultipleNivelDoCliente(nivelDoClienteId.Id);
                    foreach(Entity niveldoclientecrm in nivelDoClienteCRM.Entities)
                    {
                        int desconto = (int)niveldoclientecrm["grp_valordodesconto"];

                        Percentage percentage = new Percentage();
                        double valorComPorcentagem = percentage.Porcentagem(desconto, (Money)oportunitycrm["totallineitemamount"]);

                        Console.WriteLine("");
                        percentage.ValorTotal(desconto, (Money)oportunitycrm["totallineitemamount"]);
                        Console.WriteLine("________________________________________________________________________________________");

                        Console.WriteLine();
                        Console.WriteLine("Você deseja atualizar essa oportunidade ? Y/N");
                        string resultado = Console.ReadLine();
                        Console.WriteLine("________________________________________________________________________________________");
                        if(resultado == "y" || resultado == "Y")
                        {
                            oportunity.UpdateOpportunity(idOpotunity, valorComPorcentagem);
                            Console.WriteLine();
                            Console.WriteLine("Oportunidade atualizada com sucesso!!");
                        }
                        else if(resultado == "n" || resultado == "N")
                        {
                            Console.WriteLine();
                            Console.WriteLine("Obrigado pela atenção!");
                        }
                        else
                        {
                            Console.WriteLine();
                            Console.WriteLine("Não foi possivel entender sua resposta. Obrigado pela atenção!"); 
                        }
                    }
                }
            }
            Console.ReadKey();
        }
    }
}
