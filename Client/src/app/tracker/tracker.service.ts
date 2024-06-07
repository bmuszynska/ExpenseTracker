import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Expense } from '../models/expense';
import { Category } from '../models/category';
import { catchError } from 'rxjs/operators';
import { TrackerParams } from '../models/trackerParams';

import { Pagination } from '../models/pagination';
import { Observable } from 'rxjs/internal/Observable';
import { throwError } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class TrackerService {

  baseUrl = 'https://localhost:7217/api/';

  constructor(private http: HttpClient) { }

  getExpenses(trackerParams: TrackerParams) {
    let params = new HttpParams();

    if (trackerParams.categoryId)
      params = params.append('categoryId', trackerParams.categoryId.toString());
    if (trackerParams.month)
      params = params.append('month', trackerParams.month.toString());

    params = params.append('sort', trackerParams.sort);
    params = params.append('pageIndex', trackerParams.pageNumber);
    params = params.append('pageSize', trackerParams.pageSize);

    return this.http
      .get<Pagination<Expense[]>>(this.baseUrl + 'expenses', { params })
      .pipe(
        catchError((error) => {
          console.error('Error fetching expenses:', error);
          throw error; // Rethrow or handle as needed
        })
      );
  }

  getCategories() {
    return this.http.get<Category[]>(this.baseUrl + 'expenseCategories').pipe(
      catchError((error) => {
        console.error('Error fetching categories:', error);
        throw error; // Rethrow or handle as needed
      })
    );
  }

  addCategory(name: string) {
    console.log("inside tracker service ");
    return this.http.post<Category[]>(this.baseUrl + 'expenseCategories?name=' + name, {}).pipe(
      catchError((error) => {
        console.error('Error adding category:', error);
        throw error; // Rethrow or handle as needed
      })
    );
  }

  addExpense(expense: Expense): Observable<Expense[]> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    const payload = {
      categoryId: 1,
      description: expense.description,
      amount: expense.amount,
      date: expense.date
    };
    return this.http.post<Expense[]>(this.baseUrl + 'expenses', payload, { headers }).pipe(
      catchError((error) => {
        console.error('Error adding expense:', error);
        return throwError(error);
      })
    );
  }

  deleteExpense(id: number): Observable<any> {
    return this.http.delete(this.baseUrl + 'expenses/'+id);
  }

  deleteCategory(id: number): Observable<any> {
    return this.http.delete(this.baseUrl + 'expensecategories/'+id);
  }
}
