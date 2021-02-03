import { BankTransaction } from './BankTransaction';
import { Paging } from './Paging';

export class BankTransactions {
    paging!: Paging;
    items!: BankTransaction[];
}
