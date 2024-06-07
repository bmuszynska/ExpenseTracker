import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms'
import { Category } from 'src/app/models/category';
import { Expense } from 'src/app/models/expense';

@Component({
  selector: 'app-new-expense',
  templateUrl: './new-expense.component.html',
  styleUrls: ['./new-expense.component.scss']
})
export class NewExpenseComponent {
  expenseForm: FormGroup;
  expensePopUp = false;
  @Output() expenseAdded = new EventEmitter<Expense>();
  @Input() categories?: Category[];

  constructor(private fb: FormBuilder) {
    this.expenseForm = this.fb.group({
      category: [null, Validators.required],
      description: ['', Validators.required],
      amount: [0, [Validators.required, Validators.min(0.01)]],
      date: ['', Validators.required]
    });
  }

  openPopUp() {
    this.expensePopUp = true;
  }

  close() {
    this.expensePopUp = false;
  }

  save() {
    if (this.expenseForm.valid) {
      const newExpense = { ...this.expenseForm.value} as Expense;
      this.expenseAdded.emit(newExpense);
      this.close();
    }
  }
}