using AutoMapper;
using OpenNos.DAL.EF;
using OpenNos.Data;
using OpenNos.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenNos.Mapper;

namespace OpenNos.MapperBenchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            Benchmark(1000000, 2);
            Benchmark(1000000, 1);
            Console.ReadLine();
        }


        static void Benchmark(int iterations, int test)
        {
            double totalMiliseconds = 0;
            switch (test)
            {
                case 1:
                    {
                        Console.WriteLine("=== TEST: AutoMapper ===");

                        AccountDTO accountDTO = new AccountDTO()
                        {
                            AccountId = 12345,
                            Authority = AuthorityType.GameMaster,
                            Email = "fgdsafioasd@fvadsjifse.de",
                            Name = "fdsafdsa",
                            Password = "dfsaoiujasduiofhj43wifhj98chsa97084c mnhz34782zj3e4wgt87tgfchbjnikmbhszregv7ft8wfjbn",
                            ReferrerId = 43295834,
                            RegistrationIP = "192.168.178.1",
                            VerificationToken = "hfdsa9uf h8329 z0237 z087adsfghj iahgfw874gfjhsdbaiuzf"
                        };

                        Account account = new Account()
                        {
                            AccountId = 3245342,
                            Authority = AuthorityType.GameMaster,
                            Email = "sdfgsdfgsdfg@gfdsgfds4fdgsg.de",
                            Name = "gsdfgsdf",
                            Password = "gsfdgsdfgsd4 43 nufh7sdtz87ogvcjhkseg87rgizg87gsjhdfguzwef",
                            ReferrerId = 65437365645,
                            RegistrationIP = "534643 gfdeg45",
                            VerificationToken = "hjcrsdm io398r9z9f7zds78fz7ust3jkbkizuagsh43rbifs68g"
                        };

                        AccountDTO accountDTO2 = new AccountDTO();
                        Account account2 = new Account();

                        MapperConfiguration config = new MapperConfiguration(cfg =>
                        {
                            cfg.CreateMap(typeof(AccountDTO), typeof(Account));
                            cfg.CreateMap(typeof(Account), typeof(AccountDTO));
                        });

                        IMapper mapper = config.CreateMapper();

                        Stopwatch sw = Stopwatch.StartNew();

                        for (int i = 0; i < iterations; i++)
                        {
                            account2 = mapper.Map<Account>(accountDTO);
                            accountDTO2 = mapper.Map<AccountDTO>(account);
                            account2 = new Account();
                            accountDTO2 = new AccountDTO();
                        }
                        sw.Stop();
                        totalMiliseconds = sw.Elapsed.TotalMilliseconds;
                    }
                    break;

                case 2:
                    {
                        Console.WriteLine("=== TEST: ManualMapper ===");
                        AccountDTO accountDTO = new AccountDTO()
                        {
                            AccountId = 12345,
                            Authority = AuthorityType.GameMaster,
                            Email = "fgdsafioasd@fvadsjifse.de",
                            Name = "fdsafdsa",
                            Password = "dfsaoiujasduiofhj43wifhj98chsa97084c mnhz34782zj3e4wgt87tgfchbjnikmbhszregv7ft8wfjbn",
                            ReferrerId = 43295834,
                            RegistrationIP = "192.168.178.1",
                            VerificationToken = "hfdsa9uf h8329 z0237 z087adsfghj iahgfw874gfjhsdbaiuzf"
                        };

                        Account account = new Account()
                        {
                            AccountId = 3245342,
                            Authority = AuthorityType.GameMaster,
                            Email = "sdfgsdfgsdfg@gfdsgfds4fdgsg.de",
                            Name = "gsdfgsdf",
                            Password = "gsfdgsdfgsd4 43 nufh7sdtz87ogvcjhkseg87rgizg87gsjhdfguzwef",
                            ReferrerId = 65437365645,
                            RegistrationIP = "534643 gfdeg45",
                            VerificationToken = "hjcrsdm io398r9z9f7zds78fz7ust3jkbkizuagsh43rbifs68g"
                        };

                        AccountDTO accountDTO2 = new AccountDTO();
                        Account account2 = new Account();

                        if (Mapper.Mapper.Instance.AccountMapper != null)
                        {

                        }

                        Stopwatch sw = Stopwatch.StartNew();
                        for (int i = 0; i < iterations; i++)
                        {
                            Mapper.Mapper.Instance.AccountMapper.ToAccount(accountDTO, account2);
                            Mapper.Mapper.Instance.AccountMapper.ToAccountDTO(account, accountDTO2);
                            account2 = new Account();
                            accountDTO2 = new AccountDTO();
                        }
                        sw.Stop();
                        totalMiliseconds = sw.Elapsed.TotalMilliseconds;
                    }
                    break;
            }

            Console.WriteLine($"The test with {iterations} iterations took {totalMiliseconds} ms");
            Console.WriteLine($"The each iteration took {((totalMiliseconds * 1000000) / iterations).ToString("0.00 ns")}");
        }
    }
}
