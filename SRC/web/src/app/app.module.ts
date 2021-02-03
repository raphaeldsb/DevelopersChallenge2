import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { ModalModule } from 'ngx-bootstrap/modal';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgxPaginationModule } from 'ngx-pagination';
import { OrderModule } from 'ngx-order-pipe';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavComponent } from '../nav/nav.component';
import { ListComponent } from '../list/list.component';
import { ToastrModule } from 'ngx-toastr';
import { BankTransactionsService } from '../services/BankTransactions.service';
import { DateTimeFormatPipePipe } from '../helps/DateTimeFormatPipe.pipe';
import { NgxUploaderModule } from 'ngx-uploader';

@NgModule({
  declarations: [		
    AppComponent,
    NavComponent,
    ListComponent,
    DateTimeFormatPipePipe
   ],
  imports: [  
    BrowserModule,
    NgxUploaderModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    NgxPaginationModule,
    OrderModule,
    BrowserAnimationsModule,
    BsDatepickerModule.forRoot(),
    ModalModule.forRoot(),
    TooltipModule.forRoot(),
    BsDropdownModule.forRoot(),
    ToastrModule.forRoot({
      timeOut: 3000,
      progressBar: true,
      closeButton: true
    })
  ],
  providers: [
    BankTransactionsService
  ],
  bootstrap: [AppComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class AppModule { }
