import { MovimentationService } from './../../@core/services/movimentation.services';
import { Component, TemplateRef } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { NbDialogConfig, NbDialogRef, NbDialogService, NbIconLibraries } from '@nebular/theme';
import { LocalDataSource } from 'ng2-smart-table';
import { Validators } from '@angular/forms';
import { Movimentation } from '../../@core/models/movimentation';
import { SmartTableData } from '../../@core/data/smart-table';
import Swal from 'sweetalert2';

@Component({
  selector: 'ngx-ecommerce',
  templateUrl: './e-commerce.component.html',
  styleUrls: ['./e-commerce.component.scss'],
})
export class ECommerceComponent {
  evaIcons = [];
  modal: NbDialogRef<any>;
  icon = 'plus';
  iconMoney = 'coins';
  source: LocalDataSource = new LocalDataSource();
  movimentationForm: FormGroup;
  movimentationEdit: Movimentation;

  settings = {
    mode: 'external',
    editable: true,
    noDataMessage: 'Dados não encontrados',
    hideSubHeader: true,
    actions: {
      columnTitle: '',
    },
    add: {
      addButtonContent: '<i class="nb-plus"></i>',
      createButtonContent: '<i class="nb-checkmark"></i>',
      cancelButtonContent: '<i class="nb-close"></i>',
    },
    edit: {
      editButtonContent: '<i class="nb-edit"></i>',
      saveButtonContent: '<i class="nb-checkmark"></i>',
      cancelButtonContent: '<i class="nb-close"></i>',
    },
    delete: {
      deleteButtonContent: '<i class="nb-trash"></i>',
      confirmDelete: true,
    },
    columns: {
      tributeCode: {
        title: 'Código do Tributo',
        type: 'string',
      },
      customerName: {
        title: 'Nome Cliente',
        type: 'string',
      },
      cpf: {
        title: 'CPF',
        type: 'string',
      },
      movimentationDate: {
        title: 'Data da Movimentação',
        type: 'string',
      },
      tributeDescription: {
        title: 'Descrição do Tributo',
        type: 'string',
      },
      movimentationGain: {
        title: 'Valor do Rendimento da Movimentação',
        type: 'number',
      },
      tributeAliquot: {
        title: 'Alíquota do Tributo',
        type: 'number',
      },
      movimentationLoss: {
        title: 'Tributo da Movimentação',
        type: 'number',
      },
    },
  };

  constructor(iconsLibrary: NbIconLibraries, private dialogService: NbDialogService, private formBuilder: FormBuilder, private movimentationService: MovimentationService) {

    this.getMovimentations();

    this.evaIcons = Array.from(iconsLibrary.getPack('eva').icons.keys())
      .filter(icon => icon.indexOf('outline') === -1);

    iconsLibrary.registerFontPack('fa', { packClass: 'fa', iconClassPrefix: 'fa' });
    iconsLibrary.registerFontPack('far', { packClass: 'far', iconClassPrefix: 'fa' });
    iconsLibrary.registerFontPack('ion', { iconClassPrefix: 'ion' });

    this.movimentationForm = this.formBuilder.group({
      id: [''],
      customerName: ['', Validators.compose([Validators.required, Validators.min(10)])],
      tributeCode: [''],
      cpf: [''],
      movimentationDate: [''],
      tributeDescription: [''],
      tributeAliquot: [''],
      movimentationGain: [''],
      movimentationLoss: [''],
    });
  }

  ngOnInit(): void {

  }

  onDeleteMovimentation(event): void {
    let movimentationId = event.data.id;
    this.movimentationService.deleteMovimentation(movimentationId).subscribe(
      response => {
        if (response.statusCode == 200) {
          Swal.fire({
            title: 'Uhuuuul',
            text: response.message,
            icon: 'success',
            focusConfirm: true,
          });
          this.getMovimentations();
          this.modal.close();
        }
      }, error => {
        Swal.fire({
          title: 'Oops...',
          text: error["error"].errors.Messages[0],
          icon: 'error',
          focusConfirm: true,
        });
      })
  }

  onEditMovimentation(movimentation: Movimentation, dialog: TemplateRef<any>): void {
    this.movimentationForm = this.formBuilder.group({
      id: [movimentation.id],
      customerName: [movimentation.customerName],
      tributeCode: [movimentation.tributeCode],
      cpf: [movimentation.cpf],
      movimentationDate: [movimentation.movimentationDate],
      tributeDescription: [movimentation.tributeDescription],
      tributeAliquot: [movimentation.tributeAliquot],
      movimentationGain: [movimentation.movimentationGain],
      movimentationLoss: [movimentation.movimentationLoss],
    });

    this.modal = this.dialogService.open(
      dialog,
      {
        context: {
          title: 'Edit',
        },
        dialogClass: 'modal-xl'
      }
    );
  }

  updateMovimentation() {
    let id = this.movimentationForm.get('id').value;
    let cpf = this.movimentationForm.get('cpf').value;
    let customerName = this.movimentationForm.get('customerName').value;
    let movimentationDate = this.parseToStringDate(this.movimentationForm.get('movimentationDate').value);
    let tributeCode = this.movimentationForm.get('tributeCode').value;
    let tributeDescription = this.movimentationForm.get('tributeDescription').value;
    let tributeAliquot = this.movimentationForm.get('tributeAliquot').value;
    let movimentationGain = this.movimentationForm.get('movimentationGain').value;

    let movimentation = new Movimentation(id, customerName, cpf, movimentationDate, tributeCode, tributeDescription, tributeAliquot, movimentationGain);

    var errors = movimentation.isValid();
    if (errors.length > 0) {
      Swal.fire({
        title: 'Oops...',
        text: errors[0].message,
        icon: 'error',
        focusConfirm: true,
      });
      return;
    }

    this.movimentationService.updateMovimentation(movimentation).subscribe(response => {
      if (response.statusCode == 200) {
        Swal.fire({
          title: 'Uhuuuul',
          text: 'Movimentação atualizada com sucesso',
          icon: 'success',
          focusConfirm: true,
        });
        this.getMovimentations();
        this.modal.close();
      }
    }, error => {
      Swal.fire({
        title: 'Oops...',
        text: error["error"].errors.Messages[0],
        icon: 'error',
        focusConfirm: true,
      });
    });
  }

  addMovimentation(dialog: TemplateRef<any>): void {
    this.movimentationForm.reset();
    this.modal = this.dialogService.open(
      dialog,
      {
        context: {
          title: 'Add',
        },
        dialogClass: 'modal-xl'
      }
    );
  }

  cancelMovimentation(dialog: TemplateRef<any>): void {
    this.modal.close();
  }

  saveMovimentation() {
    let cpf = this.movimentationForm.get('cpf').value
    let customerName = this.movimentationForm.get('customerName').value;
    let movimentationDate = this.parseToStringDate(this.movimentationForm.get('movimentationDate').value);
    let tributeCode = this.movimentationForm.get('tributeCode').value;
    let tributeDescription = this.movimentationForm.get('tributeDescription').value;
    let tributeAliquot = this.movimentationForm.get('tributeAliquot').value;
    let movimentationGain = this.movimentationForm.get('movimentationGain').value;

    let movimentation = new Movimentation("", customerName, cpf, movimentationDate, tributeCode, tributeDescription, tributeAliquot, movimentationGain);

    var errors = movimentation.isValid();
    if (errors.length > 0) {
      Swal.fire({
        title: 'Oops...',
        text: errors[0].message,
        icon: 'error',
        focusConfirm: true,
      });
      return;
    }

    this.movimentationService.createMovimentation(movimentation).subscribe(response => {
      if (response.statusCode == 200) {
        Swal.fire({
          title: 'Uhuuuul',
          text: 'Movimentação cadastrada com sucesso',
          icon: 'success',
          focusConfirm: true,
        });
        this.getMovimentations();
        this.modal.close();
      }
    }, error => {
      Swal.fire({
        title: 'Oops...',
        text: error["error"].errors.Messages[0],
        icon: 'error',
        focusConfirm: true,
      });
    });
  }

  parseToStringDate(date: string): string {
    let dateSplit = date.split("/").join("");
    var dateFormated = dateSplit.substring(4, 8) + "-" + dateSplit.substring(2, 4) + "-" + dateSplit.substring(0, 2);
    return dateFormated;
  }

  getMovimentations() {
    this.movimentationService.getMovimentations().subscribe(data => {
      data.map(movimentation => movimentation.cpf = this.formataCPF(movimentation.cpf.number));
      data.map(movimentation => movimentation.movimentationDate = new Intl.DateTimeFormat('pt-BR').format(new Date(movimentation.movimentationDate)));
      this.source.load(data);
    });
  }

  formataCPF(cpf) {
    cpf = cpf.replace(/[^\d]/g, "");
    return cpf.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, "$1.$2.$3-$4");
  }
}
