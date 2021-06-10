export class Movimentation {
  id: string;
  customerName: string;
  cpf: string;
  movimentationDate: string;
  tributeCode: string;
  tributeDescription: string;
  tributeAliquot: number;
  movimentationGain: number;
  movimentationLoss: number;

  constructor(id: string = "", customerName: string, cpf: string, movimentationDate: string, tributeCode: string, tributeDescription: string, tributeAliquot: number, movimentationGain: number) {
    this.id = id;
    this.customerName = customerName;
    this.cpf = cpf;
    this.movimentationDate = movimentationDate;
    this.tributeCode = tributeCode;
    this.tributeDescription = tributeDescription;
    this.tributeAliquot = tributeAliquot;
    this.movimentationGain = movimentationGain;
  }

  isValid(): Array<Error> {
    var errors = new Array<Error>();

    if (!this.customerName)
      errors.push(new Error("O nome do cliente deve ser preenchido"));

    if (!this.cpf)
      errors.push(new Error("O cpf deve ser preenchido"));

    if (!this.movimentationDate)
      errors.push(new Error("A data da movimentação deve ser preenchido"));

    if (!this.isValidDate(this.movimentationDate))
      errors.push(new Error("A data da movimentação está em um formato inválido"));

    if (!this.tributeCode)
      errors.push(new Error("O código do tributo deve ser preenchido"));

    if (!this.tributeDescription)
      errors.push(new Error("A descrição do tributo deve ser preenchido"));

    if (!this.tributeAliquot)
      errors.push(new Error("A alíquota de tributo deve ser preenchida"));

    if (!this.movimentationGain)
      errors.push(new Error("O valor rendimento da movimentação deve ser preenchido"));

    return errors;
  }

  isValidDate(data: string): boolean {
    return new Date(data).toUTCString() !== 'Invalid Date';
  }
}