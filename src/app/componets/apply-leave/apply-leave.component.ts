
// import { CommonModule } from '@angular/common';
// import { Component, inject } from '@angular/core';
// import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
// import { MatButtonModule } from '@angular/material/button';
// import { MatDatepickerModule } from '@angular/material/datepicker';
// import { MatIconModule } from '@angular/material/icon';
// import { MatInputModule } from '@angular/material/input';
// import { MatSelectModule } from '@angular/material/select';
// import { LeaveService } from '../../services/leave.service';

// @Component({
//   selector: 'app-apply-leave',
//   standalone: true,
//   imports: [
//     CommonModule,
//     ReactiveFormsModule,
//     MatInputModule,
//     MatButtonModule,
//     MatSelectModule,
//     MatDatepickerModule,
//     MatIconModule
//   ],
//   templateUrl: './apply-leave.component.html',
//   styleUrls: ['./apply-leave.component.css']
// })
// export class ApplyLeaveComponent {
//   private fb = inject(FormBuilder);

//   leaveForm = this.fb.group({
//     type: [, [Validators.required]],
//     leaveDate: [, [Validators.required]],
//     reason: ['']
//   });
//   leaveService = inject(LeaveService)
//   onSubmit() {
//     if (this.leaveForm.invalid) {
//       alert("Please Select and Provide and field")
//     }
//     let leave: any = this.leaveForm.value;
//     this.leaveService.applyLeave(leave.type, leave.reason, leave.status).subscribe((result) => {
//       alert("Leave Apply Successfull.....!✅✅")
//     })
//   }
// }
import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { LeaveService } from '../../services/leave.service';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-apply-leave',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatInputModule,
    MatButtonModule,
    MatSelectModule,
    MatDatepickerModule,
    MatIconModule
  ],
  templateUrl: './apply-leave.component.html',
  styleUrls: ['./apply-leave.component.css']
})
export class ApplyLeaveComponent {
  private fb = inject(FormBuilder);
  private leaveService = inject(LeaveService);

  leaveForm = this.fb.group({
    type: [null, Validators.required],
    leaveDate: [null, Validators.required],
    reason: ['']
  });
  dialogRef = inject(MatDialogRef<ApplyLeaveComponent>)
  onSubmit() {
    if (this.leaveForm.invalid) {
      alert("❌ Please select all required fields.");
      return; // 🚫 Stop submission
    }

    const leave = this.leaveForm.value;
    this.leaveService.applyLeave(
      leave.type!,
      leave.reason || '',
      leave.leaveDate!
    ).subscribe(() => {
      alert("✅ Leave applied successfully!");
      this.leaveForm.reset();
      this.dialogRef.close() // Optionally reset the form
    });
  }
}
