import { Component, OnInit, EventEmitter } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { BankTransactionsService } from '../services/BankTransactions.service';
import { BankTransaction } from '../models/BankTransaction';
import { BankTransactions } from '../models/BankTransactions';
import { SearchPaging } from '../models/SearchPaging';
import { saveAs } from 'file-saver';

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.scss']
})

export class ListComponent implements OnInit {

  bankTransaction: BankTransaction = new BankTransaction();
  bankTransactions: BankTransaction[] = [];
  requestBankTransactions: BankTransactions = new BankTransactions();
  searchPaging: SearchPaging = new SearchPaging();
  registerForm: FormGroup;
  modaltitle = '';
  countTransactionsTotal = 0;
  files!: FileList;
  fileContent !: string;
  isOpen : boolean = false;

  config = {
    id: 'PaginationTransactions',
    itemsPerPage: 5,
    currentPage: 1,
    totalItems: 0
  };

  constructor(
      private fb: FormBuilder
    , private bankTransactionsService: BankTransactionsService
    , private toastr: ToastrService)
  {
    this.registerForm = this.fb.group({
      StartDate: [''],
      EndDate: ['']
    });
  }

  ngOnInit() {
    this.FilterBankTransactions();
  }

  pageChanged(event: any) {
    this.config.currentPage = event;
    this.searchPaging.Page = this.config.currentPage;
    this.searchPaging.Size = this.config.itemsPerPage;

    this.bankTransactionsService.GetTransactionsByFilters(this.searchPaging)
    .subscribe(
      (request: BankTransactions) => {
        this.bankTransactions = request.items;
        this.countTransactionsTotal = request.paging.total_items;
        this.config.totalItems = request.paging.total_items;
        }, error => {
          this.toastr.error(error.error);
        }
    );
  }
  
  FilterBankTransactions() {
    if (this.registerForm.valid) {
      this.searchPaging = Object.assign({}, this.registerForm.value);
      this.searchPaging.Page = this.config.currentPage;
      this.searchPaging.Size = this.config.itemsPerPage;

      this.bankTransactionsService.GetTransactionsByFilters(this.searchPaging)
      .subscribe(
        (request: BankTransactions) => {
          this.bankTransactions = request.items;
          this.countTransactionsTotal = request.paging.total_items;
          this.config.totalItems = request.paging.total_items;

          
          console.log(request);

          }, error => {
            this.toastr.error(error.error);
          }
      );
    }
  }

  exportToCsv() {
    if (this.registerForm.valid) {
      this.searchPaging = Object.assign({}, this.registerForm.value);

      this.bankTransactionsService.ExportToCsv(this.searchPaging)
      .subscribe(
        (data: any) => {
          const blob = new Blob([data], { type: 'application/octet-stream' });
          const fileName = 'BankTransactions.csv';
          saveAs(blob, fileName);
          this.toastr.success('Successfully generated CSV');
          }, error => {
            this.toastr.error(error.error);
          }
      );
    }
    else {
      this.toastr.error('Invalid parameters');
    }
  }

  inputOfxChange(event: any) {
    if (event.target.files && event.target.files.length) {
      this.files = event.target.files;
    }
  }

  importOFX(template: any) {
    let processed = 0;
    for (let index = 0; index < this.files.length; index++) {
      let file = this.files[index];

      this.bankTransactionsService.ImportOfx(file)
      .subscribe(
        () => {
          this.toastr.success(`${this.files[index].name} => Imported`);
          processed++;
          if (processed < this.files.length) {
            this.FilterBankTransactions();
          }
        }, error => {
          this.toastr.error(error.error.message);
          processed++;
          if (processed < this.files.length) {
            this.FilterBankTransactions();
          }
        }
      );

    }

    this.closeModal(template);
  }

  openModal(template: any) {
    this.isOpen = true;
    template.show();
  }

  itemsPerPage(qtd: any) {
    this.config.itemsPerPage = qtd as number;
    this.FilterBankTransactions()
  }

  closeModal(template: any) {
    this.isOpen = false; 
    template.hide();
  }

  delay(ms: number) {
    return new Promise( resolve => setTimeout(resolve, ms) );
  }
}