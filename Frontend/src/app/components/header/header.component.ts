
import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { EmployeeDataService } from '../../services/employee-data.service';

@Component({
  selector: 'app-header',
  imports: [],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent {
  fileName = '';

  constructor(private http: HttpClient, private dataService: EmployeeDataService) {}

  onFileSelected(event: Event) {
      const input = event.target as HTMLInputElement;
      if(input.files == null || input.files.length == 0) {
        return;
      }

      const file: File = input.files[0];

      if (file) {

          this.fileName = file.name;

          const formData = new FormData();

          formData.append('file', file);

          this.http.post('https://localhost:7133/api/employees/ProcessCsvDataAllEmployeePairs', formData).subscribe({
            next: (response) => {
              console.log('File uploaded successfully:', response);
              this.dataService.setData(response);
            },
            error: (err) => {
              console.error('Upload failed:', err);
            }
          });
      }
  }
}
