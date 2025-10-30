// import { Component, inject, OnInit } from '@angular/core';
// import { ActivatedRoute } from '@angular/router';
// import { CommonModule } from '@angular/common';
// import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
// import { TableComponent } from '../../componets/table/table.component';
// import { LeaveService } from '../../services/leave.service';
// import { PagedData } from '../../types/paged-data';
// import { AttendanceType, IAttendance } from '../../types/attendance';

// @Component({
//   selector: 'app-attendance',
//   standalone: true,
//   imports: [CommonModule, MatPaginatorModule, TableComponent],
//   templateUrl: './attendance.component.html',
//   styleUrls: ['./attendance.component.css']
// })
// export class AttendanceComponent implements OnInit {
//   private readonly leaveService = inject(LeaveService);
//   private readonly route = inject(ActivatedRoute);

//   employeeId!: number;
//   employeeName: string = '';

//   filter = {
//     pageIndex: 0,
//     pageSize: 5
//   };

//   showCols: any[] = [
//     {
//       key: 'date',
//       format: (row: IAttendance) => {
//         const date = new Date(row.date);
//         const day = date.getDate().toString().padStart(2, '0');
//         const month = (date.getMonth() + 1).toString().padStart(2, '0');
//         const year = date.getFullYear();
//         return `${day}/${month}/${year}`;
//       },
//     },
//     {
//       key: 'type',
//       format: (row: IAttendance) => {
//         switch (row.type) {
//           case AttendanceType.Leave:
//             return '<span class="text-red-500 font-semibold">🔴 Leave</span>';
//           case AttendanceType.Present:
//             return '<span class="text-green-600 font-semibold">🟢 Present</span>';
//           default:
//             return 'Unknown';
//         }
//       }
//     }
//   ];

//   data!: PagedData<IAttendance>;

//   ngOnInit(): void {
//     this.employeeId = Number(this.route.snapshot.paramMap.get('id'));
//     this.employeeName = this.route.snapshot.queryParamMap.get('name') || 'Employee';
//     this.getLatestData();
//   }

//   getLatestData(): void {
//     this.leaveService.getAttendanceHistoryByEmployee(this.employeeId, this.filter).subscribe(result => {
//       this.data = result;
//     });
//   }

//   onPageChange(event: PageEvent): void {
//     this.filter.pageIndex = event.pageIndex;
//     this.filter.pageSize = event.pageSize;
//     this.getLatestData();
//   }

//   handleRowAction(event: any) {
//     console.log('Row action triggered:', event);
//   }
// }

import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { TableComponent } from '../../componets/table/table.component';
import { LeaveService } from '../../services/leave.service';
import { PagedData } from '../../types/paged-data';
import { AttendanceType, IAttendance } from '../../types/attendance';

@Component({
  selector: 'app-attendance',
  standalone: true,
  imports: [CommonModule, MatPaginatorModule, TableComponent],
  templateUrl: './attendance.component.html',
  styleUrls: ['./attendance.component.css']
})
export class AttendanceComponent implements OnInit {
  private readonly leaveService = inject(LeaveService);
  private readonly route = inject(ActivatedRoute);

  employeeId: number = 0;
  employeeName: string = '';
  isAdminMode = false;

  filter = {
    pageIndex: 0,
    pageSize: 5
  };

  showCols: any[] = [
    {
      key: 'date',
      format: (row: IAttendance) => {
        const date = new Date(row.date);
        return `${date.getDate().toString().padStart(2, '0')}/${(date.getMonth() + 1).toString().padStart(2, '0')
          }/${date.getFullYear()}`;
      }
    },
    {
      key: 'type',
      format: (row: IAttendance) => {
        switch (row.type) {
          case AttendanceType.Leave:
            return '🔴 Leave';
          case AttendanceType.Present:
            return '🟢 Present';
          default:
            return '❓ Unknown';
        }
      }
    }
  ];

  data!: PagedData<IAttendance>;

  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get('id');
    this.employeeId = idParam ? +idParam : 0;
    this.employeeName = this.route.snapshot.queryParamMap.get('name') || 'You';

    // If route has valid employeeId, it's Admin mode
    this.isAdminMode = this.employeeId !== 0;

    this.getLatestData();
  }

  getLatestData(): void {
    if (this.isAdminMode) {
      this.leaveService.getAttendanceHistoryByEmployee(this.employeeId, this.filter).subscribe({
        next: (result) => (this.data = result),
        error: (err) => {
          console.error('❌ Admin: Error fetching attendance data:', err);
          alert('Admin failed to load attendance data.');
        }
      });
    } else {
      this.leaveService.getMyAttendanceHistory(this.filter).subscribe({
        next: (result) => (this.data = result),
        error: (err) => {
          console.error('❌ Employee: Error fetching your attendance:', err);
          alert('Failed to load your attendance data.');
        }
      });
    }
  }

  onPageChange(event: PageEvent): void {
    this.filter.pageIndex = event.pageIndex;
    this.filter.pageSize = event.pageSize;
    this.getLatestData();
  }

  handleRowAction(event: any) {
    console.log('Row action triggered:', event);
  }
}
