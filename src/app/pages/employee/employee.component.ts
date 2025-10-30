


// import { Component, inject, OnInit } from '@angular/core';
// import { HttpService } from '../../services/http.service';
// import { MatTableModule } from '@angular/material/table';
// import { TableComponent } from '../../componets/table/table.component';
// import { IEmployee } from '../../types/employee';
// import { MatButtonModule } from '@angular/material/button';
// import { MatDialog } from '@angular/material/dialog';
// import { EmployeeFormComponent } from './employee-form/employee-form.component';
// import { MatInputModule } from '@angular/material/input';
// import { FormControl, ReactiveFormsModule } from '@angular/forms';
// import { MatFormFieldModule } from '@angular/material/form-field';
// import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
// @Component({
//   selector: 'app-employee',
//   standalone: true,
//   imports: [
//     MatTableModule,
//     TableComponent,
//     MatButtonModule,
//     MatInputModule,
//     ReactiveFormsModule,
//     MatFormFieldModule,
//     MatPaginatorModule,
//   ],
//   templateUrl: './employee.component.html',
//   styleUrls: ['./employee.component.css']
// })
// export class EmployeeComponent implements OnInit {

//   private httpService = inject(HttpService);
//   private dialog = inject(MatDialog);

//   employeeList: IEmployee[] = [];
//   totalCount = 0;
//   showCol: string[] = ['id', 'name', 'email', 'phone', 'action'];

//   searchControl = new FormControl('');
//   filter: any = {
//     search: '',
//     pageIndex: 0,
//     pageSize: 5
//   };

//   ngOnInit(): void {
//     this.getLatestData();

//     this.searchControl.valueChanges.subscribe((value: string | null) => {
//       this.filter.search = value ?? '';
//       this.filter.pageIndex = 0;
//       this.getLatestData();
//     });
//   }

//   getLatestData(): void {
//     this.httpService.getEmployeeList(this.filter).subscribe(result => {
//       this.employeeList = result.data;
//       this.totalCount = result.totalCount;
//     });
//   }

//   add(): void {
//     const ref = this.dialog.open(EmployeeFormComponent, {
//       panelClass: 'm-auto',
//       data: {}
//     });

//     ref.afterClosed().subscribe(result => {
//       if (result === true) {
//         this.getLatestData();
//       }
//     });
//   }

//   edit(employee: IEmployee): void {
//     const ref = this.dialog.open(EmployeeFormComponent, {
//       panelClass: 'm-auto',
//       data: { employeeId: employee.id },
//     });

//     ref.afterClosed().subscribe(result => {
//       if (result === true) {
//         this.getLatestData();
//       }
//     });
//   }

//   delete(employee: IEmployee): void {
//     if (confirm(`Are you sure you want to delete ${employee.name}?`)) {
//       this.httpService.deleteEmployee(employee.id).subscribe(() => {
//         alert('✅ Record deleted successfully.');
//         this.getLatestData();
//       });
//     }
//   }

//   markAttendance(employee: IEmployee): void {
//     console.log('📌 Marking attendance for:', employee);
//     // You can add attendance logic here
//   }


//   pageChange(event: PageEvent): void {
//     this.filter.pageIndex = event.pageIndex;
//     this.filter.pageSize = event.pageSize;
//     this.getLatestData();
//   }
// }


import { Component, inject, OnInit } from '@angular/core';
import { HttpService } from '../../services/http.service';
import { MatTableModule } from '@angular/material/table';
import { TableComponent } from '../../componets/table/table.component';
import { IEmployee } from '../../types/employee';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog } from '@angular/material/dialog';
import { EmployeeFormComponent } from './employee-form/employee-form.component';
import { MatInputModule } from '@angular/material/input';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { Router } from '@angular/router'; // ✅ Import Router

@Component({
  selector: 'app-employee',
  standalone: true,
  imports: [
    MatTableModule,
    TableComponent,
    MatButtonModule,
    MatInputModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatPaginatorModule,
  ],
  templateUrl: './employee.component.html',
  styleUrls: ['./employee.component.css']
})
export class EmployeeComponent implements OnInit {

  private httpService = inject(HttpService);
  private dialog = inject(MatDialog);
  private router = inject(Router); // ✅ Inject Router

  employeeList: IEmployee[] = [];
  totalCount = 0;
  showCol: string[] = ['id', 'name', 'email', 'phone', 'action'];

  searchControl = new FormControl('');
  filter: any = {
    search: '',
    pageIndex: 0,
    pageSize: 5
  };

  ngOnInit(): void {
    this.getLatestData();

    this.searchControl.valueChanges.subscribe((value: string | null) => {
      this.filter.search = value ?? '';
      this.filter.pageIndex = 0;
      this.getLatestData();
    });
  }

  getLatestData(): void {
    this.httpService.getEmployeeList(this.filter).subscribe(result => {
      this.employeeList = result.data;
      this.totalCount = result.totalCount;
    });
  }

  add(): void {
    const ref = this.dialog.open(EmployeeFormComponent, {
      panelClass: 'm-auto',
      data: {}
    });

    ref.afterClosed().subscribe(result => {
      if (result === true) {
        this.getLatestData();
      }
    });
  }

  edit(employee: IEmployee): void {
    const ref = this.dialog.open(EmployeeFormComponent, {
      panelClass: 'm-auto',
      data: { employeeId: employee.id },
    });

    ref.afterClosed().subscribe(result => {
      if (result === true) {
        this.getLatestData();
      }
    });
  }

  delete(employee: IEmployee): void {
    if (confirm(`Are you sure you want to delete ${employee.name}?`)) {
      this.httpService.deleteEmployee(employee.id).subscribe(() => {
        alert('✅ Record deleted successfully.');
        this.getLatestData();
      });
    }
  }

  markAttendance(employee: IEmployee): void {
    // ✅ Navigate to attendance page with ID and name
    this.router.navigate(['/attendance', employee.id], {
      queryParams: { name: employee.name }
    });
  }

  pageChange(event: PageEvent): void {
    this.filter.pageIndex = event.pageIndex;
    this.filter.pageSize = event.pageSize;
    this.getLatestData();
  }
}
