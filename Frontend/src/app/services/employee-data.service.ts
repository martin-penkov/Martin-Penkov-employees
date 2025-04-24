import { Injectable } from '@angular/core';
import { EmployeePairData } from '../types/EmployeePairData.js';
import { BehaviorSubject } from 'rxjs/internal/BehaviorSubject';

@Injectable({
  providedIn: 'root'
})
export class EmployeeDataService {
  private employeePairData = new BehaviorSubject<EmployeePairData[]>([]);
  employeePairs$ = this.employeePairData.asObservable();

  setData(data: any): void {
    this.employeePairData.next(data);
  }
}
