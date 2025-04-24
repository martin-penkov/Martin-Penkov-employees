
import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';

@Component({
  selector: 'app-header',
  imports: [],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent {
  fileName = '';

  constructor(private http: HttpClient) {}

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

          this.http.post('https://localhost:7133/api/employees', formData).subscribe({
            next: (response) => {
              console.log('File uploaded successfully:', response);
              // Optionally show success message or update UI
            },
            error: (err) => {
              console.error('Upload failed:', err);
              // Optionally show error message to user
            }
          });
      }
  }
}
