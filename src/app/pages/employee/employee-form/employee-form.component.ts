
import { Component, inject } from '@angular/core';
import { FormBuilder, Validators, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatRadioModule } from '@angular/material/radio';
import { CommonModule } from '@angular/common';
import { IDepartment } from '../../../types/department';
import { HttpService } from '../../../services/http.service';
import { IEmployee, ICreateEmployee } from '../../../types/employee';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';

@Component({
  selector: 'app-employee-form',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    FormsModule,
    MatInputModule,
    MatSelectModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatButtonModule,
    MatCardModule,
    MatRadioModule,
    CommonModule,
    MatFormFieldModule
  ],
  templateUrl: './employee-form.component.html',
  styleUrls: ['./employee-form.component.css']
})
export class EmployeeFormComponent {


  private fb = inject(FormBuilder);
  private httpService = inject(HttpService);
  private dialogRef = inject(MatDialogRef<EmployeeFormComponent>);
  data = inject<any>(MAT_DIALOG_DATA);

  employeeForm = this.fb.group({
    name: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    phone: [''],
    jobTitle: ['', Validators.required],
    gender: ['', Validators.required],
    departmentId: [null, Validators.required],
    joiningDate: ['', Validators.required],
    lastWorkingDate: ['', Validators.required],
    dateOfBirth: ['', Validators.required]
  });

  genders = ['Male', 'Female', 'Other'];
  jobTitles = ['Web Developer', '.NET Developer', 'Flutter Developer', 'Java Developer'];
  departments: IDepartment[] = [];

  ngOnInit() {
    this.httpService.getDepartments().subscribe((result) => {
      this.departments = result;
    });

    if (this.data?.employeeId) {
      this.httpService.getEmployeeById(this.data.employeeId).subscribe((result: IEmployee) => {
        const { id, ...rest } = result;
        this.employeeForm.patchValue(rest as any);
        this.employeeForm.get('gender')?.disable();
        this.employeeForm.get('dateOfBirth')?.disable();
        this.employeeForm.get('joiningDate')?.disable();
      });
    }
  }

  onSubmit() {
    if (this.employeeForm.invalid) return;

    const rawValue = this.employeeForm.getRawValue();
    console.log('Submitting:', rawValue);
    if (this.data?.employeeId) {
      this.httpService.updateEmployee(this.data.employeeId, rawValue as any).subscribe(() => {
        alert('Record Updated ✅');
        this.dialogRef.close(true);
      });
    } else {
      this.httpService.addEmployee(rawValue as any).subscribe(() => {
        alert('Record Saved ✅');
        this.dialogRef.close(true);
      });
    }
  }
}
