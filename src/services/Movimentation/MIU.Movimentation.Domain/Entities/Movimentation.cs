using MIU.Core.Domain;
using MIU.Core.ValueObjects;
using System;

namespace MIU.Movimentations.Domain.Entities
{
    public class Movimentation : Entity, IAggregateRoot
    {
        public Movimentation(string tributeCode, 
                             string customerName,
                             string cpf, 
                             DateTime movimentationDate, 
                             string tributeDescription, 
                             int tributeAliquot, 
                             int movimentationGain)
        {
            TributeCode = tributeCode;
            CustomerName = customerName;
            Cpf = new CPF(cpf);
            MovimentationDate = movimentationDate;
            TributeDescription = tributeDescription;
            TributeAliquot = tributeAliquot;
            MovimentationGain = movimentationGain;
            GenerateMovimentationLoss();
        }

        protected Movimentation(){ }

        public string TributeCode { get; set; }
        public string CustomerName { get; private set; }
        public CPF Cpf { get; private set; }
        public DateTime MovimentationDate { get; private set; }
        public string TributeDescription { get; private set; }
        public int TributeAliquot { get; private set; }
        public int MovimentationGain { get; private set; }
        public int MovimentationLoss { get; private set; }

        public void UpdateMovimentation(string tributeCode,
                                        string customerName,
                                        string cpf,
                                        DateTime movimentationDate,
                                        string tributeDescription,
                                        int tributeAliquot,
                                        int movimentationGain)
        {
            TributeCode = tributeCode;
            CustomerName = customerName;
            Cpf = new CPF(cpf);
            MovimentationDate = movimentationDate;
            TributeDescription = tributeDescription;
            TributeAliquot = tributeAliquot;
            MovimentationGain = movimentationGain;
            GenerateMovimentationLoss();
        }

        public void GenerateMovimentationLoss()
        {
            MovimentationLoss = (int)Math.Round((double)(TributeAliquot * MovimentationGain) / 100);
            MovimentationGain = MovimentationGain - (int)Math.Round((double)(TributeAliquot * MovimentationGain) / 100);
        }
    }
}
