/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { BankTransactionsService } from './BankTransactions.service';

describe('Service: BankTransactions', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [BankTransactionsService]
    });
  });

  it('should ...', inject([BankTransactionsService], (service: BankTransactionsService) => {
    expect(service).toBeTruthy();
  }));
});
