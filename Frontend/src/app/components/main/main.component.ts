import { Component, signal } from '@angular/core';
import { EmployeeDataService } from '../../services/employee-data.service';
import { EmployeePairData } from '../../types/EmployeePairData';
import { NgClass, NgFor } from '@angular/common';

@Component({
  selector: 'app-main',
  imports: [NgFor, NgClass],
  templateUrl: './main.component.html',
  styleUrl: './main.component.css'
})
export class MainComponent {
  _data = signal<EmployeePairData[]>([]);

  constructor(private dataService: EmployeeDataService) { }

  ngOnInit(): void {
    this.dataService.employeePairs$.subscribe(pairs => {
      let currentLongest = 0;
      let longestPairIndex = null;

      pairs.forEach((pair, index) => {
        if(pair.daysWorkedTogether >= currentLongest){
          longestPairIndex = index;
          currentLongest = pair.daysWorkedTogether;
        }
      });

      if(longestPairIndex != null){
        pairs[longestPairIndex].shouldHighlight = true;
      }

      this._data.set(pairs);
    });
  }
}
