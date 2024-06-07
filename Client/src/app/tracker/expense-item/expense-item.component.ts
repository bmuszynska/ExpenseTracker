import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Expense } from 'src/app/models/expense';

@Component({
  selector: 'app-expense-item',
  templateUrl: './expense-item.component.html',
  styleUrls: ['./expense-item.component.scss']
})
export class ExpenseItemComponent {

  @Input() expense?: Expense;
  @Output() expenseDeleted = new EventEmitter<number>();
  
  deleteExpense(expenseId: number) {
    this.expenseDeleted.emit(expenseId);
  }
}
