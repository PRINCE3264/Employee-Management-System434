
// import { Component, inject } from '@angular/core';
// import { MatButtonModule } from '@angular/material/button';
// import { MatCardModule } from '@angular/material/card';
// import { MatDialog, MatDialogModule } from '@angular/material/dialog';
// import { ApplyLeaveComponent } from '../../componets/apply-leave/apply-leave.component';
// // Correct path

// @Component({
//   selector: 'app-employeedashboard',
//   standalone: true,
//   imports: [MatCardModule, MatButtonModule, MatDialogModule],
//   templateUrl: './employeedashboard.component.html',
//   styleUrls: ['./employeedashboard.component.css']
// })
// export class EmployeedashboardComponent {
//   private readonly dialog = inject(MatDialog);

//   applyLeave() {
//     this.openDialog();
//   }

//   openDialog(): void {
//     const dialogRef = this.dialog.open(ApplyLeaveComponent, {
//       panelClass: 'm-auto',
//       data: {} // Pass any required data here
//     });

//     dialogRef.afterClosed().subscribe((result: any) => {
//       console.log('Dialog closed', result);
//     });
//   }
// }


import { Component, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { ApplyLeaveComponent } from '../../componets/apply-leave/apply-leave.component';
import { environment } from '../../../environments/environment.development';
@Component({
  selector: 'app-employeedashboard',
  standalone: true,
  imports: [MatCardModule, MatButtonModule, MatDialogModule],
  templateUrl: './employeedashboard.component.html',
  styleUrls: ['./employeedashboard.component.css']
})
export class EmployeedashboardComponent {
  private readonly dialog = inject(MatDialog);
  private readonly http = inject(HttpClient);

  attendanceMarked = false; // optional flag to disable button after marking

  applyLeave() {
    this.openDialog();
  }

  openDialog(): void {
    const dialogRef = this.dialog.open(ApplyLeaveComponent, {
      panelClass: 'm-auto',
      data: {} // Pass any required data here
    });

    dialogRef.afterClosed().subscribe((result: any) => {
      console.log('Dialog closed', result);
    });
  }

  markAttendance(): void {
    const url = `${environment.apiBaseUrl}/Attendance/mark-present`;

    this.http.post(url, {}).subscribe({
      next: (res) => {
        console.log('✅ Attendance marked successfully:', res);
        alert('✅ Attendance marked!');
        this.attendanceMarked = true;
      },
      error: (err) => {
        console.error('❌ Error marking attendance:', err);
        alert('❌ Already marked or error occurred.');
      }
    });
  }


}
