import { Component, signal } from '@angular/core';
import { EmployeeDataService } from '../../services/employee-data.service';
import { EmployeePairData } from '../../types/EmployeePairData';
import { NgFor } from '@angular/common';

@Component({
  selector: 'app-main',
  imports: [NgFor],
  templateUrl: './main.component.html',
  styleUrl: './main.component.css'
})
export class MainComponent {
  _data = signal<EmployeePairData[]>([]);

  constructor(private dataService: EmployeeDataService) { }

  ngOnInit(): void {
    this.dataService.employeePairs$.subscribe(pairs => {
      this._data.set(pairs);
    });
  }
}
