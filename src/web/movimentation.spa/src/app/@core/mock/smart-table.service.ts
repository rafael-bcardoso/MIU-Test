import { Injectable } from '@angular/core';
import { SmartTableData } from '../data/smart-table';

@Injectable()
export class SmartTableService extends SmartTableData {

  data = [{
    id: 1,
    customerName: 'Rafael Cardoso',
    cpf: '423.836.548-84',
    movimentationDate: '31/01/2020',
    tributeCode: '001',
    tributeDescription: 'Movimentação de Teste',
    tributeAliquot: '1000',
    movimentationGain: '100000',
    movimentationLoss: '1500'
  }, {
    id: 2,
    customerName: 'Pâmela Cardoso',
    cpf: '465.856.668-01',
    movimentationDate: '01/10/2020',
    tributeCode: '002',
    tributeDescription: 'Movimentação de Teste',
    tributeAliquot: '1000',
    movimentationGain: '100000',
    movimentationLoss: '1500'
  }];

  getData() {
    return this.data;
  }
}
