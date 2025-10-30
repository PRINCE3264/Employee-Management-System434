
import { Component, inject, OnInit } from '@angular/core';
import { HttpService } from './../../services/http.service';
import { IDepartment } from '../../types/department';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-departments',
  standalone: true,
  imports: [CommonModule, MatButtonModule, MatInputModule, MatFormFieldModule, FormsModule],
  templateUrl: './departments.component.html',
  styleUrls: ['./departments.component.css']
})
export class DepartmentsComponent implements OnInit {
  private httpService = inject(HttpService);

  departments: IDepartment[] = [];
  departmentName = '';
  selectedDepartmentId: number | null = null;

  isFormOpen = true;
  isEdit = false;

  ngOnInit() {
    this.loadDepartments();
  }

  loadDepartments() {
    this.httpService.getDepartments().subscribe(result => {
      this.departments = result;
    });
  }

  addDepartment() {
    if (!this.departmentName.trim()) return;

    this.httpService.addDepartment(this.departmentName).subscribe(response => {
      this.departments.push(response);
      this.resetForm();
    });
  }

  editDepartment(dept: IDepartment) {
    this.departmentName = dept.name;
    this.selectedDepartmentId = dept.id;
    this.isEdit = true;
    this.isFormOpen = false;
  }

  updateDepartment() {
    if (!this.departmentName.trim() || this.selectedDepartmentId === null) return;

    this.httpService.updateDepartment(this.selectedDepartmentId, this.departmentName)
      .subscribe(updated => {
        const index = this.departments.findIndex(d => d.id === this.selectedDepartmentId);
        if (index !== -1) this.departments[index].name = this.departmentName;
        this.resetForm();
      });
  }

  deleteDepartment(id: number) {
    this.httpService.deleteDepartment(id).subscribe(() => {
      this.departments = this.departments.filter(d => d.id !== id);
    });
  }

  resetForm() {
    this.departmentName = '';
    this.selectedDepartmentId = null;
    this.isEdit = false;
    this.isFormOpen = true;
  }
}



// import { Component, inject, OnInit } from '@angular/core';
// import { HttpService } from './../../services/http.service';
// import { IDepartment } from '../../types/department';
// import { MatButtonModule } from '@angular/material/button';
// import { MatInputModule } from '@angular/material/input';
// import { MatFormFieldModule } from '@angular/material/form-field';
// import { FormsModule } from '@angular/forms';
// import { CommonModule } from '@angular/common';

// @Component({
//   selector: 'app-departments',
//   standalone: true,
//   imports: [CommonModule, MatButtonModule, MatInputModule, MatFormFieldModule, FormsModule],
//   templateUrl: './departments.component.html',
//   styleUrls: ['./departments.component.css']
// })
// export class DepartmentsComponent implements OnInit {
//   private httpService = inject(HttpService);

//   departments: IDepartment[] = [];
//   departmentName = '';
//   selectedDepartmentId: number | null = null;

//   isFormOpen = true;
//   isEdit = false;

//   ngOnInit() {
//     this.loadDepartments();
//   }

//   loadDepartments() {
//     this.httpService.getDepartments().subscribe(result => {
//       this.departments = result;
//     });
//   }

//   addDepartment() {
//     if (!this.departmentName.trim()) return;

//     this.httpService.addDepartment(this.departmentName).subscribe(response => {
//       this.departments.push(response);
//       this.resetForm();
//     });
//   }

//   editDepartment(dept: IDepartment) {
//     this.departmentName = dept.name;
//     this.selectedDepartmentId = dept.id;
//     this.isEdit = true;
//     this.isFormOpen = false;
//   }

//   updateDepartment() {
//     if (!this.departmentName.trim() || this.selectedDepartmentId === null) return;

//     this.httpService.updateDepartment(this.selectedDepartmentId, this.departmentName)
//       .subscribe(updated => {
//         const index = this.departments.findIndex(d => d.id === this.selectedDepartmentId);
//         if (index !== -1) this.departments[index].name = this.departmentName;
//         this.resetForm();
//       });
//   }

//   deleteDepartment(id: number) {
//     if (!confirm('Are you sure you want to delete this department?')) return;

//     this.httpService.deleteDepartment(id).subscribe({
//       next: () => {
//         this.departments = this.departments.filter(d => d.id !== id);
//         alert('✅ Department deleted successfully.');
//       },
//       error: (err) => {
//         if (err.status === 400 && err.error?.message) {
//           alert('❌ ' + err.error.message);
//         } else {
//           alert('❌ Failed to delete department. Please try again.');
//         }
//       }
//     });
//   }

//   resetForm() {
//     this.departmentName = '';
//     this.selectedDepartmentId = null;
//     this.isEdit = false;
//     this.isFormOpen = true;
//   }
// }
